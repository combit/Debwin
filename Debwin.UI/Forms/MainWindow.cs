using Debwin.Core;
using Debwin.Core.Controller;
using Debwin.Core.MessageSources;
using Debwin.Core.Views;
using Debwin.UI.Forms;
using Debwin.UI.Panels;
using Debwin.UI.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using WeifenLuo.WinFormsUI.Docking;
using Microsoft.Win32;
using System.Reflection;
using System.Diagnostics;
using System.Xml;

namespace Debwin.UI
{

    public interface IMainWindow
    {
        FilterPanel GetFilterPanel();

        IDebwinController GetDebwinController();

        LogViewPanel GetActiveLogView();

        void OpenNewLogView(ILogController logController, IQueryableLogView rootLogView, LogViewPanelOptions logViewPanelOptions);

        /// <summary>Like Form.ShowDialog(), but temporarily disables the stay-on-top flag of this window, so it does not cover the modal dialog (looks like freezed UI to the user).</summary>
        DialogResult ShowDialogEx(Form dialog);

        bool IsVisible();
    }

    public partial class MainWindow : Form, IMainWindow, ISingleInstanceApp
    {
        private readonly IDebwinController _debwinController;
        private readonly IUserPreferences _userPreferences;

        private MessageDetailsPanel _detailsPanel;
        private FilterPanel _filterPanel;
        private LogControllerPanel _logControllerPanel;
        private LogStructurePanel _logStructurePanel;
        private LogViewPanel _activeLogViewPanel;
        private LlJobAnalyzerPanel _jobAnalyzerPanel;

        private StartPagePanel _startPagePanel;
        private GlobalKeyboardHook _globalKeyboardHook;

        private bool _isDisposed;

        private int _ctrlLeft1, _ctrlLeft2, _ctrlTop1, _ctrlTop2;
        private Screen _currentScreenBeforeMinimize;

        public IDockContent GetContent(string persistString)
        {
            // by default, persist string is just the class name for the required IDockContent. Simply return the prepared classes.
            switch (persistString)
            {
                case "Debwin.UI.Panels.FilterPanel":
                    return _filterPanel;
                case "Debwin.UI.Panels.LogControllerPanel":
                    return _logControllerPanel;
                case "Debwin.UI.Panels.LogStructurePanel":
                    return _logStructurePanel;
                case "Debwin.UI.Panels.StartPagePanel":
                    return _startPagePanel;
                case "Debwin.UI.Panels.LlJobAnalyzerPanel":
                    return _jobAnalyzerPanel;
                case "Debwin.UI.Panels.LogViewPanel":
                    return _activeLogViewPanel;
                case "Debwin.UI.Panels.MessageDetailsPanel":
                    return _detailsPanel;
            }
            return null;
        }

        private static string GetDockPersistenceFile()
        {
            string combitDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "combit");
            if (!Directory.Exists(combitDirectory))
            {
                Directory.CreateDirectory(combitDirectory);
            }
            return Path.Combine(combitDirectory, "Debwin4WindowStates.xml");
        }


