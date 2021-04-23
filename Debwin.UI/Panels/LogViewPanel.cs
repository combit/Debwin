using combit.DebwinExtensions.MessageSources;
using Debwin.Core;
using Debwin.Core.Metadata;
using Debwin.Core.Views;
using Debwin.UI.Forms;
using Debwin.UI.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Debwin.UI.Panels
{

    /// <summary>
    /// Manages and displays a stack of views to a log. Each view on the stack is a filtered view based on the previous view:
    /// Root Log -> Filtered By Level -> Filtered By Level & Thread  -> ...
    /// </summary>
    public partial class LogViewPanel : DockContent //, ILogViewObserver
    {

        public event EventHandler<LogMessageSelectedEventArgs> SelectedLogMessage;

        private readonly ILogController _logController;
        private readonly List<IQueryableLogView> _logViewStack;
        private readonly IMainWindow _mainWindow;
        private readonly IUserPreferences _userPreferences;
        private bool _autoScrollActive;
        private bool _autoScrollForced;  // for ring-buffering
        private static float _defaultListLogMessageFontSize = 8.25F;
        private DateTime? _baseTimestamp;   // If set, marks the base value for the display of relative timestamps

        public LogViewPanel(IMainWindow mainWindow, IUserPreferences userPreferences, ILogController logController, LogViewPanelOptions options)
        {
            _logViewStack = new List<IQueryableLogView>();
            _logController = logController;
            _mainWindow = mainWindow;
            _userPreferences = userPreferences;
            _autoScrollActive = true;
            InitializeComponent();

            CreateDefaultColumns(options.DefaultColumns);
            lstLogMessages.Resize += new System.EventHandler(this.lstLogMessages_Resize);
            SetTabTitle(_logController.Name);
            UpdateSavedFilterMenu();

            listviewLayoutTimer.Enabled = false;

            SetIsLoadingItems(true);
            txtSearchBox.Items.AddRange(userPreferences.SearchHistory.ToArray());

            if (!options.LiveCaptureMode)   // Disable controls which are not relevant for logs that do not change after the initial loading (log from file etc.)
            {
                btnToggleAutoScroll.Visible = false;
                btnClearLog.Visible = false;
                autoScrollToolstripSeparator.Visible = false;
                btnAppendMessage.Visible = false;
                btnCaptureActive.Visible = false;
                _autoScrollActive = false;
            }

            if (!DesignMode)
            {
                lstLogMessages.RetrieveVirtualItem += LstLogMessages_RetrieveVirtualItem;
                listviewLayoutTimer.Start();

                _userPreferences.PropertyChanged += userPreferences_PropertyChanged;
                _userPreferences.SavedFilters.ItemsChanged += SavedFilters_ItemsChanged;
                _logController.PropertyChanged += logController_PropertyChanged;
                _logController.ReceivedControlMessage += logController_ReceivedControlMessage;
            }

            SetupLongTermMonitoring();

            this.FormClosed += LogViewPanel_FormClosed;
        }


        private void SavedFilters_ItemsChanged(object sender, EventArgs e)
        {
            UpdateSavedFilterMenu();
        }

        private void UpdateSavedFilterMenu()
        {
            btnSavedFilters.DropDownItems.Clear();
            if (_userPreferences.SavedFilters.Count == 0)
            {
                btnSavedFilters.Enabled = false;
            }
            else
            {
                foreach (var filter in _userPreferences.SavedFilters)
                {
                    var menuItem = btnSavedFilters.DropDownItems.Add(filter.Name);
                    menuItem.Tag = filter;
                    menuItem.Click += savedFilterMenuItem_OnClick;
                }
                btnSavedFilters.Enabled = true;
            }
        }

        private void savedFilterMenuItem_OnClick(object sender, EventArgs e)
        {
            PushFilteredView((sender as ToolStripItem).Tag as FilterDefinition, true);
        }

        private void logController_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ILogController.Name))
            {
                SetTabTitle(_logController.Name);
            }
        }


        private void logController_ReceivedControlMessage(object sender, ReceivedControlMessageEventArgs e)
        {
            if (e.ControlMessage is ClearAllViewsOfControllerControlMessage)
            {
                this.Invoke(new Action(() => this.ClearLogViews()));
            }
        }

        private void SetTabTitle(string name)
        {
            this.Text = this.TabText = (name ?? "Log").Replace("&", "&&");  // prevent mnemonics
        }

        private void LogViewPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            lstLogMessages.RetrieveVirtualItem -= LstLogMessages_RetrieveVirtualItem;
            SetIsLoadingItems(true);

            lstLogMessages.SelectedIndices.Clear();   // triggers selection of "null" item => clear details window etc.

            // First close the opened log views
            for (int i = _logViewStack.Count - 1; i >= 0; i--)
            {
                //_logViewStack[i].SetObserver(null);
                _logController.RemoveLogView(_logViewStack[i]);
                _logViewStack.RemoveAt(i);
            }

            _logController.PropertyChanged -= logController_PropertyChanged;
            _logController.ReceivedControlMessage -= logController_ReceivedControlMessage;

            _userPreferences.PropertyChanged -= userPreferences_PropertyChanged;
            _userPreferences.SavedFilters.ItemsChanged -= SavedFilters_ItemsChanged;

            SaveColumnLayoutForControllerType();
        }

        private void SaveColumnLayoutForControllerType()
        {
            if (String.IsNullOrEmpty(_logController.Name))
                return;

            // Get types and width of the columns
            var columnSet = new List<LogViewPanelColumnDefinition>(lstLogMessages.Columns.Count);
            foreach (ColumnHeader listviewColumn in lstLogMessages.Columns.OfType<ColumnHeader>().OrderBy(col => col.DisplayIndex))  // the columns must be ordered by DisplayIndex, as the user might have changed the order by drag&drop
            {
                columnSet.Add(new LogViewPanelColumnDefinition() { MessagePropertyID = (int)listviewColumn.Tag, Width = listviewColumn.Width });
            }

            // And save to user preferences file
            var existingSet = _userPreferences.LogViewPanelColumnSets.Find(set => set.ControllerName == _logController.Name);
            if (existingSet != null)
                _userPreferences.LogViewPanelColumnSets.Remove(existingSet);

            _userPreferences.LogViewPanelColumnSets.Add(new LogViewPanelColumnSet()
            {
                ControllerName = _logController.Name,
                Columns = columnSet
            });
        }

        public ILogController LogController { get { return _logController; } }

        // Listens if a setting was changed that affects this panel
        private void userPreferences_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IUserPreferences.TimeFormat):
                case nameof(IUserPreferences.TimeFormatMode):
                case nameof(IUserPreferences.DateTimeFormat):
                case nameof(IUserPreferences.ShowTabChars):
                    lstLogMessages.Invalidate();
                    break;
            }
        }

        /// <summary>
        /// Adds the log view to the stack and sets it as the new view to display.
        /// </summary>
        public void PushLogView(IQueryableLogView logView)
        {
            _logViewStack.Add(logView);
            SetIsLoadingItems(false);
            RedrawView(logView);

            if (_logViewStack.Count > 1)
            {
                btnPopLogView.Enabled = true;
                miSelectInUnfilteredView.Enabled = true;
            }
            //this.EnableAutoScroll();
        }

        /// <summary>
        /// Shows or hides the item list / the "working on it..." message
        /// </summary>
        /// <param name="isLoading">True to hide the items list.</param>
        private void SetIsLoadingItems(bool isLoading)
        {
            if (isLoading)
            {
                panelLogView.Dock = DockStyle.None;
                panelLogView.Hide();
                panelLogLoader.Dock = DockStyle.Fill;
                panelLogLoader.Show();
            }
            else
            {
                panelLogLoader.Dock = DockStyle.None;
                panelLogLoader.Hide();
                panelLogView.Dock = DockStyle.Fill;
                panelLogView.Show();

                //logView.SetObserver(this);

            }
        }

        /// <summary>
        /// Releases the currently displayed view and restores the previous view (= undo filter).
        /// </summary>
        private void PopLogView()
        {
            // free current view, if it is not the root view
            var displayedView = GetCurrentLogView();
            if (displayedView != null && _logViewStack.Count > 1)
            {
                //displayedView.SetObserver(null);
                LogController.RemoveLogView(displayedView);
                _logViewStack.RemoveAt(_logViewStack.Count - 1);
            }



            if (_logViewStack.Count == 1)  // root view is active now
            {
                btnPopLogView.Enabled = false;

                miSelectInUnfilteredView.Enabled = false;
            }

            // show next logview from stack (may be null!)
            var nextDisplayedView = GetCurrentLogView();
            if (nextDisplayedView != null)
            {
                SetIsLoadingItems(false);
                RedrawView(nextDisplayedView);
            }
            else
            {
                SetIsLoadingItems(true);
            }
        }

        /// <summary>
        /// Returns the top item of the log view stack.
        /// </summary>
        private IQueryableLogView GetCurrentLogView()
        {
            if (_logViewStack.Count == 0)
                return null;

            return _logViewStack[_logViewStack.Count - 1];
        }

        /// <summary>
        /// Returns the bottom item of the log view stack.
        /// </summary>
        private IQueryableLogView GetRootLogView()
        {
            if (_logViewStack.Count == 0)
                return null;

            return _logViewStack[0];
        }


        private void GoToRootView()
        {
            while (GetCurrentLogView() != GetRootLogView())
            {
                PopLogView();
            }
        }

        public void ShowChooseColumnsDialog()
        {
            // Prepare a list of the currently displayed properties from the visible list columns
            var currentlySelectedPropertyIDs = lstLogMessages.Columns.Cast<ColumnHeader>().Select(column => (int)column.Tag);

            // Let the user choose which properties to show
            var selectedColumnsDialog = new LogViewPanelColumnsDialog(GetLogMessagePropertyInfo(), currentlySelectedPropertyIDs.ToList());
            if (_mainWindow.ShowDialogEx(selectedColumnsDialog) == DialogResult.OK)
            {
                // Create new columns for the list, while restoring the width of columns that have existed before
                var newColumns = selectedColumnsDialog.SelectedPropertyIDs.Select(propID => new LogViewPanelColumnDefinition()
                {
                    MessagePropertyID = propID,
                    Width = lstLogMessages.Columns.Cast<ColumnHeader>().FirstOrDefault(c => (int)c.Tag == propID)?.Width ?? 150   // get previous column width or use default
                });

                CreateColumnsForColumnSet(newColumns.ToArray());   // ToArray() is required to execute the Linq before the listview is cleared
            }
        }

        private IEnumerable<LogMessagePropertyInfo> GetLogMessagePropertyInfo()
        {
            // We need to get a list of all properties that the messages in the view could have.
            // When messages from two sources are combined in one log controller, two different messages might have two different sets of properties
            // => get all message types, create an instance of each type, ask that instance for it's custom properties, and get the metadata for each of that properties:
            var allCustomPropertyInfos = _logController
                .GetMessageCollectors()
                .SelectMany(mc => mc.Parser.GetSupportedMessageTypeCodes()).Distinct()
                .Select(typeCode => LogMessageFactory.GetFactoryForTypeCode(typeCode).Invoke())
                .SelectMany(message => message.GetCustomPropertyIDs().Select(propId => message.GetPropertyMetadata(propId)));

            // Then combine the custom properties with the built-in properties of each Debwin4 message object:
            LogMessage baseMessage = new LogMessage();
            var allStandardPropertyInfos = baseMessage.GetStandardPropertyIDs().Select(propId => baseMessage.GetPropertyMetadata(propId));
            return Enumerable.Concat(allStandardPropertyInfos, allCustomPropertyInfos);
        }

        private void CreateDefaultColumns(LogViewPanelColumnSet defaultColumns = null)
        {
            // Try to get previously saved columns for this controller type (e.g. RS/cRM/...)
            var savedColumnSet = _userPreferences.LogViewPanelColumnSets.FirstOrDefault(set => set.ControllerName == _logController.Name);

            // Use a default 
            if (savedColumnSet == null)
            {
                savedColumnSet = defaultColumns ?? new LogViewPanelColumnSet()
                {
                    ControllerName = null,
                    Columns = new List<LogViewPanelColumnDefinition>()
                    {
                        new LogViewPanelColumnDefinition() { MessagePropertyID = PropertyIdentifiers.PROPERTY_LEVEL, Width = 25 },
                        new LogViewPanelColumnDefinition() { MessagePropertyID = PropertyIdentifiers.PROPERTY_MODULE_NAME, Width = 80 },
                        new LogViewPanelColumnDefinition() { MessagePropertyID = PropertyIdentifiers.PROPERTY_LOGGER_NAME, Width = 120 },
                        new LogViewPanelColumnDefinition() { MessagePropertyID = PropertyIdentifiers.PROPERTY_THREAD, Width = 60 },
                        new LogViewPanelColumnDefinition() { MessagePropertyID = PropertyIdentifiers.PROPERTY_TIMESTAMP, Width = 170 },
                        new LogViewPanelColumnDefinition() { MessagePropertyID = PropertyIdentifiers.PROPERTY_MESSAGE, Width = 350 }
                    }
                };
            }

            CreateColumnsForColumnSet(savedColumnSet.Columns);
        }

        private void CreateColumnsForColumnSet(IEnumerable<LogViewPanelColumnDefinition> columns)
        {
            lstLogMessages.Columns.Clear();

            // Get a list of all known LogMessage subtypes that the curent message parser(s) could create, so we can ask for the property metadata corresponding to a property ID
            var messageTypes = _logController
                .GetMessageCollectors()
                .SelectMany(mc => mc.Parser.GetSupportedMessageTypeCodes()).Distinct()
                .Select(typeCode => LogMessageFactory.GetFactoryForTypeCode(typeCode).Invoke());

            // Create the columns in the listview
            foreach (var column in columns)
            {
                // Get metadata for the message property corresponding to the column
                LogMessagePropertyInfo propertyInfo = null;
                foreach (var knownMessageType in messageTypes)
                {
                    propertyInfo = knownMessageType.GetPropertyMetadata(column.MessagePropertyID);
                    if (propertyInfo != null) break;
                }

                if (propertyInfo == null)
                    continue;

                CreateColumn(propertyInfo.UIName, column.Width, GetAlignmentForColumnType(propertyInfo), column.MessagePropertyID);
            }
        }

        private HorizontalAlignment GetAlignmentForColumnType(LogMessagePropertyInfo propInfo)
        {
            if (propInfo.DataType == typeof(DateTime))
                return HorizontalAlignment.Right;
            else if (propInfo.PropertyID == PropertyIdentifiers.PROPERTY_THREAD)
                return HorizontalAlignment.Right;
            else
                return HorizontalAlignment.Left;
        }

        private void CreateColumn(string title, int width, HorizontalAlignment align, int propertyIdentifier)
        {
            if (propertyIdentifier == PropertyIdentifiers.PROPERTY_LEVEL)
                title = string.Empty;
            lstLogMessages.Columns.Add(new ColumnHeader() { Text = title, Width = width, TextAlign = align, Tag = propertyIdentifier });
        }


        public void HandleApplyFilterRequest(ApplyFilterEventArgs e)
        {
            // Only for small logs we should allow the automatically triggered filters that are created while typing in the filter panel ("instant search")
            if (e.IsAutoTriggeredFilter && _logViewStack[0].MessageCount > 100000)
                return;

            bool setFocusToListView = !e.IsAutoTriggeredFilter;   // don't steal focus while user is typing in the filter panel
            PushFilteredView(e.FilterDefinition, setFocusToListView);
        }

        /// <summary>
        /// Creates a new log view on the stack using the provided filter.
        /// </summary>
        /// <param name="extendCurrentFilter">True to create a sub view of the current view, false to filter based on the root (unfiltered) view.</param>
        public void PushFilteredView(FilterDefinition requestedFilter, bool setListFocusAfterFiltering)
        {
            var previouslySelectedMsg = GetSelectedLogMessage();

            GoToRootView();
            var parentView = GetCurrentLogView();

            if (parentView.MessageCount > 50000)
            {
                SetIsLoadingItems(true);
            }

            Task t = Task.Factory.StartNew(() =>
            {
                var filteredView = new MemoryBasedLogView(parentView, requestedFilter);
                LogController.AddView(filteredView);

                Invoke(new Action(() =>
                {
                    PushLogView(filteredView);

                    // Try to restore the selection if the previously selected message is contained in the filtered view
                    if (previouslySelectedMsg != null)
                    {
                        int newIndexOfSelectedMsg = filteredView.FindIndexOfMessage(false, 0, msg => ReferenceEquals(msg, previouslySelectedMsg));
                        if (newIndexOfSelectedMsg != -1)
                            SetSelectedMessageByIndex(newIndexOfSelectedMsg, false);
                    }

                    if (setListFocusAfterFiltering)
                        lstLogMessages.Focus();
                }));
            });
        }


        private int _layoutTimerTicks = 0;

        private void listviewLayoutTimer_Tick(object sender, EventArgs e)
        {
            var currentView = GetCurrentLogView();

            if (DesignMode || currentView == null)  // for some reason the VS designer always enables the timer...
                return;

            if (_layoutTimerTicks == 10)  // only once per second
            {
                // Check if the maximum message count in the view was reached and a backup log file has been attached (show a link in the status bar then)
                var attachedFileWriter = _logController.GetLogViews().OfType<FileBasedLogView>().FirstOrDefault();
                if (attachedFileWriter != null)
                {
                    lblAttachedLogFileName.Visible = lblAttachedFileNotification.Visible = true;
                    lblLineNumberLabel.Spring = false;
                    lblAttachedFileNotification.Text = (attachedFileWriter.IsOverflowFile ? "Message limit was reached. Dropping old messages!" : "Messages are also saved to the disk.") + " Full Log:";
                    lblAttachedLogFileName.Tag = attachedFileWriter.FilePath;
                    lblAttachedLogFileName.Text = Path.GetFileName(attachedFileWriter.FilePath);
                }
                else
                {
                    lblAttachedLogFileName.Visible = lblAttachedFileNotification.Visible = false;
                    lblLineNumberLabel.Spring = true;
                }

                // adjust state of the capturing active button:
                btnCaptureActive.Checked = (_logController.GetMessageCollectors().Any(c => !c.IsStopped));

                _layoutTimerTicks = 0;
            }
            _layoutTimerTicks++;

            listviewLayoutTimer.Enabled = false;
            RedrawView(currentView, false);
            listviewLayoutTimer.Enabled = true;

            // If the internal view is log view is ring-buffering, auto scroll should be always enabled because the messages in the list are moving anyway (virtual index changes on new messages)
            if (GetRootLogView() is ISupportsMaximumMessageCount logView)
            {
                _autoScrollForced = (logView.IsRingBuffering && btnCaptureActive.Checked);
            }

            UpdateToolbarButtonStates();
        }

        private void UpdateToolbarButtonStates()
        {
            btnToggleAutoScroll.Enabled = btnCaptureActive.Checked && !_autoScrollForced;
            btnToggleAutoScroll.Checked = btnCaptureActive.Checked && (_autoScrollActive || _autoScrollForced);
        }

        /// <summary>
        /// Holds the number of messages per view. If the message count is unchanged and a redraw is not forced, there is no need to redraw the messages.
        /// </summary>
        Dictionary<IQueryableLogView, int> _messageCounts = new Dictionary<IQueryableLogView, int>();

        /// <summary>
        /// Adapts the rows to the count of messages in the log view and performs the auto-scrolling if enabled.
        /// </summary>
        /// <param name="currentView">View to use for the virtual list size.</param>
        /// <param name="forceRedraw">Force the redraw, even if no new messages are available.</param>
        private void RedrawView(IQueryableLogView currentView, bool forceRedraw = true)
        {
            if (!_mainWindow.IsVisible())   // Bug in Setter of ListView.VirtualListSize may throw a NullRefException if the control is not visible
                return;

            int lastMessageCount;

            if (!_messageCounts.TryGetValue(currentView, out lastMessageCount))
            {
                lastMessageCount = 0;
            }

            if ((currentView.MessageCount != lastMessageCount) || forceRedraw)
            {
                _messageCounts[currentView] = currentView.MessageCount;

                // Workaround for bug in ListView: The ListView starts flickering as soon as the virtual list size is modified
                // and an item which is not in the visible scroll area is selected & focused => we keep the selected item, or, if it is not visible, the first visible item focused
                if (lstLogMessages.SelectedIndices.Count > 0 && !(Control.ModifierKeys == Keys.Shift))   // when shift is pressed, user wants to select items => don`t change the focused item
                {
                    // Remove the focus from the old item so we don't run into another virtualmode listview bug/sideeffect of the workaround where 
                    // multiple rows have the focus rect when scrolling upwards while a row is selected
                    if (lstLogMessages.FocusedItem != null)
                    {
                        lstLogMessages.FocusedItem.Focused = false;
                    }

                    if (lstLogMessages.TopItem != null)
                    {
                        Rectangle selectedItemArea = lstLogMessages.Items[lstLogMessages.SelectedIndices[0]].Bounds;
                        Rectangle listviewClientArea = lstLogMessages.ClientRectangle;
                        int headerHeight = lstLogMessages.TopItem.Bounds.Top;

                        if (selectedItemArea.Y + selectedItemArea.Height > headerHeight && selectedItemArea.Y + selectedItemArea.Height < listviewClientArea.Height)   // if the selected item is in the visible region 
                        {
                            lstLogMessages.Items[lstLogMessages.SelectedIndices[0]].Focused = true;
                        }
                        else
                        {
                            lstLogMessages.TopItem.Focused = true;  // if selected item is not visible, set the focus (not the selection!) on the first visible item
                        }
                    }
                }
                lstLogMessages.VirtualListSize = currentView.MessageCount;

                if ((_autoScrollActive || _autoScrollForced) && lstLogMessages.VirtualListSize != 0)
                {
                    lstLogMessages.EnsureVisible(lstLogMessages.VirtualListSize - 1);
                }
                lstLogMessages.Invalidate();
            }
            lblVisibleMessageCount.Visible = lblVisibleMessages.Visible = lblFilterNameLabel.Visible = lblFilterNameValue.Visible = (currentView.FilterPredicate != null);
            lblFilterNameValue.Text = currentView.FilterPredicate?.Name ?? "(temporary)";
            lblVisibleMessageCount.Text = currentView.MessageCount.ToString("N0");
            lblBufferedMessagesCount.Text = GetRootLogView().MessageCount.ToString("N0");
            lblReceivedMessageCount.Text = _logController.TotalReceivedMessages.ToString("N0");
            var selectedIndices = lstLogMessages.SelectedIndices;
            lblLineNumber.Text = (selectedIndices.Count != 0 ? selectedIndices[0]+1 : 0).ToString("N0");
        }


        private void lstLogMessages_Resize(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            // Adapt the width of the last column to the available listview width, width a minimum width of 1200px:
            if (lstLogMessages.Columns.Count > 1)
            {
                int usedWidth = 0;
                for (int i = 0; i < lstLogMessages.Columns.Count - 1; i++)
                {
                    usedWidth += lstLogMessages.Columns[i].Width;
                }
                lstLogMessages.Columns[lstLogMessages.Columns.Count - 1].Width = Math.Max(lstLogMessages.Width - usedWidth - 5, 1200);
            }
        }


        /// <summary>
        /// Creates a (virtual) listview row for a log message.
        /// </summary>
        private void LstLogMessages_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            var currentView = GetCurrentLogView();
            const int ICON_DEBUG = 0;
            const int ICON_INFO = 1;
            const int ICON_WARN = 2;
            const int ICON_ERROR = 3;
            const int ICON_COMMENT = 4;

            LogMessage message = currentView.GetMessage(e.ItemIndex);


            // Handle invalid messages
            if (message == null)  // This may occur when the LogView is cleared and a redraw happens before the timer sets the new VirtualListSize. After some msec the correct size will be set
            {
                e.Item = new ListViewItem(String.Empty);

                for (int i = 0; i < lstLogMessages.Columns.Count; i++)    // Create a dummy row since we cannot simply return no row
                {
                    e.Item.SubItems.Add(string.Empty);
                }
                return;
            }

            // Get color for the log level
            Color foreColor = Color.Black;
            Color backColor = Color.White;
            int imageIndex = ICON_DEBUG;
            switch (message.Level)
            {
                case LogLevel.Debug:
                    imageIndex = ICON_DEBUG;
                    foreColor = Color.Gray;
                    break;
                case LogLevel.Info:
                    imageIndex = ICON_INFO;
                    break;
                case LogLevel.Warning:
                    imageIndex = ICON_WARN;
                    backColor = Color.LightYellow;
                    break;
                case LogLevel.Error:
                    imageIndex = ICON_ERROR;
                    backColor = Color.Firebrick;
                    foreColor = Color.White;
                    break;
                case LogLevel.UserComment:
                    imageIndex = ICON_COMMENT;
                    backColor = Color.LightGreen;
                    foreColor = Color.DarkGreen;
                    break;
            }

            e.Item = new ListViewItem(String.Empty, imageIndex) { UseItemStyleForSubItems = false };

            // Get the message property for this column and handle the different types
            for (int i = 1; i < lstLogMessages.Columns.Count; i++)
            {
                int propertyIdentifier = (int)lstLogMessages.Columns[i].Tag;
                object value = message.GetProperty(propertyIdentifier);
                var currentCell = e.Item.SubItems.Add(string.Empty);
                currentCell.BackColor = backColor;
                currentCell.ForeColor = foreColor;

                if (value == null)
                {
                    currentCell.Text = String.Empty;
                }
                else if (propertyIdentifier == PropertyIdentifiers.PROPERTY_LINE_NR && (int)value < 0) 
                {
                    currentCell.Text = String.Empty;   // when no line nr is available (i.e. the log message is not from a file)
                }
                else if (value is string)
                {
                    currentCell.Text = value.ToString();

                    // Optional: remove indentation of the messages
                    if (propertyIdentifier == PropertyIdentifiers.PROPERTY_MESSAGE && _userPreferences.IgnoreLogIndentation)
                    {
                        currentCell.Text = currentCell.Text.TrimStart(' ', '\t');
                    }
                }
                else if (value is int)
                {
                    currentCell.Text = value.ToString();
                }
                else if (value is DateTime)
                {
                    DateTime currentTimestamp = (DateTime)value;

                    if (_userPreferences.TimeFormatMode == TimeFormatMode.RelativeTime && e.ItemIndex > 0)   // relative time
                    {
                        DateTime baseValue = _baseTimestamp.HasValue ? _baseTimestamp.Value : currentView.GetMessage(e.ItemIndex - 1).Timestamp;  // compare with previous message or user-defined base timestamp?
                        int relativeTimeMsec = (int)(currentTimestamp - baseValue).TotalMilliseconds;

                        if (relativeTimeMsec == 0)
                        {
                            currentCell.Text = "0";
                        }
                        else if (relativeTimeMsec < 60 * 1000 || _baseTimestamp.HasValue)
                        {
                            currentCell.Text = "+ " + relativeTimeMsec.ToString().PadLeft(3) + " ms";
                            if (relativeTimeMsec > 50)
                            {
                                currentCell.ForeColor = Color.IndianRed;
                            }
                        }
                        else
                        {
                            currentCell.Text = FormatDatetimeColumn(currentTimestamp);  // for longer jumps, repeat the base value
                        }
                    }
                    else  // absolute time
                    {
                        currentCell.Text = FormatDatetimeColumn(currentTimestamp);
                    }
                }
                else
                {
                    currentCell.Text = "<unknown type>";
                }

                // Validation and consolidation
                if (currentCell.Text.Length > 259) // #40285
                {
                    currentCell.Text = currentCell.Text.Substring(0, 258) + '\u2026' /* "..." */;
                }

                if (_userPreferences.ShowTabChars)
                {
                    currentCell.Text = currentCell.Text.Replace("\t", "\\t");
                }
            }


        }

        private string FormatDatetimeColumn(DateTime value)
        {
            if (value.Year == 1 || _userPreferences.TimeFormatMode == TimeFormatMode.TimeOnly)
            {
                return value.ToString(_userPreferences.TimeFormat);
            }
            else
            {
                return value.ToString(_userPreferences.DateTimeFormat);
            }
        }

        private void btnToggleAutoScroll_Click(object sender, EventArgs e)
        {
            if (!_autoScrollActive)
            {
                EnableAutoScroll();
            }
            else
            {
                DisableAutoScroll();
            }
        }

        public void ClearLogViews()
        {
            foreach (ILogView logView in _logController.GetLogViews())
            {
                // Are there invisible file-based log views attached?
                if (logView is FileBasedLogView fileLog)
                {
                    DialogResult dialogResult = DialogResult.No;
                    if(fileLog.IsOverflowFile &&_userPreferences.EnableLongTermMonitoring == false)
                    {
                        dialogResult = MessageBox.Show("Debwin4 has written a backup log file because the in-memory buffer reached the maximum size. Would you like to remove the log file backup at '" + fileLog.FilePath + "'?", "Debwin4", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    }

                    // Deleting auto-attached files (happens when the in-memory buffer reaches the max. size) might be unwanted by the user
                    if (!fileLog.IsOverflowFile || dialogResult == DialogResult.Yes)
                    {
                        if (fileLog.IsOverflowFile)
                        {
                            _logController.RemoveLogView(fileLog);
                            fileLog.Dispose();
                            try { File.Delete(fileLog.FilePath); } catch (Exception) { }
                        }
                        else
                        {
                            fileLog.ClearMessages();
                        }
                        continue;
                    }
                }

                logView.ClearMessages();
            }

            lstLogMessages.VirtualListSize = 0;
            RedrawView(GetCurrentLogView());
            SelectedLogMessage?.Invoke(this, new LogMessageSelectedEventArgs() { Message = null });  // clear log message in details panel

            if (_userPreferences.EnableAutoScrollOnClearLog)
            {
                EnableAutoScroll();
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            ClearLogViews();
        }

        public void DisableAutoScroll()
        {
            _autoScrollActive = false;
            UpdateToolbarButtonStates();
        }

        public void EnableAutoScroll()
        {
            lstLogMessages.SelectedIndices.Clear();
            _autoScrollActive = true;
            UpdateToolbarButtonStates();
        }

        public enum ChangeFontSizeAction
        {
            Increase,
            Decrease,
            Reset
        }
        public void ExecuteChangeFontSizeAction(ChangeFontSizeAction changeFontSizeAction)
        {
            Font currentFont = this.lstLogMessages.Font;
            float zoomFactor = 1.1F;
            float newFontSize = 0;
            switch (changeFontSizeAction)
            {
                case ChangeFontSizeAction.Decrease:
                    {
                        newFontSize = ((currentFont.Size / zoomFactor) <= 1) ? 1 : (currentFont.Size / zoomFactor);
                    }
                    break;

                case ChangeFontSizeAction.Increase:
                    {
                        newFontSize = currentFont.Size * zoomFactor;
                    }
                    break;

                case ChangeFontSizeAction.Reset:
                    {
                        newFontSize = _defaultListLogMessageFontSize;
                    }
                    break;
            }

            _userPreferences.ListLogMessageFontSize = newFontSize;
            this.lstLogMessages.Font = new System.Drawing.Font(
                                                                currentFont.Name,
                                                                newFontSize,
                                                                currentFont.Style,
                                                                currentFont.Unit,
                                                                currentFont.GdiCharSet
                                                                );
            lstLogMessages.Invalidate();
        }

        private bool _hasShownNoSelectInRingBufferMessage;

        private void lstLogMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_autoScrollForced)
            {
                // prevent a selection when the ring-buffering is active, as the messages move to the top even without scrolling
                // when we're deleting the old messages (even the selected message would be deleted eventually!):
                lstLogMessages.SelectedIndices.Clear();
                if (!_hasShownNoSelectInRingBufferMessage)
                {
                    _hasShownNoSelectInRingBufferMessage = true;
                    MessageBox.Show("Selecting a message is not possible in ring-buffer mode when the capturing is still active. Please stop capturing first.", "Debwin4", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            if (_userPreferences.DisableAutoScrollOnSelection)
            {
                DisableAutoScroll();
            }

            HandleItemSelectionChanged();

            var currentView = GetCurrentLogView();
            if (currentView != null)
            {
                SelectedLogMessage?.Invoke(this, new LogMessageSelectedEventArgs() { Message = GetSelectedLogMessage() });
            }
        }

        private void lstLogMessages_VirtualItemsSelectionRangeChanged(object sender, ListViewVirtualItemsSelectionRangeChangedEventArgs e)
        {
            HandleItemSelectionChanged();
        }

        private void HandleItemSelectionChanged()
        {
            if (lstLogMessages.SelectedIndices.Count > 1)
            {
                btnSaveLog.Text = "Save Selection";
                btnCopyLog.Text = "Copy Selection";
                copySelectedMessagesMenuItem.Text = "Copy Selected Messages";
            }
            else
            {
                btnSaveLog.Text = "Save Log";
                btnCopyLog.Text = "Copy Log";
                copySelectedMessagesMenuItem.Text = "Copy Selected Message";
            }
        }

        private LogMessage GetSelectedLogMessage()
        {
            var currentView = GetCurrentLogView();
            return (lstLogMessages.SelectedIndices.Count != 0 ? currentView.GetMessage(lstLogMessages.SelectedIndices[0]) : null);
        }

        private void btnPopLogView_ButtonClick(object sender, EventArgs e)
        {
            var selectedMessage = GetSelectedLogMessage();

            PopLogView();

            if (selectedMessage != null)   // restore previously selected message
            {
                FindAndSelectMessage(new Predicate<LogMessage>(msg => Object.ReferenceEquals(msg, selectedMessage)), false);
                lstLogMessages.Focus();
            }
        }

        private void lstLogMessages_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var selectedMessage = GetSelectedLogMessage();
                if (selectedMessage == null)
                    return;


                mnuLogMessageContext.Show(lstLogMessages, e.Location);
            }
        }

        private void SetDateFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedMsg = GetSelectedLogMessage();
            _mainWindow.GetFilterPanel().SetDateFromTo(sender == dateFromToolStripMenuItem, selectedMsg.Timestamp);
        }

        private void lstLogMessages_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (e.Delta < 0)
                {
                    ExecuteChangeFontSizeAction(ChangeFontSizeAction.Decrease);
                }
                else if (e.Delta > 0)
                {
                    ExecuteChangeFontSizeAction(ChangeFontSizeAction.Increase);
                }
            }
        }

        private void lstLogMessages_Scroll(object sender, ScrollEventArgs e)
        {
            DisableAutoScroll();

            // Workaround for a side effect of the anti-flickering hack (see RedrawView): When the last message was selected and the 
            // view is scrolled up, multiple (random) rows have the focus rect (while scrolling up, the focus is set to the first visible
            // row with a high frequency, and the ListView seems to still have the focus on the old row when it is moving down
            // A complete list redraw after each scrolling step seems to improve the problem:
            if (e.Type == ScrollEventType.ThumbTrack)
            {
                lstLogMessages.Invalidate();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void byThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainWindow.GetFilterPanel().SetThreadFilter(GetSelectedLogMessage().Thread);
        }


        private void includeLoggerInFilterMenuItem_Click(object sender, EventArgs e)
        {
            _mainWindow.GetFilterPanel().SetLoggerFilter(GetSelectedLogMessage().LoggerName, false);
        }

        private void excludeLoggerInFilterMenuItem_Click(object sender, EventArgs e)
        {
            _mainWindow.GetFilterPanel().SetLoggerFilter(GetSelectedLogMessage().LoggerName, true);
        }

        private void includeModuleInFilterMenuItem_Click(object sender, EventArgs e)
        {
            _mainWindow.GetFilterPanel().AddModuleFilter(GetSelectedLogMessage().GetProperty(PropertyIdentifiers.PROPERTY_MODULE_NAME) as string, false);
        }

        private void excludeModuleInFilterMenuItem_Click(object sender, EventArgs e)
        {
            _mainWindow.GetFilterPanel().AddModuleFilter(GetSelectedLogMessage().GetProperty(PropertyIdentifiers.PROPERTY_MODULE_NAME) as string, true);
        }


        /// <summary>Gets the current message and selects it in the unfiltered view to see the context of the message.</summary>
        private void miSelectInUnfilteredView_Click(object sender, EventArgs e)
        {
            var selectedMessage = GetSelectedLogMessage();
            if (selectedMessage == null)
                return;

            GoToRootView();
            FindAndSelectMessage(new Predicate<LogMessage>(msg => Object.ReferenceEquals(msg, selectedMessage)), false);
            lstLogMessages.Focus();
        }

        private void setBaseForRelativeTimeMenuItem_Click(object sender, EventArgs e)
        {
            var selectedMessage = GetSelectedLogMessage();
            if (selectedMessage == null)
                return;

            _baseTimestamp = selectedMessage.Timestamp;
            lstLogMessages.Invalidate();
        }




        /// <summary>Searches for warnings and errors and selects the next match (that is not in the same group of warning/error messages).</summary>
        private void btnFindIssue_Click(object sender, EventArgs e)
        {
            bool findLast = (sender == btnFindLastIssue);
            LogMessage currentSelectedMessage;
            int currentSelectedIndex;
            LogMessage previousSelectedMessage;
            int previousSelectedIndex;
            bool hasDoneWrapAround = false;
            SetCurrentSearchText(findLast ? "<findlast>" : "<findnext>");

            while (true)
            {
                // First save position and level of the currently selected message
                previousSelectedMessage = GetSelectedLogMessage();
                previousSelectedIndex = (lstLogMessages.SelectedIndices.Count > 0 ? lstLogMessages.SelectedIndices[0] : -1);

                // Jump to the next (previous) issue
                bool hasMatch = FindAndSelectMessage((message) => message.Level >= LogLevel.Warning, findLast);
                lstLogMessages.Focus();

                // Get position and level again
                currentSelectedMessage = GetSelectedLogMessage();
                currentSelectedIndex = (lstLogMessages.SelectedIndices.Count > 0 ? lstLogMessages.SelectedIndices[0] : -1);

                // If the selection was already on an issue, and the next match is its neighbour message, we don't want to move the selection just to the next (previous) row
                // because often several lines of the same error text are sent as separate messages (e.g. LL/cRM)
                // -> Start the search again so we jump to the next (group of) issue(s)
                if (hasMatch && previousSelectedMessage != null && currentSelectedMessage != null
                    && previousSelectedMessage.Level >= LogLevel.Warning && Math.Abs(previousSelectedIndex - currentSelectedIndex) == 1)
                {
                    continue;
                }
                // if we reach end (start) of list, continue search at the top (bottom) - but only once to prevent infinite loop!
                else if (!hasMatch && _lastSearchReachedEndOfList && !hasDoneWrapAround)
                {
                    hasDoneWrapAround = true;
                    SystemSounds.Beep.Play();
                    continue;
                }
                else
                {
                    // make sure to set the cursor to the new position
                    lstLogMessages.FocusedItem = lstLogMessages.Items[currentSelectedIndex];
                    break;
                }
            }
        }

        /// <summary>Lets the user enter a custom message and adds the comment to the log.</summary>
        private void btnAppendMessage_Click(object sender, EventArgs e)
        {
            var frmCreateMessage = new CreateMessageDialog();
            if (_mainWindow.ShowDialogEx(frmCreateMessage) == DialogResult.OK)
            {
                DateTime now = DateTime.Now;
                foreach (ILogView logView in _logController.GetLogViews())
                {
                    logView.AppendMessage(new LogMessage(frmCreateMessage.Message)
                    {
                        Level = LogLevel.UserComment,
                        // If we use DateTime.Now directly, it will contain a 'ticks' part, so a comparison with a filter 
                        // that is created from a string with millisecond-precision can never be true (#20829)
                        Timestamp = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second)
                    });
                }
                RedrawView(GetCurrentLogView());
            }
        }



        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            bool saveToFile = (sender != btnCopyLog);
            bool openEditor = (sender == btnSaveAndEditLog);
            bool isFilterActive = (_logViewStack.Count > 1);
            bool saveOnlySelectedMessages = (lstLogMessages.SelectedIndices.Count >= (saveToFile ? 2 : 1) && lstLogMessages.SelectedIndices.Count != lstLogMessages.VirtualListSize);
            bool useFilteredLog = saveOnlySelectedMessages; // if there is an explicit selection, we always need to use the visible log as source
            string outputFilePath = null;

            // The user might not intentionally save a filtered log...
            if (isFilterActive && !saveOnlySelectedMessages && MessageBox.Show("A filter is active. Would you like to save only the filtered (currently displayed) messages?" + Environment.NewLine + Environment.NewLine + "If the log file is for the combit support, please save an unfiltered log (choose 'No').", "Debwin4", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                useFilteredLog = true;
            }

            // If a log file (containing all messages) is attached (reasons: buffer limit reached/control message from cRM), it might be better to copy
            // the already existing (full) log file instead of saving the current in-memory buffer as that buffer contains only the last X messages
            if (!saveOnlySelectedMessages && !useFilteredLog && _logController.GetLogViews().Any(log => log is FileBasedLogView))
            {
                FileBasedLogView fileLog = _logController.GetLogViews().OfType<FileBasedLogView>().First();
                if (openEditor)
                {
                    // user wants to show it only in the editor, no need to save it at an other place
                    outputFilePath = fileLog.FilePath;
                }
                else
                {
                    SaveFileDialog fileDialog = new SaveFileDialog() { Filter = "Debwin4 Log File (.log4)|*.log4" };
                    if (fileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        outputFilePath = fileDialog.FileName;
                        File.Copy(fileLog.FilePath, outputFilePath, true);
                    }
                }
            }
            else
            {
                // Give the Log Writer the information of the displayed columns - might be needed to write a formatted text file
                List<int> columnsToSave = new List<int>();
                foreach (ColumnHeader columnInView in lstLogMessages.Columns)
                {
                    if (columnInView.Width <= 10)  // skip columns that are hidden in the UI when writing a formatted file
                        continue;

                    int propertyIdOfColumn = (int)columnInView.Tag;
                    columnsToSave.Add(propertyIdOfColumn);
                }

                IQueryableLogView logToSave = useFilteredLog ? GetCurrentLogView() : GetRootLogView();

                var saveLogDialog = new SaveLogDialog(logToSave, saveToFile, saveOnlySelectedMessages ? Enumerable.OfType<int>(lstLogMessages.SelectedIndices) : null, columnsToSave);
                if (openEditor)
                {
                    saveLogDialog.LogFilePath = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), "log"));
                }

                if (saveLogDialog.ShowDialog() == DialogResult.OK)
                    outputFilePath = saveLogDialog.LogFilePath;
            }
            if (outputFilePath != null)
            {
                ShowInExplorerOrEditor(outputFilePath, openEditor);
            }
        }

        private void ShowInExplorerOrEditor(string file, bool openEditor)
        {
            if (openEditor)
            {
                Process.Start(_userPreferences.EditorPath, file);
            }
            else if (MessageBox.Show(this, "Log was saved. Show log file in Windows Explorer?", "Debwin", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Process.Start("explorer.exe", "/select, \"" + file + "\"");
            }
        }

        private void btnSaveWithSpecificColumns_Click(object sender, EventArgs e)
        {
            LogViewPanelColumnsDialog selectedColumnsDialog;

            if (_userPreferences.SelectedPropertyIDs.Count > 0)
            {
                // Let the user choose which properties to show
                selectedColumnsDialog = new LogViewPanelColumnsDialog(GetLogMessagePropertyInfo(), _userPreferences.SelectedPropertyIDs);
            }
            else
            {
                // Prepare a list of the currently displayed properties from the visible list columns
                var currentlySelectedPropertyIDs = lstLogMessages.Columns.Cast<ColumnHeader>().Select(column => (int)column.Tag);
                // Let the user choose which properties to show
                selectedColumnsDialog = new LogViewPanelColumnsDialog(GetLogMessagePropertyInfo(), currentlySelectedPropertyIDs.ToList());
            }
            selectedColumnsDialog.label1.Text = "Selected columns:";

            if (_mainWindow.ShowDialogEx(selectedColumnsDialog) == DialogResult.OK)
            {
                _userPreferences.SelectedPropertyIDs = selectedColumnsDialog.SelectedPropertyIDs.ToList();
                SaveLogDialog saveLogDialog = new SaveLogDialog(GetCurrentLogView(), true, null, selectedColumnsDialog.SelectedPropertyIDs.ToList());
                saveLogDialog.SetFilter("Formatted log file|*.txt");
                saveLogDialog.ShowDialog();
                string outputFilePath = saveLogDialog.LogFilePath;
                if(outputFilePath != null)
                {
                    ShowInExplorerOrEditor(outputFilePath, false);
                }
            }
        }

            private void copySelectedMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopySelectedMessages();
        }


        #region "Text Search"

        private bool _lastSearchReachedEndOfList;   // True when the text search ends without a match. Reset if a different search text is used.
        private string _lastSearchText;             // Keeps the last text that was searched, so _lastSearchReachedEndOfList can be reset if an other text is searched

        /// <summary>
        /// Keeps book of which text is currently searched, so the _lastSearchReachedEndOfList flag can be reset if the user starts searching for a different thing
        /// </summary>
        private void SetCurrentSearchText(string text)
        {
            if (_lastSearchText != text)
            {
                _lastSearchReachedEndOfList = false;
            }
            _lastSearchText = text;
            if (!txtSearchBox.Items.Contains(text))
            {
                txtSearchBox.Items.Insert(0, text);

                while (txtSearchBox.Items.Count > 10)
                {
                    txtSearchBox.Items.RemoveAt(10);
                }
            }
            _userPreferences.SearchHistory = new System.Collections.ArrayList(txtSearchBox.Items);
        }

        private void findTextMenuItem_ButtonClick(object sender, EventArgs e)
        {
            bool ignoreCase = !caseSensitiveMenuItem.Checked;
            bool findLast = (sender == findLastMenuItem);
            Predicate<LogMessage> predicate;

            // Entered something like ":123"? --> Goto line 123
            int lineNr;
            if (txtSearchBox.Text.Length > 1 && txtSearchBox.Text.StartsWith(":") && int.TryParse(txtSearchBox.Text.Substring(1), out lineNr))
            {
                JumpToLogLine(lineNr);
                return;
            }

            // regular text search

            SetCurrentSearchText(txtSearchBox.Text);
            if (txtSearchBox.Text.StartsWith("/") && txtSearchBox.Text.EndsWith("/") && txtSearchBox.Text.Length > 2)  // Regex search when in slashes:   /regex/
            {
                Regex regex = new Regex(txtSearchBox.Text.Substring(1, txtSearchBox.Text.Length - 2), RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                predicate = (msg) => regex.Match(msg.Message).Success;
            }
            else
            {
                predicate = (msg) => msg.Message.IndexOf(txtSearchBox.Text, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture) != -1;
            }

            txtSearchBox.Enabled = false;
            txtSearchBox.BackColor = SystemColors.Window;
            this.Cursor = Cursors.WaitCursor;
            if (!FindAndSelectMessage(predicate, findLast))
            {
                txtSearchBox.BackColor = Color.Tomato;
            }

            this.Cursor = Cursors.Default;
            txtSearchBox.Enabled = true;

            lstLogMessages.Focus();
        }


        public void JumpToLogLine(int lineNr)
        {
            DisableAutoScroll();
            if (lineNr > 0 && lineNr <= lstLogMessages.VirtualListSize)  // lineNr is not zero-based!
            {
                SetSelectedMessageByIndex(lineNr - 1, true);
                lstLogMessages.Focus();
            }
            else
            {
                txtSearchBox.BackColor = Color.Tomato;
            }
        }

        /// <summary>
        /// Searches the next or last log message which matches the specified predicate, starting from the currently selected list entry if available.
        /// </summary>
        /// <param name="searchBottomToTop">If true, the search is done from newer to older log messages.</param>
        private bool FindAndSelectMessage(Predicate<LogMessage> searchPredicate, bool searchBottomToTop)
        {
            var currentView = GetCurrentLogView();
            if (currentView == null)
                return false;

            int startIndex;

            // If a row is selected, use it as the start index for the search.
            // But when the last search run hit the end of the list before finding a match (_lastSearchReachedEndOfList = true),
            // wrap around and begin at the default search start index (else-case)
            if (!_lastSearchReachedEndOfList)
            {
                if (lstLogMessages.SelectedIndices.Count > 0)  // normal case: start searching after the selected row
                {
                    startIndex = lstLogMessages.SelectedIndices[0] + (searchBottomToTop ? -1 : 1);
                }
                else
                {
                    startIndex = 0;   // initial case: no row is selected, view is scrolled to the bottom
                    _lastSearchReachedEndOfList = true;     // simulate jump to the top of the list from the bottom, so the calculation of the scroll direction (below) does not get confused
                }
            }
            else   // otherwise begin at end or start of the list
            {
                startIndex = searchBottomToTop ? lstLogMessages.VirtualListSize - 1 : 0;
            }

            if (startIndex == -1)
            {
                _lastSearchReachedEndOfList = true;
                SystemSounds.Beep.Play();
                return false;
            }

            DisableAutoScroll();
            var indexOfMatch = currentView.FindIndexOfMessage(searchBottomToTop, startIndex, searchPredicate);
            if (indexOfMatch == -1)
            {
                _lastSearchReachedEndOfList = true;
                return false;
            }
            else
            {
                SetSelectedMessageByIndex(indexOfMatch, true);
                _lastSearchReachedEndOfList = false;
                return true;
            }
        }

        private void SetSelectedMessageByIndex(int rowIndex, bool setAsFocusedItem)
        {
            lstLogMessages.SelectedIndices.Clear();
            lstLogMessages.SelectedIndices.Add(rowIndex);
            lstLogMessages.Items[rowIndex].Selected = true;

            int rowToScrollTo = rowIndex;

            // If the listview needs to scroll to an area that is not in view so far, scroll the listview so far that the matched message is vertically centered:
            var itemRect = lstLogMessages.GetItemRect(rowIndex, ItemBoundsPortion.Entire);
            var topItemRect = lstLogMessages.GetItemRect(lstLogMessages.TopItem.Index, ItemBoundsPortion.Entire);



            if (!itemRect.IntersectsWith(new Rectangle(0, topItemRect.Y, topItemRect.Width, lstLogMessages.ClientSize.Height - topItemRect.Y)))
            {
                int visibleRowsCount = lstLogMessages.Height / itemRect.Height;    // number of rows in the visible area
                int currentlyCenteredIndex = lstLogMessages.TopItem.Index + visibleRowsCount / 2;

                // scrolling up or down? handle the case that a wrap-around happens and the search is continued at the other end of the list
                bool willScrollUp = rowIndex < currentlyCenteredIndex;

                rowToScrollTo = rowIndex + (willScrollUp ? -1 : 1) * (visibleRowsCount / 2);            // depending on the scrolling direction, add or subtract half of the list height 
                rowToScrollTo = Math.Min(Math.Max(0, rowToScrollTo), lstLogMessages.VirtualListSize - 1);   // clip to indexes that exist
            }

            lstLogMessages.EnsureVisible(rowToScrollTo);

            if (setAsFocusedItem)
            {
                lstLogMessages.FocusedItem = lstLogMessages.Items[rowIndex];
            }

        }

        private void errorCodeNegNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtSearchBox.Text = "/-[0-9]+/";
        }

        private void txtSearchBox_TextChanged(object sender, EventArgs e)
        {
            // reset flags from last (unsuccessful) search
            txtSearchBox.BackColor = SystemColors.Window;
            _lastSearchReachedEndOfList = false;
        }

        private void txtSearchBox_Click(object sender, System.EventArgs e)
        {
            txtSearchBox.BackColor = System.Drawing.SystemColors.Window;
        }


        private void txtSearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                findTextMenuItem_ButtonClick(txtSearchBox, null);
                e.Handled = true;
            }
        }

        private void txtSearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)  // supress the beep sound when pressing enter (#25400) - only works in KeyPress event!
                e.Handled = true;
        }

        #endregion

        private void lstLogMessages_ScrolledToEnd(object sender, EventArgs e)
        {
            if (_userPreferences.EnableAutoScrollOnScrollToEnd && lstLogMessages.SelectedIndices.Count < 2)
            {
                EnableAutoScroll();
            }
        }

        private void SetupLongTermMonitoring()
        {
            string hour = DateTime.Now.ToString("HH");
            Util.TaskScheduler.Instance.ScheduleTask(Convert.ToInt32(hour) + 1, 0, 1,  // Create a logfile every hour
            () =>
            {
                if (_userPreferences.EnableLongTermMonitoring)
                {
                    string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
                    string path = _userPreferences.LogFilePath + string.Format("{0}-{1}.log4", LogController.Name, currentDateTime);
                    var attachedFileWriter = _logController.GetLogViews().OfType<FileBasedLogView>().FirstOrDefault();
                    if (attachedFileWriter != null)
                    {
                        attachedFileWriter.WriteLongTermMonitoringFile(path);
                        this.Invoke(new Action(() => this.ClearLogViews()));
                    }
                    else
                    {
                        IQueryableLogView logView = GetCurrentLogView();
                        if (logView != null)
                        {
                            SaveLogDialog saveLogDialog = new SaveLogDialog(logView, true, null, null);
                            saveLogDialog.LogFilePath = _userPreferences.LogFilePath + string.Format("{0}-{1}.log4", LogController.Name, currentDateTime);
                            saveLogDialog.ShowDialog();
                            this.Invoke(new Action(() => this.ClearLogViews()));
                        }
                    }
                }
            });
        }

        private void CopySelectedMessages()
        {
            if (this.lstLogMessages.SelectedIndices.Count == 0)
                return;

            int[] selectedIndices = null;
            if (lstLogMessages.SelectedIndices.Count != lstLogMessages.VirtualListSize)   // skip collecting selected indices when all items are selected
            {
                selectedIndices = new int[this.lstLogMessages.SelectedIndices.Count];
                this.lstLogMessages.SelectedIndices.CopyTo(selectedIndices, 0);
            }

            var saveLogDialog = new SaveLogDialog(GetCurrentLogView(), false, selectedIndices, null);
            saveLogDialog.ShowDialog();
        }


        private void GoToMessage(int index)
        {
            if (index < 0 || index >= lstLogMessages.VirtualListSize)
                return;

            lstLogMessages.SelectedIndices.Clear();
            lstLogMessages.SelectedIndices.Add(index);
            lstLogMessages.FocusedItem = lstLogMessages.Items[index];
            lstLogMessages.EnsureVisible(index);
        }

        private void lstLogMessages_KeyDown(object sender, KeyEventArgs e)
        {
            // Copy to clipboard
            if (e.Control && e.KeyCode == Keys.C
                || e.Control && e.KeyCode == Keys.Insert)
            {
                btnCopyLog.PerformClick();
                e.Handled = true;
            }
            // Toggle Auto-Scroll
            else if (e.KeyCode == Keys.F5)
            {
                btnToggleAutoScroll.PerformClick();
                e.Handled = true;
            }
            // Paste from clipboard
            else if (e.Control && e.KeyCode == Keys.V
                || e.Shift && e.KeyCode == Keys.Insert)
            {
                StartPagePanel.OpenLogFromClipboard(_mainWindow);
            }
            // Select all
            else if (e.Control && e.KeyCode == Keys.A)
            {
                lstLogMessages.FastSelectAllItems();
            }
            // Undo Filter
            else if (e.Control && e.KeyCode == Keys.Z)
            {
                btnPopLogView.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                btnSaveLog.PerformButtonClick();
            }
            // Start Capturing
            else if (e.KeyCode == Keys.F9)
            {
                StartOrStopMessagesSources(true);
            }
            // Stop Capturing
            else if (e.KeyCode == Keys.Pause)
            {
                StartOrStopMessagesSources(false);
            }
            // Previous Issue
            else if (e.Control && e.KeyCode == Keys.Up)
            {
                btnFindLastIssue.PerformClick();
                e.Handled = true;
            }
            // Next Issue
            else if (e.Control && e.KeyCode == Keys.Down)
            {
                btnFindNextIssue.PerformClick();
                e.Handled = true;
            }
            // Search
            else if (e.Control && e.KeyCode == Keys.F)
            {
                InvokeTextSearch();
            }
            // Clear Messages
            else if (e.Control && e.KeyCode == Keys.Delete)
            {
                btnClearLog.PerformClick();
            }
            // Save to file
            else if (e.Control && e.KeyCode == Keys.S)
            {
                btnSaveLog.PerformClick();
            }
            // Select all items until the last (Shift+End)
            else if (e.Control && e.Shift && e.KeyCode == Keys.End && lstLogMessages.SelectedIndices.Count == 1)
            {
                lstLogMessages.BeginUpdate();
                for (int i = lstLogMessages.SelectedIndices[0] + 1; i < lstLogMessages.VirtualListSize; i++)
                {
                    lstLogMessages.SelectedIndices.Add(i);
                }
                lstLogMessages.EndUpdate();
            }
            // Override Ctrl+Home/Ctrl+End behaviour (move selection to top/end instead of moving the focus only)
            else if (e.Control && e.KeyCode == Keys.Home)
            {
                GoToMessage(0);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.End)
            {
                GoToMessage(lstLogMessages.VirtualListSize - 1);
                e.Handled = true;
            }
            // Ctrl+PageUp/PageDown by default moves the focus (not selection) one page up/down  --> ignore completely for Debwin
            else if (e.Control && (e.KeyCode == Keys.PageUp || e.KeyCode == Keys.PageDown))
            {
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.Subtract)
            {
                ExecuteChangeFontSizeAction(ChangeFontSizeAction.Decrease);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.Add)
            {
                ExecuteChangeFontSizeAction(ChangeFontSizeAction.Increase);
                e.Handled = true;
            }
            else if (e.Control && (e.KeyCode == Keys.NumPad0 || e.KeyCode == Keys.D0))
            {
                ExecuteChangeFontSizeAction(ChangeFontSizeAction.Reset);
                e.Handled = true;
            }
        }

        public void InvokeTextSearch()
        {
            txtSearchBox.Focus();
            txtSearchBox.SelectAll();
        }

        private void StartOrStopMessagesSources(bool shouldBeStarted)
        {
            foreach (var messageCollector in _logController.GetMessageCollectors())
            {
                if (messageCollector.IsStopped && shouldBeStarted)
                {
                    messageCollector.Start();
                }
                else if (!messageCollector.IsStopped && !shouldBeStarted)
                {
                    messageCollector.Stop();
                }
            }
            btnCaptureActive.Checked = shouldBeStarted;
        }

        private void chooseColumnsMenuItem_Click(object sender, EventArgs e)
        {
            ShowChooseColumnsDialog();
        }

        private void lstLogMessages_ColumnReordered(object sender, ColumnReorderedEventArgs e)
        {
            if (e.OldDisplayIndex == 0 || e.NewDisplayIndex == 0)   // prevent reordering the first column (Level Icon)
                e.Cancel = true;
        }

        private void mnuLogMessageContext_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool moduleColumnVisible = lstLogMessages.Columns.OfType<ColumnHeader>().Any(c => (int)c.Tag == PropertyIdentifiers.PROPERTY_MODULE_NAME);
            bool loggerColumnVisible = lstLogMessages.Columns.OfType<ColumnHeader>().Any(c => (int)c.Tag == PropertyIdentifiers.PROPERTY_LOGGER_NAME);
            includeModuleInFilterMenuItem.Visible = excludeModuleInFilterMenuItem.Visible = moduleColumnVisible;
            includeLoggerInFilterMenuItem.Visible = excludeLoggerInFilterMenuItem.Visible = loggerColumnVisible;
        }

        private void lblAttachedLogFileName_Click(object sender, EventArgs e)
        {
            // User intends to use the output file now => flush all internal write buffers
            foreach (var fileLog in _logController.GetLogViews().OfType<FileBasedLogView>())
            {
                fileLog.TriggerFlush();
            }

            // Open Windows Explorer and select the output file
            string argument = "/select, \"" + lblAttachedLogFileName.Tag.ToString() + "\"";
            Process.Start("explorer.exe", argument);
        }

        private void btnStartStopReceiver_Click(object sender, EventArgs e)
        {
            StartOrStopMessagesSources(!btnCaptureActive.Checked);
        }

        private void goToLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtSearchBox.Text = ":";
            txtSearchBox.Focus();
            txtSearchBox.SelectionStart = 1;
        }

        private void LogViewPanel_Load(object sender, EventArgs e)
        {
            // the width of the menu dropdown buttons isnt DPI-scaled by WinForms yet --> manual scaling
            btnSaveLog.DropDownButtonWidth = (int)(16 * this.AutoScaleFactor.Width);
            findTextMenuItem.DropDownButtonWidth = (int)(16 * this.AutoScaleFactor.Width);
        }
    }


    public class LogViewPanelOptions
    {
        /// <summary>Optionally specifies a set of suitable columns for the properties of the log type that is going to be displayed.</summary>
        public LogViewPanelColumnSet DefaultColumns { get; set; }

        /// <summary>Gets or sets if the log will be displaying live captured messages. For non-live-capture, buttons like Clear or Autoscroll may be hidden.</summary>
        public bool LiveCaptureMode { get; set; }

    }

    public class LogMessageSelectedEventArgs : EventArgs
    {
        public LogMessage Message { get; set; }
    }

}