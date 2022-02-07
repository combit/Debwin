namespace Debwin.UI.Panels
{
    partial class LogViewPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogViewPanel));
            this.ilLogLevelIcons = new System.Windows.Forms.ImageList(this.components);
            this.listviewLayoutTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblReceivedMessages = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblReceivedMessageCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblBufferedMessages = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblBufferedMessagesCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFilterNameLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFilterNameValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVisibleMessages = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVisibleMessageCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAttachedFileNotification = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAttachedLogFileName = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLineNumberLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLineNumber = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelLogLoader = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panelLogView = new System.Windows.Forms.Panel();
            this.lstLogMessages = new Debwin.UI.Controls.DoubleBufferedListView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnCaptureActive = new System.Windows.Forms.ToolStripButton();
            this.btnClearLog = new System.Windows.Forms.ToolStripButton();
            this.btnToggleAutoScroll = new System.Windows.Forms.ToolStripButton();
            this.autoScrollToolstripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.btnSaveLog = new System.Windows.Forms.ToolStripSplitButton();
            this.btnSaveAndEditLog = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSaveWithSpecificColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCopyLog = new System.Windows.Forms.ToolStripButton();
            this.btnAppendMessage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSavedFilters = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnPopLogView = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnFindLastIssue = new System.Windows.Forms.ToolStripButton();
            this.btnFindNextIssue = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtSearchBox = new System.Windows.Forms.ToolStripComboBox();
            this.findTextMenuItem = new System.Windows.Forms.ToolStripSplitButton();
            this.findNextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findLastMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.caseSensitiveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.patternsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorCodeNegNumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLogMessageContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miSelectInUnfilteredView = new System.Windows.Forms.ToolStripMenuItem();
            this.copySelectedMessagesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bookmarkMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setBookmarkMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextBookmarkMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previousBookmarkMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyBookmarkedLinesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllBookmarksMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setBaseForRelativeTimeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chooseColumnsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byThreadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.includeLoggerInFilterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excludeLoggerInFilterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.includeModuleInFilterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excludeModuleInFilterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.panelLogLoader.SuspendLayout();
            this.panelLogView.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.mnuLogMessageContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // ilLogLevelIcons
            // 
            this.ilLogLevelIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilLogLevelIcons.ImageStream")));
            this.ilLogLevelIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilLogLevelIcons.Images.SetKeyName(0, "Tip.png");
            this.ilLogLevelIcons.Images.SetKeyName(1, "Information.png");
            this.ilLogLevelIcons.Images.SetKeyName(2, "Warning.png");
            this.ilLogLevelIcons.Images.SetKeyName(3, "Error.png");
            this.ilLogLevelIcons.Images.SetKeyName(4, "User-Comment.png");
            this.ilLogLevelIcons.Images.SetKeyName(5, "BookmarkTip.png");
            this.ilLogLevelIcons.Images.SetKeyName(6, "BookmarkInformation.png");
            this.ilLogLevelIcons.Images.SetKeyName(7, "BookmarkWarning.png");
            this.ilLogLevelIcons.Images.SetKeyName(8, "BookmarkError.png");
            this.ilLogLevelIcons.Images.SetKeyName(9, "BookmarkUser-Comment.png");
            // 
            // listviewLayoutTimer
            // 
            this.listviewLayoutTimer.Tick += new System.EventHandler(this.listviewLayoutTimer_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblReceivedMessages,
            this.lblReceivedMessageCount,
            this.lblBufferedMessages,
            this.lblBufferedMessagesCount,
            this.lblFilterNameLabel,
            this.lblFilterNameValue,
            this.lblVisibleMessages,
            this.lblVisibleMessageCount,
            this.lblAttachedFileNotification,
            this.lblAttachedLogFileName,
            this.lblLineNumberLabel,
            this.lblLineNumber});
            this.statusStrip1.Location = new System.Drawing.Point(0, 342);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(976, 24);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblReceivedMessages
            // 
            this.lblReceivedMessages.Name = "lblReceivedMessages";
            this.lblReceivedMessages.Size = new System.Drawing.Size(111, 19);
            this.lblReceivedMessages.Text = "Received Messages:";
            // 
            // lblReceivedMessageCount
            // 
            this.lblReceivedMessageCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblReceivedMessageCount.Name = "lblReceivedMessageCount";
            this.lblReceivedMessageCount.Size = new System.Drawing.Size(25, 19);
            this.lblReceivedMessageCount.Text = "{0}";
            this.lblReceivedMessageCount.ToolTipText = "Number of messages in this view which are currently not displayed until the next " +
    "refresh";
            // 
            // lblBufferedMessages
            // 
            this.lblBufferedMessages.Name = "lblBufferedMessages";
            this.lblBufferedMessages.Size = new System.Drawing.Size(109, 19);
            this.lblBufferedMessages.Text = "Buffered Messages:";
            // 
            // lblBufferedMessagesCount
            // 
            this.lblBufferedMessagesCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblBufferedMessagesCount.Name = "lblBufferedMessagesCount";
            this.lblBufferedMessagesCount.Size = new System.Drawing.Size(25, 19);
            this.lblBufferedMessagesCount.Text = "{0}";
            this.lblBufferedMessagesCount.ToolTipText = "Number of messages in this view which are currently not displayed until the next " +
    "refresh";
            // 
            // lblFilterNameLabel
            // 
            this.lblFilterNameLabel.Name = "lblFilterNameLabel";
            this.lblFilterNameLabel.Size = new System.Drawing.Size(36, 19);
            this.lblFilterNameLabel.Text = "Filter:";
            this.lblFilterNameLabel.Visible = false;
            // 
            // lblFilterNameValue
            // 
            this.lblFilterNameValue.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblFilterNameValue.Name = "lblFilterNameValue";
            this.lblFilterNameValue.Size = new System.Drawing.Size(51, 19);
            this.lblFilterNameValue.Text = "{Name}";
            this.lblFilterNameValue.Visible = false;
            // 
            // lblVisibleMessages
            // 
            this.lblVisibleMessages.Name = "lblVisibleMessages";
            this.lblVisibleMessages.Size = new System.Drawing.Size(98, 19);
            this.lblVisibleMessages.Text = "Visible Messages:";
            this.lblVisibleMessages.ToolTipText = "Number of messages in the buffer which match the filter and are currently visible" +
    "";
            this.lblVisibleMessages.Visible = false;
            // 
            // lblVisibleMessageCount
            // 
            this.lblVisibleMessageCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblVisibleMessageCount.Name = "lblVisibleMessageCount";
            this.lblVisibleMessageCount.Size = new System.Drawing.Size(25, 19);
            this.lblVisibleMessageCount.Text = "{0}";
            this.lblVisibleMessageCount.ToolTipText = "Number of messages in the buffer which match the filter and are currently visible" +
    "";
            this.lblVisibleMessageCount.Visible = false;
            // 
            // lblAttachedFileNotification
            // 
            this.lblAttachedFileNotification.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAttachedFileNotification.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblAttachedFileNotification.Image = ((System.Drawing.Image)(resources.GetObject("lblAttachedFileNotification.Image")));
            this.lblAttachedFileNotification.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAttachedFileNotification.Name = "lblAttachedFileNotification";
            this.lblAttachedFileNotification.Size = new System.Drawing.Size(259, 19);
            this.lblAttachedFileNotification.Spring = true;
            this.lblAttachedFileNotification.Text = "{lblAttachedFileNotification}";
            this.lblAttachedFileNotification.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAttachedFileNotification.Visible = false;
            // 
            // lblAttachedLogFileName
            // 
            this.lblAttachedLogFileName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAttachedLogFileName.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblAttachedLogFileName.IsLink = true;
            this.lblAttachedLogFileName.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.lblAttachedLogFileName.LinkColor = System.Drawing.Color.SteelBlue;
            this.lblAttachedLogFileName.Name = "lblAttachedLogFileName";
            this.lblAttachedLogFileName.Size = new System.Drawing.Size(151, 19);
            this.lblAttachedLogFileName.Text = "{lblAttachedLogFileName}";
            this.lblAttachedLogFileName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAttachedLogFileName.Visible = false;
            this.lblAttachedLogFileName.Click += new System.EventHandler(this.lblAttachedLogFileName_Click);
            // 
            // lblLineNumberLabel
            // 
            this.lblLineNumberLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblLineNumberLabel.Name = "lblLineNumberLabel";
            this.lblLineNumberLabel.Size = new System.Drawing.Size(670, 19);
            this.lblLineNumberLabel.Spring = true;
            this.lblLineNumberLabel.Text = "Ln";
            this.lblLineNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLineNumber
            // 
            this.lblLineNumber.Name = "lblLineNumber";
            this.lblLineNumber.Size = new System.Drawing.Size(21, 19);
            this.lblLineNumber.Text = "{0}";
            // 
            // panelLogLoader
            // 
            this.panelLogLoader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelLogLoader.Controls.Add(this.label1);
            this.panelLogLoader.Location = new System.Drawing.Point(1013, 22);
            this.panelLogLoader.Name = "panelLogLoader";
            this.panelLogLoader.Size = new System.Drawing.Size(164, 324);
            this.panelLogLoader.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 324);
            this.label1.TabIndex = 0;
            this.label1.Text = "Working on it...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelLogView
            // 
            this.panelLogView.Controls.Add(this.lstLogMessages);
            this.panelLogView.Controls.Add(this.toolStrip1);
            this.panelLogView.Controls.Add(this.statusStrip1);
            this.panelLogView.Location = new System.Drawing.Point(12, 12);
            this.panelLogView.Name = "panelLogView";
            this.panelLogView.Size = new System.Drawing.Size(976, 366);
            this.panelLogView.TabIndex = 9;
            // 
            // lstLogMessages
            // 
            this.lstLogMessages.AllowColumnReorder = true;
            this.lstLogMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLogMessages.Font = new System.Drawing.Font("Consolas", _userPreferences.ListLogMessageFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLogMessages.FullRowSelect = true;
            this.lstLogMessages.HideSelection = false;
            this.lstLogMessages.Location = new System.Drawing.Point(0, 25);
            this.lstLogMessages.Name = "lstLogMessages";
            this.lstLogMessages.ShowGroups = false;
            this.lstLogMessages.Size = new System.Drawing.Size(976, 317);
            this.lstLogMessages.SmallImageList = this.ilLogLevelIcons;
            this.lstLogMessages.TabIndex = 6;
            this.lstLogMessages.UseCompatibleStateImageBehavior = false;
            this.lstLogMessages.View = System.Windows.Forms.View.Details;
            this.lstLogMessages.VirtualMode = true;
            this.lstLogMessages.Scroll += new System.Windows.Forms.ScrollEventHandler(this.lstLogMessages_Scroll);
            this.lstLogMessages.ScrolledToEnd += new System.EventHandler(this.lstLogMessages_ScrolledToEnd);
            this.lstLogMessages.ColumnReordered += new System.Windows.Forms.ColumnReorderedEventHandler(this.lstLogMessages_ColumnReordered);
            this.lstLogMessages.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.LstLogMessages_RetrieveVirtualItem);
            this.lstLogMessages.SelectedIndexChanged += new System.EventHandler(this.lstLogMessages_SelectedIndexChanged);
            this.lstLogMessages.VirtualItemsSelectionRangeChanged += new System.Windows.Forms.ListViewVirtualItemsSelectionRangeChangedEventHandler(this.lstLogMessages_VirtualItemsSelectionRangeChanged);
            this.lstLogMessages.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstLogMessages_KeyDown);
            this.lstLogMessages.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstLogMessages_MouseClick);
            this.lstLogMessages.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.lstLogMessages_MouseWheel);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCaptureActive,
            this.btnClearLog,
            this.btnToggleAutoScroll,
            this.autoScrollToolstripSeparator,
            this.btnSaveLog,
            this.btnCopyLog,
            this.btnAppendMessage,
            this.toolStripSeparator1,
            this.btnSavedFilters,
            this.btnPopLogView,
            this.toolStripSeparator2,
            this.btnFindLastIssue,
            this.btnFindNextIssue,
            this.toolStripLabel1,
            this.txtSearchBox,
            this.findTextMenuItem});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(976, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnCaptureActive
            // 
            this.btnCaptureActive.Checked = true;
            this.btnCaptureActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnCaptureActive.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCaptureActive.Image = ((System.Drawing.Image)(resources.GetObject("btnCaptureActive.Image")));
            this.btnCaptureActive.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCaptureActive.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnCaptureActive.Name = "btnCaptureActive";
            this.btnCaptureActive.Size = new System.Drawing.Size(23, 22);
            this.btnCaptureActive.Text = "Turn logging on or off";
            this.btnCaptureActive.Click += new System.EventHandler(this.btnStartStopReceiver_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClearLog.Image = global::Debwin.UI.Properties.Resources.Waste_Bin;
            this.btnClearLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(23, 22);
            this.btnClearLog.Text = "Clear";
            this.btnClearLog.ToolTipText = "Clear all messages in the current view (Ctrl+Del)";
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnToggleAutoScroll
            // 
            this.btnToggleAutoScroll.Checked = true;
            this.btnToggleAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnToggleAutoScroll.Image = global::Debwin.UI.Properties.Resources.Autoscroll_On;
            this.btnToggleAutoScroll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToggleAutoScroll.Name = "btnToggleAutoScroll";
            this.btnToggleAutoScroll.Size = new System.Drawing.Size(87, 22);
            this.btnToggleAutoScroll.Text = "Auto-Scroll";
            this.btnToggleAutoScroll.ToolTipText = "Auto-Scroll to the end of the message list (F5)";
            this.btnToggleAutoScroll.Click += new System.EventHandler(this.btnToggleAutoScroll_Click);
            // 
            // autoScrollToolstripSeparator
            // 
            this.autoScrollToolstripSeparator.Name = "autoScrollToolstripSeparator";
            this.autoScrollToolstripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSaveLog
            // 
            this.btnSaveLog.DropDownButtonWidth = 16;
            this.btnSaveLog.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSaveAndEditLog,
            this.btnSaveWithSpecificColumns});
            this.btnSaveLog.Image = global::Debwin.UI.Properties.Resources.Save;
            this.btnSaveLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveLog.Name = "btnSaveLog";
            this.btnSaveLog.Size = new System.Drawing.Size(91, 22);
            this.btnSaveLog.Text = "Save Log";
            this.btnSaveLog.ToolTipText = "Saves the log to a file (Ctrl+S).\r\nIf multiple messages are selected, only these " +
    "messages are saved.";
            this.btnSaveLog.ButtonClick += new System.EventHandler(this.btnSaveLog_Click);
            // 
            // btnSaveAndEditLog
            // 
            this.btnSaveAndEditLog.Name = "btnSaveAndEditLog";
            this.btnSaveAndEditLog.Size = new System.Drawing.Size(242, 22);
            this.btnSaveAndEditLog.Text = "Save and Open in Editor";
            this.btnSaveAndEditLog.Click += new System.EventHandler(this.btnSaveLog_Click);
            // 
            // btnSaveWithSpecificColumns
            // 
            this.btnSaveWithSpecificColumns.Name = "btnSaveWithSpecificColumns";
            this.btnSaveWithSpecificColumns.Size = new System.Drawing.Size(242, 22);
            this.btnSaveWithSpecificColumns.Text = "Save Log with Specific Columns";
            this.btnSaveWithSpecificColumns.Click += new System.EventHandler(this.btnSaveWithSpecificColumns_Click);
            // 
            // btnCopyLog
            // 
            this.btnCopyLog.Image = global::Debwin.UI.Properties.Resources.Copy;
            this.btnCopyLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopyLog.Name = "btnCopyLog";
            this.btnCopyLog.Size = new System.Drawing.Size(78, 22);
            this.btnCopyLog.Text = "Copy Log";
            this.btnCopyLog.ToolTipText = "Copies the log to the clipboard (Ctrl+C)\r\nIf multiple messages are selected, only" +
    " these messages are copied.";
            this.btnCopyLog.Click += new System.EventHandler(this.btnSaveLog_Click);
            // 
            // btnAppendMessage
            // 
            this.btnAppendMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAppendMessage.Image = ((System.Drawing.Image)(resources.GetObject("btnAppendMessage.Image")));
            this.btnAppendMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAppendMessage.Name = "btnAppendMessage";
            this.btnAppendMessage.Size = new System.Drawing.Size(23, 22);
            this.btnAppendMessage.Text = "Add Comment";
            this.btnAppendMessage.ToolTipText = "Adds a custom message to the log";
            this.btnAppendMessage.Click += new System.EventHandler(this.btnAppendMessage_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSavedFilters
            // 
            this.btnSavedFilters.Image = ((System.Drawing.Image)(resources.GetObject("btnSavedFilters.Image")));
            this.btnSavedFilters.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSavedFilters.Name = "btnSavedFilters";
            this.btnSavedFilters.Size = new System.Drawing.Size(76, 22);
            this.btnSavedFilters.Text = "Filters...";
            this.btnSavedFilters.ToolTipText = "Load a saved filter";
            // 
            // btnPopLogView
            // 
            this.btnPopLogView.Enabled = false;
            this.btnPopLogView.Image = ((System.Drawing.Image)(resources.GetObject("btnPopLogView.Image")));
            this.btnPopLogView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPopLogView.Name = "btnPopLogView";
            this.btnPopLogView.Size = new System.Drawing.Size(83, 22);
            this.btnPopLogView.Text = "Clear Filter";
            this.btnPopLogView.ToolTipText = "Disables any filters so all messages are visible (Ctrl+Z)";
            this.btnPopLogView.Click += new System.EventHandler(this.btnPopLogView_ButtonClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnFindLastIssue
            // 
            this.btnFindLastIssue.Image = global::Debwin.UI.Properties.Resources.Arrow_Up_Orange;
            this.btnFindLastIssue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFindLastIssue.Name = "btnFindLastIssue";
            this.btnFindLastIssue.Size = new System.Drawing.Size(101, 22);
            this.btnFindLastIssue.Text = "Previous Issue";
            this.btnFindLastIssue.ToolTipText = "Go to previous warning/error (Ctrl+Up)";
            this.btnFindLastIssue.Click += new System.EventHandler(this.btnFindIssue_Click);
            // 
            // btnFindNextIssue
            // 
            this.btnFindNextIssue.Image = global::Debwin.UI.Properties.Resources.Arrow_Down_Orange;
            this.btnFindNextIssue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFindNextIssue.Name = "btnFindNextIssue";
            this.btnFindNextIssue.Size = new System.Drawing.Size(81, 22);
            this.btnFindNextIssue.Text = "Next Issue";
            this.btnFindNextIssue.ToolTipText = "Go to next warning/error (Ctrl+Down)";
            this.btnFindNextIssue.Click += new System.EventHandler(this.btnFindIssue_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(45, 22);
            this.toolStripLabel1.Text = "Search:";
            // 
            // txtSearchBox
            // 
            this.txtSearchBox.BackColor = System.Drawing.SystemColors.Window;
            this.txtSearchBox.Name = "txtSearchBox";
            this.txtSearchBox.Size = new System.Drawing.Size(150, 25);
            this.txtSearchBox.ToolTipText = "Find next item containg a text (Ctrl+F)\r\nEnclose in slashes to enable regex searc" +
    "h:  /regex/";
            this.txtSearchBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearchBox_KeyPress);
            this.txtSearchBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearchBox_KeyUp);
            this.txtSearchBox.Click += new System.EventHandler(this.txtSearchBox_Click);
            this.txtSearchBox.TextChanged += new System.EventHandler(this.txtSearchBox_TextChanged);
            // 
            // findTextMenuItem
            // 
            this.findTextMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.findTextMenuItem.DropDownButtonWidth = 16;
            this.findTextMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findNextMenuItem,
            this.findLastMenuItem,
            this.toolStripMenuItem2,
            this.caseSensitiveMenuItem,
            this.patternsToolStripMenuItem,
            this.goToLineToolStripMenuItem});
            this.findTextMenuItem.Image = global::Debwin.UI.Properties.Resources.Find;
            this.findTextMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findTextMenuItem.Name = "findTextMenuItem";
            this.findTextMenuItem.Size = new System.Drawing.Size(37, 22);
            this.findTextMenuItem.Text = "Search Text";
            this.findTextMenuItem.ButtonClick += new System.EventHandler(this.findTextMenuItem_ButtonClick);
            // 
            // findNextMenuItem
            // 
            this.findNextMenuItem.Name = "findNextMenuItem";
            this.findNextMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.findNextMenuItem.Size = new System.Drawing.Size(222, 22);
            this.findNextMenuItem.Text = "Find Next (Down)";
            this.findNextMenuItem.Click += new System.EventHandler(this.findTextMenuItem_ButtonClick);
            // 
            // findLastMenuItem
            // 
            this.findLastMenuItem.Name = "findLastMenuItem";
            this.findLastMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F3)));
            this.findLastMenuItem.Size = new System.Drawing.Size(222, 22);
            this.findLastMenuItem.Text = "Find Previous (Up)";
            this.findLastMenuItem.Click += new System.EventHandler(this.findTextMenuItem_ButtonClick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(219, 6);
            // 
            // caseSensitiveMenuItem
            // 
            this.caseSensitiveMenuItem.CheckOnClick = true;
            this.caseSensitiveMenuItem.Name = "caseSensitiveMenuItem";
            this.caseSensitiveMenuItem.Size = new System.Drawing.Size(222, 22);
            this.caseSensitiveMenuItem.Text = "Case Sensitive";
            // 
            // patternsToolStripMenuItem
            // 
            this.patternsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.errorCodeNegNumberToolStripMenuItem});
            this.patternsToolStripMenuItem.Name = "patternsToolStripMenuItem";
            this.patternsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.patternsToolStripMenuItem.Text = "Regex Patterns";
            // 
            // errorCodeNegNumberToolStripMenuItem
            // 
            this.errorCodeNegNumberToolStripMenuItem.Name = "errorCodeNegNumberToolStripMenuItem";
            this.errorCodeNegNumberToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.errorCodeNegNumberToolStripMenuItem.Text = "Error Code (Neg. Number)";
            this.errorCodeNegNumberToolStripMenuItem.Click += new System.EventHandler(this.errorCodeNegNumberToolStripMenuItem_Click);
            // 
            // goToLineToolStripMenuItem
            // 
            this.goToLineToolStripMenuItem.Name = "goToLineToolStripMenuItem";
            this.goToLineToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.goToLineToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.goToLineToolStripMenuItem.Text = "Go to Line";
            this.goToLineToolStripMenuItem.Click += new System.EventHandler(this.goToLineToolStripMenuItem_Click);
            // 
            // mnuLogMessageContext
            // 
            this.mnuLogMessageContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSelectInUnfilteredView,
            this.copySelectedMessagesMenuItem,
            this.bookmarkMenuItem,
            this.setBaseForRelativeTimeMenuItem,
            this.chooseColumnsMenuItem,
            this.toolStripMenuItem1,
            this.filterToolStripMenuItem,
            this.dateFromToolStripMenuItem,
            this.dateToToolStripMenuItem,
            this.byThreadToolStripMenuItem,
            this.includeLoggerInFilterMenuItem,
            this.excludeLoggerInFilterMenuItem,
            this.includeModuleInFilterMenuItem,
            this.excludeModuleInFilterMenuItem});
            this.mnuLogMessageContext.Name = "mnuLogMessageContext";
            this.mnuLogMessageContext.Size = new System.Drawing.Size(268, 296);
            this.mnuLogMessageContext.Opening += new System.ComponentModel.CancelEventHandler(this.mnuLogMessageContext_Opening);
            // 
            // miSelectInUnfilteredView
            // 
            this.miSelectInUnfilteredView.Enabled = false;
            this.miSelectInUnfilteredView.Image = ((System.Drawing.Image)(resources.GetObject("miSelectInUnfilteredView.Image")));
            this.miSelectInUnfilteredView.Name = "miSelectInUnfilteredView";
            this.miSelectInUnfilteredView.Size = new System.Drawing.Size(267, 22);
            this.miSelectInUnfilteredView.Text = "Undo Filters and Select This Message";
            this.miSelectInUnfilteredView.Click += new System.EventHandler(this.miSelectInUnfilteredView_Click);
            // 
            // copySelectedMessagesMenuItem
            // 
            this.copySelectedMessagesMenuItem.Image = global::Debwin.UI.Properties.Resources.Copy;
            this.copySelectedMessagesMenuItem.Name = "copySelectedMessagesMenuItem";
            this.copySelectedMessagesMenuItem.Size = new System.Drawing.Size(267, 22);
            this.copySelectedMessagesMenuItem.Text = "Copy Selected Messages";
            this.copySelectedMessagesMenuItem.Click += new System.EventHandler(this.copySelectedMessagesToolStripMenuItem_Click);
            // 
            // bookmarkMenuItem
            // 
            this.bookmarkMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setBookmarkMenuItem,
            this.nextBookmarkMenuItem,
            this.previousBookmarkMenuItem,
            this.copyBookmarkedLinesMenuItem,
            this.clearAllBookmarksMenuItem});
            this.bookmarkMenuItem.Name = "bookmarkMenuItem";
            this.bookmarkMenuItem.Size = new System.Drawing.Size(267, 22);
            this.bookmarkMenuItem.Text = "Bookmark";
            // 
            // setBookmarkMenuItem
            // 
            this.setBookmarkMenuItem.Image = global::Debwin.UI.Properties.Resources.Bookmark;
            this.setBookmarkMenuItem.Name = "setBookmarkMenuItem";
            this.setBookmarkMenuItem.Size = new System.Drawing.Size(231, 22);
            this.setBookmarkMenuItem.Text = "Toggle Bookmark (Ctrl+F2)";
            this.setBookmarkMenuItem.Click += new System.EventHandler(this.setBookmarkMenuItem_Click);
            // 
            // nextBookmarkMenuItem
            // 
            this.nextBookmarkMenuItem.Image = global::Debwin.UI.Properties.Resources.Arrow_Down_Orange;
            this.nextBookmarkMenuItem.Name = "nextBookmarkMenuItem";
            this.nextBookmarkMenuItem.Size = new System.Drawing.Size(231, 22);
            this.nextBookmarkMenuItem.Text = "Next Bookmark (F2)";
            this.nextBookmarkMenuItem.Click += new System.EventHandler(this.nextBookmarkMenuItem_Click);
            // 
            // previousBookmarkMenuItem
            // 
            this.previousBookmarkMenuItem.Image = global::Debwin.UI.Properties.Resources.Arrow_Up_Orange;
            this.previousBookmarkMenuItem.Name = "previousBookmarkMenuItem";
            this.previousBookmarkMenuItem.Size = new System.Drawing.Size(231, 22);
            this.previousBookmarkMenuItem.Text = "Previous Bookmark (Shift+F2)";
            this.previousBookmarkMenuItem.Click += new System.EventHandler(this.previousBookmarkMenuItem_Click);
            // 
            // copyBookmarkedLinesMenuItem
            // 
            this.copyBookmarkedLinesMenuItem.Image = global::Debwin.UI.Properties.Resources.Copy;
            this.copyBookmarkedLinesMenuItem.Name = "copyBookmarkedLinesMenuItem";
            this.copyBookmarkedLinesMenuItem.Size = new System.Drawing.Size(231, 22);
            this.copyBookmarkedLinesMenuItem.Text = "Copy Bookmarked Lines";
            this.copyBookmarkedLinesMenuItem.Click += new System.EventHandler(this.copyBookmarkedLinesMenuItem_Click);
            // 
            // clearAllBookmarksMenuItem
            // 
            this.clearAllBookmarksMenuItem.Image = global::Debwin.UI.Properties.Resources.Waste_Bin;
            this.clearAllBookmarksMenuItem.Name = "clearAllBookmarksMenuItem";
            this.clearAllBookmarksMenuItem.Size = new System.Drawing.Size(231, 22);
            this.clearAllBookmarksMenuItem.Text = "Clear All Bookmarks";
            this.clearAllBookmarksMenuItem.Click += new System.EventHandler(this.clearAllBookmarksMenuItem_Click);
            // 
            // setBaseForRelativeTimeMenuItem
            // 
            this.setBaseForRelativeTimeMenuItem.Name = "setBaseForRelativeTimeMenuItem";
            this.setBaseForRelativeTimeMenuItem.Size = new System.Drawing.Size(267, 22);
            this.setBaseForRelativeTimeMenuItem.Text = "Set as Base for Relative Timestamps";
            this.setBaseForRelativeTimeMenuItem.Click += new System.EventHandler(this.setBaseForRelativeTimeMenuItem_Click);
            // 
            // chooseColumnsMenuItem
            // 
            this.chooseColumnsMenuItem.Name = "chooseColumnsMenuItem";
            this.chooseColumnsMenuItem.Size = new System.Drawing.Size(267, 22);
            this.chooseColumnsMenuItem.Text = "Choose Columns...";
            this.chooseColumnsMenuItem.Click += new System.EventHandler(this.chooseColumnsMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(264, 6);
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.filterToolStripMenuItem.Enabled = false;
            this.filterToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(267, 22);
            this.filterToolStripMenuItem.Text = "Filter by";
            // 
            // dateFromToolStripMenuItem
            // 
            this.dateFromToolStripMenuItem.Name = "dateFromToolStripMenuItem";
            this.dateFromToolStripMenuItem.Size = new System.Drawing.Size(267, 22);
            this.dateFromToolStripMenuItem.Text = "Date/Time (From)";
            this.dateFromToolStripMenuItem.Click += new System.EventHandler(this.SetDateFilterToolStripMenuItem_Click);
            // 
            // dateToToolStripMenuItem
            // 
            this.dateToToolStripMenuItem.Name = "dateToToolStripMenuItem";
            this.dateToToolStripMenuItem.Size = new System.Drawing.Size(267, 22);
            this.dateToToolStripMenuItem.Text = "Date/Time (To)";
            this.dateToToolStripMenuItem.Click += new System.EventHandler(this.SetDateFilterToolStripMenuItem_Click);
            // 
            // byThreadToolStripMenuItem
            // 
            this.byThreadToolStripMenuItem.Name = "byThreadToolStripMenuItem";
            this.byThreadToolStripMenuItem.Size = new System.Drawing.Size(267, 22);
            this.byThreadToolStripMenuItem.Text = "Thread";
            this.byThreadToolStripMenuItem.Click += new System.EventHandler(this.byThreadToolStripMenuItem_Click);
            // 
            // includeLoggerInFilterMenuItem
            // 
            this.includeLoggerInFilterMenuItem.Name = "includeLoggerInFilterMenuItem";
            this.includeLoggerInFilterMenuItem.Size = new System.Drawing.Size(267, 22);
            this.includeLoggerInFilterMenuItem.Text = "Logger (Include)";
            this.includeLoggerInFilterMenuItem.Click += new System.EventHandler(this.includeLoggerInFilterMenuItem_Click);
            // 
            // excludeLoggerInFilterMenuItem
            // 
            this.excludeLoggerInFilterMenuItem.Name = "excludeLoggerInFilterMenuItem";
            this.excludeLoggerInFilterMenuItem.Size = new System.Drawing.Size(267, 22);
            this.excludeLoggerInFilterMenuItem.Text = "Logger (Exclude)";
            this.excludeLoggerInFilterMenuItem.Click += new System.EventHandler(this.excludeLoggerInFilterMenuItem_Click);
            // 
            // includeModuleInFilterMenuItem
            // 
            this.includeModuleInFilterMenuItem.Name = "includeModuleInFilterMenuItem";
            this.includeModuleInFilterMenuItem.Size = new System.Drawing.Size(267, 22);
            this.includeModuleInFilterMenuItem.Text = "Module (Include)";
            this.includeModuleInFilterMenuItem.Click += new System.EventHandler(this.includeModuleInFilterMenuItem_Click);
            // 
            // excludeModuleInFilterMenuItem
            // 
            this.excludeModuleInFilterMenuItem.Name = "excludeModuleInFilterMenuItem";
            this.excludeModuleInFilterMenuItem.Size = new System.Drawing.Size(267, 22);
            this.excludeModuleInFilterMenuItem.Text = "Module (Exclude)";
            this.excludeModuleInFilterMenuItem.Click += new System.EventHandler(this.excludeModuleInFilterMenuItem_Click);
            // 
            // LogViewPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 391);
            this.Controls.Add(this.panelLogView);
            this.Controls.Add(this.panelLogLoader);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "LogViewPanel";
            this.Text = "LogPanel";
            this.Load += new System.EventHandler(this.LogViewPanel_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelLogLoader.ResumeLayout(false);
            this.panelLogView.ResumeLayout(false);
            this.panelLogView.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.mnuLogMessageContext.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnToggleAutoScroll;
        private System.Windows.Forms.ToolStripButton btnClearLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Controls.DoubleBufferedListView lstLogMessages;
        private System.Windows.Forms.ImageList ilLogLevelIcons;
        private System.Windows.Forms.Timer listviewLayoutTimer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblVisibleMessageCount;
        private System.Windows.Forms.Panel panelLogLoader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelLogView;
        private System.Windows.Forms.ContextMenuStrip mnuLogMessageContext;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dateFromToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dateToToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miSelectInUnfilteredView;
        private System.Windows.Forms.ToolStripButton btnPopLogView;
        private System.Windows.Forms.ToolStripMenuItem byThreadToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnFindNextIssue;
        private System.Windows.Forms.ToolStripButton btnFindLastIssue;
        private System.Windows.Forms.ToolStripButton btnAppendMessage;
        private System.Windows.Forms.ToolStripSeparator autoScrollToolstripSeparator;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox txtSearchBox;
        private System.Windows.Forms.ToolStripButton btnCopyLog;
        private System.Windows.Forms.ToolStripMenuItem caseSensitiveMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem findLastMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findNextMenuItem;
        private System.Windows.Forms.ToolStripSplitButton findTextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copySelectedMessagesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem patternsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem errorCodeNegNumberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnSaveAndEditLog;
        private System.Windows.Forms.ToolStripMenuItem btnSaveWithSpecificColumns;
        private System.Windows.Forms.ToolStripSplitButton btnSaveLog;
        private System.Windows.Forms.ToolStripStatusLabel lblVisibleMessages;
        private System.Windows.Forms.ToolStripStatusLabel lblBufferedMessagesCount;
        private System.Windows.Forms.ToolStripStatusLabel lblBufferedMessages;
        private System.Windows.Forms.ToolStripMenuItem setBaseForRelativeTimeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bookmarkMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setBookmarkMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextBookmarkMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previousBookmarkMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllBookmarksMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyBookmarkedLinesMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem includeLoggerInFilterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excludeLoggerInFilterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem includeModuleInFilterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excludeModuleInFilterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chooseColumnsMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton btnSavedFilters;
        private System.Windows.Forms.ToolStripStatusLabel lblFilterNameLabel;
        private System.Windows.Forms.ToolStripStatusLabel lblFilterNameValue;
        private System.Windows.Forms.ToolStripStatusLabel lblAttachedFileNotification;
        private System.Windows.Forms.ToolStripStatusLabel lblAttachedLogFileName;
        private System.Windows.Forms.ToolStripStatusLabel lblReceivedMessages;
        private System.Windows.Forms.ToolStripStatusLabel lblReceivedMessageCount;
        private System.Windows.Forms.ToolStripButton btnCaptureActive;
        private System.Windows.Forms.ToolStripMenuItem goToLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel lblLineNumberLabel;
        private System.Windows.Forms.ToolStripStatusLabel lblLineNumber;
    }
}