        public MainWindow(IDebwinController debwinController, IUserPreferences preferences)
        {
            _ctrlLeft1 = 0;
            _ctrlLeft2 = 0;
            _ctrlTop1 = 0;
            _ctrlTop2 = 0;
            _userPreferences = preferences;
            _debwinController = debwinController;
            
            InitializeComponent();

            this.dockPanel1.ActiveContentChanged += this.dockPanel1_ActiveDocumentChanged;  // needs to be detached manually!

            this.WindowState = FormWindowState.Minimized;
            this.StartPosition = FormStartPosition.WindowsDefaultBounds;

            // restore last main window position
            if (!_userPreferences.MainWindowBounds.IsEmpty && IsVisibleOnAnyScreen(_userPreferences.MainWindowBounds))
            {
                this.StartPosition = FormStartPosition.Manual;
                this.DesktopBounds = _userPreferences.MainWindowBounds;
            }

            var theme = new VS2015LightTheme();
            theme.Measures.DockPadding = 0;

            dockPanel1.Theme = theme;
            _filterPanel = new FilterPanel(_userPreferences);
            _filterPanel.RequestedFilter += filterPanel_RequestedFilter;
            _logControllerPanel = new LogControllerPanel(_debwinController);
            _logStructurePanel = new LogStructurePanel();
            _startPagePanel = new StartPagePanel(this, debwinController, preferences);
            _jobAnalyzerPanel = new LlJobAnalyzerPanel(this);
            _detailsPanel = new MessageDetailsPanel(_userPreferences, this);
            _detailsPanel.CloseButtonVisible = false;

            if (File.Exists(GetDockPersistenceFile()))
            {
                try
                {
                    // load dock states from XML
                    dockPanel1.LoadFromXml(GetDockPersistenceFile(), GetContent);

                    // if Debwin4 was closed without an active log view, the details panel might not have been initialized at the time - in that case, use default
                    if (_detailsPanel.DockState == DockState.Unknown || _detailsPanel.DockState == DockState.Hidden)
                    {
                        _detailsPanelDockState = DockState.DockBottom;
                    }
                    else
                    {
                        _detailsPanelDockState = _detailsPanel.DockState;
                    }
                    _detailsPanel.Hide();
                }
                catch (Exception) // as this would otherwise crash the app on startup...
                {
                    InitUIToDefaults();
                }
            }
            else
            {
                InitUIToDefaults();
            }

            showTabsMenuItem.Checked = _userPreferences.ShowTabChars;
            this.TopMost = stayOnTopToolStripMenuItem.Checked = _userPreferences.MainWindowTopMost;
            notifyIcon.Visible = hideOnMinimizeMenuItem.Checked = _userPreferences.MainWindowHideOnMinimize;
            miDateFormatDateTime.Checked = (_userPreferences.TimeFormatMode == TimeFormatMode.DateTime);
            miDateFormatTimeOnly.Checked = (_userPreferences.TimeFormatMode == TimeFormatMode.TimeOnly);
            miDateFormatRelative.Checked = (_userPreferences.TimeFormatMode == TimeFormatMode.RelativeTime);

            if (_userPreferences.InstallKeyboardHook)
                SetupKeyboardHooks();

#if DEBUG
            logStructureToolStripMenuItem.Visible = true;
            logsToolStripMenuItem.Visible = true;
#endif
        }

        private void InitUIToDefaults()
        {
            // use defaults
            _filterPanel.Show(dockPanel1, DockState.DockRightAutoHide);
            _logControllerPanel.Show(dockPanel1, DockState.DockLeftAutoHide);
            _logControllerPanel.Hide();  // although we directly hide it, the panel window should have been loaded so it can listen for the events of new log sources etc.          
            _logStructurePanel.Show(dockPanel1, DockState.DockLeftAutoHide);
            _logStructurePanel.Hide(); // see above           
            _startPagePanel.Show(dockPanel1, DockState.Document);
            _jobAnalyzerPanel.Show(dockPanel1, DockState.DockRightAutoHide);
            _detailsPanelDockState = DockState.DockBottom;
        }

        private void CheckBoxTimeFormatMode_CheckedChanged(Object sender, EventArgs e)
        {
            if(sender == miDateFormatDateTime)
            {
                _filterPanel.ChangeDate_TimeLabels(TimeFormatMode.DateTime);
            }
            else if (sender == miDateFormatRelative)
            {
                _filterPanel.ChangeDate_TimeLabels(TimeFormatMode.RelativeTime);
            }
            else
            {
                _filterPanel.ChangeDate_TimeLabels(TimeFormatMode.TimeOnly);
            }
        }

        static bool _sessionEnding = false;

        private static int WM_QUERYENDSESSION = 0x11;
        private DockState _detailsPanelDockState = DockState.Unknown;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_QUERYENDSESSION)
            {
                _sessionEnding = true;
            }

            // If this is WM_QUERYENDSESSION, the closing event should be
            // raised in the base WndProc.
            base.WndProc(ref m);

        } //WndProc 

        public bool IsVisible()
        {
            return this.WindowState != FormWindowState.Minimized;
        }

        // Handles the command line arguments of a second Debwin instance that the user tried to start (signal via .NET Remoting)
        public bool HandleSecondAppInstanceLaunch(IList<string> cmdArgs)
        {
            this.Invoke(new Action(() =>
            {
                ActivateWindow();

                // If second instance was started with a file path in the cmd args (e.g. after opening a .log4 file in the explorer), open it in this instance
                if (cmdArgs != null)
                {
                    for (int i = 1; i < cmdArgs.Count; i++)
                    {
                        if (File.Exists(cmdArgs[i]))
                        {
                            OpenLogFile(cmdArgs[i]);
                        }
                    }
                }
            }));
            return true;
        }

        private void ActivateWindow()
        {
            this.Show();
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
            else
                this.Activate();
        }

        public void SetupKeyboardHooks()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }

        // Handles system-wide keyboard hooks
        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            // Alt+Delete: Clear view of currently active log panel
            if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VkDelete && e.KeyboardState == GlobalKeyboardHook.KeyboardState.SysKeyDown && e.KeyboardData.Flags == GlobalKeyboardHook.LlkhfAltdown)
            {
                LogViewPanel logPanel = dockPanel1.ActiveDocument as LogViewPanel;
                if (logPanel != null)
                {
                    logPanel.ClearLogViews();
                    e.Handled = true;
                }
            }
        }

        public IDebwinController GetDebwinController()
        {
            return _debwinController;
        }


        #region "Filter Handling"

        public FilterPanel GetFilterPanel()
        {
            return _filterPanel;
        }

        // event when a filter was applied in the filter panel
        private void filterPanel_RequestedFilter(object sender, ApplyFilterEventArgs e)
        {
            if (_activeLogViewPanel == null)
            {
                MessageBox.Show("Please open a log view before applying filters.");
                return;
            }

            _activeLogViewPanel.HandleApplyFilterRequest(e /*, e.ExtendExistingFilter*/);
        }

        #endregion

        #region "Log View Handling"

        public void OpenNewLogView(ILogController logController, IQueryableLogView rootLogView, LogViewPanelOptions logViewOptions)
        {
            logController.MessageSourceError += LogController_MessageSourceError;

            var logPanel = new LogViewPanel(this, _userPreferences, logController, logViewOptions);
            logPanel.PushLogView(rootLogView);
            logPanel.Show(dockPanel1, DockState.Document);
            SetActiveLogView(logPanel);
            logPanel.SelectedLogMessage += logPanel_SelectedLogMessage;
            logPanel.Activated += LogPanel_Activated;

            if (_detailsPanelDockState != DockState.Hidden && _detailsPanelDockState != DockState.Unknown)
                _detailsPanel.Show(dockPanel1, _detailsPanelDockState);

            logPanel.FormClosed += LogViewPanel_FormClosed;
        }


        private void LogViewPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            // As long as we allow only one LogViewPanel per controller, we can close the controller together with the view
            ILogController logController = ((LogViewPanel)sender).LogController;
            _debwinController.RemoveLogController(logController);
            logController.Dispose();
        }

        private void LogController_MessageSourceError(object sender, MessageSourceErrorEventArgs e)
        {
            string messageText;
            MessageBoxIcon icon;
            if (e.MessageSourceError is DebwinUserException)   // Status Message that is intended for the user (i.e. more warning than error)
            {
                messageText = e.MessageSourceError.Message;
                icon = MessageBoxIcon.Warning;
            }
            else  // Real exception in Debwin, include Stack Trace!
            {
                messageText = e.MessageSourceError.ToString();
                icon = MessageBoxIcon.Error;
            }

            this.Invoke(new Action(() =>
                MessageBox.Show(messageText, "Error", MessageBoxButtons.OK, icon)
            ));
        }

        private void logPanel_SelectedLogMessage(object sender, LogMessageSelectedEventArgs e)
        {
            _detailsPanel.ShowMessageDetails(e?.Message);
        }

        public void OpenLogFile(string filepath = null)
        {
            StartPagePanel.TryOpenFile(this, filepath, false);
        }

        public void OpenUdpListener()
        {
            var newUdpSourceDialog = new CreateUdpSourceDialog();
            if (ShowDialogEx(newUdpSourceDialog) == DialogResult.OK)
            {
                var listener = new UdpMessageSource() { Port = newUdpSourceDialog.Port };
                var parser = newUdpSourceDialog.MessageParser;

                var logController = DebwinController.GetNewLogController(_debwinController, listener.GetName(), listener, parser);
                var rootView = new MemoryBasedLogView(_userPreferences.MaximumMessageCount);
                logController.AddView(rootView);
                OpenNewLogView(logController, rootView, new LogViewPanelOptions() { LiveCaptureMode = true });
                logController.GetMessageCollectors()[0].Start();
            }
        }

        public LogViewPanel GetActiveLogView()
        {
            return _activeLogViewPanel;
        }

        #endregion


        #region "Window Events"


        /// <summary>Like Form.ShowDialog(), but temporarily disables the stay-on-top flag of this window, so it does not cover the modal dialog (looks like freezed UI to the user).</summary>
        public DialogResult ShowDialogEx(Form dialog)
        {
            bool tmpTopMost = this.TopMost;
            this.TopMost = false;
            Application.DoEvents();
            DialogResult result = dialog.ShowDialog(this);
            this.TopMost = tmpTopMost;
            return result;
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Environment.CommandLine.Contains("/noclosequery"))
            {
                if (!_sessionEnding && MessageBox.Show("Are you sure you want to close Debwin4?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            _isDisposed = true;

            // async code in this event might run after disposing the form if we do not detach it before closing the form:
            this.dockPanel1.ActiveContentChanged -= this.dockPanel1_ActiveDocumentChanged;
            dockPanel1.SaveAsXml(GetDockPersistenceFile());

            _globalKeyboardHook?.Dispose();
            _filterPanel.Close();
            _jobAnalyzerPanel.Close();
            _logControllerPanel.Close();
            _logStructurePanel.Close();
            _detailsPanel.Close();

            // remember last main window position
            _userPreferences.MainWindowState = this.WindowState;
            if (WindowState == FormWindowState.Normal)
            {
                _userPreferences.MainWindowBounds = this.DesktopBounds;
            }
        }

        /// <summary>Checks if a window with the specified bounds would be visible on any of the computer`s screens.</summary>
        private bool IsVisibleOnAnyScreen(Rectangle windowBounds)
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.IntersectsWith(windowBounds))
                    return true;
            }
            return false;
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            if (Environment.CommandLine.Contains("/minimized"))
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else if (Environment.CommandLine.Contains("/maximized"))
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else if (Environment.CommandLine.Contains("/normal"))
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = _userPreferences.MainWindowState;
            }
            MainWindow_Resize(this, new EventArgs());
        }


        private void MainWindow_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized && _userPreferences.MainWindowHideOnMinimize)
            {
                this.Hide();
            }
            EnsureVisible(this);
        }

        private bool CheckIfScreenExists()
        {
            if (_currentScreenBeforeMinimize != null)
            {
                bool exists = false;
                foreach (Screen screen in Screen.AllScreens)
                {
                    if (screen.DeviceName == _currentScreenBeforeMinimize.DeviceName)
                    {
                        exists = true;
                        break;
                    }
                }
                return exists;
            }
            return true;
        }

        private void EnsureVisible(Form form)
        {
            Rectangle ctrlRect = form.DisplayRectangle;
            ctrlRect.Y = form.Top;
            ctrlRect.X = form.Left;
            Rectangle screenRect = Screen.GetWorkingArea(form);
            if (this.WindowState == FormWindowState.Minimized)
            {
                _currentScreenBeforeMinimize = Screen.FromControl(this);
                //Tweak the ctrl's Top and Left until it's fully visible. 
                _ctrlLeft1 += Math.Min(0, screenRect.Left + screenRect.Width - form.Left - form.Width);
                _ctrlLeft2 -= Math.Min(0, form.Left - screenRect.Left);
                _ctrlTop1 += Math.Min(0, screenRect.Top + screenRect.Height - form.Top - form.Height);
                _ctrlTop2 -= Math.Min(0, form.Top - screenRect.Top);
            }

            if (!CheckIfScreenExists())
            {
                //Tweak the ctrl's Top and Left until it's fully visible. 
                form.Left += _ctrlLeft1 + Math.Min(0, screenRect.Left + screenRect.Width - form.Left - form.Width);
                form.Left -= _ctrlLeft2;
                form.Left -= Math.Min(0, form.Left - screenRect.Left);
                form.Top -= _ctrlTop2;
                form.Top += _ctrlTop1 + Math.Min(0, screenRect.Top + screenRect.Height - form.Top - form.Height);
                form.Top -= Math.Min(0, form.Top - screenRect.Top);
            }
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                _userPreferences.MainWindowBounds = this.DesktopBounds;
            }
        }

        #endregion


        #region "Menu Item Events"

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new AboutDialog();
            ShowDialogEx(about);
        }

        private void addLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenLogFile();
        }

        private void addUDPListenerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenUdpListener();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideOnMinimizeMenuItem.Checked = false;
            this.Close();
        }

        private void logsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _logControllerPanel.Show(dockPanel1);
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Left)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void logStructureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _logStructurePanel.Show(dockPanel1);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool oldTopMostValue = this.TopMost;
            this.TopMost = false;  // not compatible with modal dialog
            var settingsDialog = new SettingsDialog(_userPreferences);
            ShowDialogEx(settingsDialog);
            this.TopMost = oldTopMostValue;

            if (_userPreferences.InstallKeyboardHook && _globalKeyboardHook == null)
            {
                SetupKeyboardHooks();
            }
            else
            {
                _globalKeyboardHook?.Dispose();
                _globalKeyboardHook = null;
            }
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _filterPanel.Show(dockPanel1);
        }


        private void miDateFormatOption_Click(object sender, EventArgs e)
        {
            miDateFormatTimeOnly.Checked = miDateFormatRelative.Checked = miDateFormatDateTime.Checked = miDateFormatRelativeToRef.Checked = false;
            (sender as ToolStripMenuItem).Checked = true;

            if (sender == miDateFormatDateTime)
            {
                _userPreferences.TimeFormatMode = TimeFormatMode.DateTime;
            }
            else if (sender == miDateFormatTimeOnly)
            {
                _userPreferences.TimeFormatMode = TimeFormatMode.TimeOnly;
            }
            else if (sender == miDateFormatRelative)
            {
                _userPreferences.TimeFormatMode = TimeFormatMode.RelativeTime;
            }
            else if (sender == miDateFormatRelativeToRef)
            {
                _userPreferences.TimeFormatMode = TimeFormatMode.RelativeToReference;
            }
        }

        private void ExitDebwinMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowTabsMenuItem_Click(object sender, EventArgs e)
        {
            _userPreferences.ShowTabChars = showTabsMenuItem.Checked;
        }

        private void stayOnTopToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            _userPreferences.MainWindowTopMost = stayOnTopToolStripMenuItem.Checked;
            this.TopMost = stayOnTopToolStripMenuItem.Checked;
        }

        private void hideOnMinimizeToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            notifyIcon.Visible = _userPreferences.MainWindowHideOnMinimize = hideOnMinimizeMenuItem.Checked;
        }

        private void startPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _startPagePanel.Show();
        }


        #endregion

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void restoreMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void clearMenuItem_Click(object sender, EventArgs e)
        {
            LogViewPanel logPanel = dockPanel1.ActiveDocument as LogViewPanel;
            if (logPanel != null)
            {
                logPanel.ClearLogViews();
            }
        }

        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chooseColumnsMenuItem_Click(object sender, EventArgs e)
        {
            var activeLogView = dockPanel1.ActiveDocument as LogViewPanel;
            if (activeLogView != null)
                activeLogView.ShowChooseColumnsDialog();
        }


        private void LogPanel_Activated(object sender, EventArgs e)
        {
            SetActiveLogView(sender as LogViewPanel);
        }


        private void dockPanel1_ActiveDocumentChanged(object sender, EventArgs e)
        {
            // this event is also triggered when un-docking a log (where the next tab becomes the new active document in the main window)
            // or when switching from a floating window to the filter panel --> only set the active doc if main window has the focus
            new Task(() =>  // delay this handler as it may happen that when focusing the filter panel, the active document of the main window gets the focus first (the ContainsFocus property below is a false-positive for a short time)
            {
                Thread.Sleep(25);
                try
                {
                    if (_isDisposed)
                        return;

                    this.Invoke(new Action(() =>
                    {
                        if (_isDisposed)
                            return;

                        if (Form.ActiveForm == this && (dockPanel1.ActiveDocument as DockContent).ContainsFocus)
                        {
                            SetActiveLogView((dockPanel1.ActiveDocument as LogViewPanel));
                        }
                    }));
                }
                catch (ObjectDisposedException) { /* form dispose may happen at any time during this task as ActiveDocumentChanged fires several times during form_closing */ }
            }).Start();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            // check for blocking firewall rules
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules");

                foreach (string subkey in key.GetValueNames())
                {
                    string value = Convert.ToString(key.GetValue(subkey)).ToLowerInvariant();
                    if (value.Contains($"App={Assembly.GetExecutingAssembly().Location}".ToLowerInvariant()) && value.Contains("action=block"))
                    {
                        MessageBox.Show("Debwin4 has been blocked by the firewall\n\nPlease allow Debwin4.exe to communicate through the firewall in the dialog that will pop up next. You might need to refer to your system administrator to do so.", "Debwin4", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Process.Start("control", "/name Microsoft.WindowsFirewall");
                        break;
                    }
                }
            }
            // but don't let the app start fail due to "whatever"
            catch
            { }

        }

        private void SetActiveLogView(LogViewPanel panel)
        {
            _activeLogViewPanel = panel;

            if (panel != null)
            {
                chooseColumnsMenuItem.Enabled = true;
                this.Text = "Debwin4 - " + panel.TabText.Replace("&&", "&"); // mnemonics are not processed in the title bar
            }
            else
            {
                chooseColumnsMenuItem.Enabled = false;
                this.Text = "Debwin4";
            }
        }

        private void MainWindow_StyleChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("change");
        }

        private void jobAnalyzerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_jobAnalyzerPanel.IsDisposed)
                _jobAnalyzerPanel = new LlJobAnalyzerPanel(this);
            _jobAnalyzerPanel.Show(dockPanel1);
        }
    }
}
