namespace Debwin.UI.Forms
{
    partial class SettingsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTimeFormat = new System.Windows.Forms.TextBox();
            this.txtDateTimeFormat = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTextEditor = new System.Windows.Forms.TextBox();
            this.chkAutoScrollOnScrollToEnd = new System.Windows.Forms.CheckBox();
            this.chkDisableAutoScrollOnSelection = new System.Windows.Forms.CheckBox();
            this.btnChooseEditor = new System.Windows.Forms.Button();
            this.chooseEditorDialog = new System.Windows.Forms.OpenFileDialog();
            this.chkInstallGlobalKeyboardHook = new System.Windows.Forms.CheckBox();
            this.chkEnableAutostart = new System.Windows.Forms.CheckBox();
            this.chkIgnoreLogIndentation = new System.Windows.Forms.CheckBox();
            this.chkEnableAutoscrollOnClear = new System.Windows.Forms.CheckBox();
            this.trackMaxMessageCount = new System.Windows.Forms.TrackBar();
            this.chkLimitMessageCount = new System.Windows.Forms.CheckBox();
            this.lblMaxMessageCount = new System.Windows.Forms.Label();
            this.lblEstimatedMemoryUsage = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLogFilePath = new System.Windows.Forms.TextBox();
            this.btnChooseDirectory = new System.Windows.Forms.Button();
            this.chooseLogFilePathDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.chkLongTermMonitoring = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackMaxMessageCount)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Time Format:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(308, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Date+Time Format:";
            // 
            // txtTimeFormat
            // 
            this.txtTimeFormat.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtTimeFormat.Location = new System.Drawing.Point(3, 21);
            this.txtTimeFormat.Name = "txtTimeFormat";
            this.txtTimeFormat.Size = new System.Drawing.Size(302, 23);
            this.txtTimeFormat.TabIndex = 1;
            // 
            // txtDateTimeFormat
            // 
            this.txtDateTimeFormat.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtDateTimeFormat.Location = new System.Drawing.Point(311, 21);
            this.txtDateTimeFormat.Name = "txtDateTimeFormat";
            this.txtDateTimeFormat.Size = new System.Drawing.Size(303, 23);
            this.txtDateTimeFormat.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Text Editor:";
            // 
            // txtTextEditor
            // 
            this.txtTextEditor.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtTextEditor.Location = new System.Drawing.Point(3, 3);
            this.txtTextEditor.Name = "txtTextEditor";
            this.txtTextEditor.Size = new System.Drawing.Size(582, 23);
            this.txtTextEditor.TabIndex = 0;
            // 
            // chkAutoScrollOnScrollToEnd
            // 
            this.chkAutoScrollOnScrollToEnd.AutoSize = true;
            this.chkAutoScrollOnScrollToEnd.Location = new System.Drawing.Point(3, 260);
            this.chkAutoScrollOnScrollToEnd.Name = "chkAutoScrollOnScrollToEnd";
            this.chkAutoScrollOnScrollToEnd.Size = new System.Drawing.Size(444, 19);
            this.chkAutoScrollOnScrollToEnd.TabIndex = 6;
            this.chkAutoScrollOnScrollToEnd.Text = "Automatically enable Auto-Scroll when the message list was scrolled to the end";
            this.chkAutoScrollOnScrollToEnd.UseVisualStyleBackColor = true;
            // 
            // chkDisableAutoScrollOnSelection
            // 
            this.chkDisableAutoScrollOnSelection.AutoSize = true;
            this.chkDisableAutoScrollOnSelection.Location = new System.Drawing.Point(3, 235);
            this.chkDisableAutoScrollOnSelection.Name = "chkDisableAutoScrollOnSelection";
            this.chkDisableAutoScrollOnSelection.Size = new System.Drawing.Size(370, 19);
            this.chkDisableAutoScrollOnSelection.TabIndex = 5;
            this.chkDisableAutoScrollOnSelection.Text = "Automatically disable Auto-Scroll when a log message is selected";
            this.chkDisableAutoScrollOnSelection.UseVisualStyleBackColor = true;
            // 
            // btnChooseEditor
            // 
            this.btnChooseEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChooseEditor.Image = ((System.Drawing.Image)(resources.GetObject("btnChooseEditor.Image")));
            this.btnChooseEditor.Location = new System.Drawing.Point(591, 3);
            this.btnChooseEditor.Name = "btnChooseEditor";
            this.btnChooseEditor.Size = new System.Drawing.Size(23, 23);
            this.btnChooseEditor.TabIndex = 1;
            this.btnChooseEditor.UseVisualStyleBackColor = true;
            this.btnChooseEditor.Click += new System.EventHandler(this.btnChooseEditor_Click);
            // 
            // chooseEditorDialog
            // 
            this.chooseEditorDialog.Filter = "Executables (.exe)|*.exe";
            // 
            // chkInstallGlobalKeyboardHook
            // 
            this.chkInstallGlobalKeyboardHook.AutoSize = true;
            this.chkInstallGlobalKeyboardHook.Location = new System.Drawing.Point(3, 310);
            this.chkInstallGlobalKeyboardHook.Name = "chkInstallGlobalKeyboardHook";
            this.chkInstallGlobalKeyboardHook.Size = new System.Drawing.Size(297, 19);
            this.chkInstallGlobalKeyboardHook.TabIndex = 8;
            this.chkInstallGlobalKeyboardHook.Text = "Enable system-wide hotkey \'Alt+Delete\' to clear log";
            this.chkInstallGlobalKeyboardHook.UseVisualStyleBackColor = true;
            // 
            // chkEnableAutostart
            // 
            this.chkEnableAutostart.AutoSize = true;
            this.chkEnableAutostart.Location = new System.Drawing.Point(3, 335);
            this.chkEnableAutostart.Name = "chkEnableAutostart";
            this.chkEnableAutostart.Size = new System.Drawing.Size(248, 19);
            this.chkEnableAutostart.TabIndex = 9;
            this.chkEnableAutostart.Text = "Automatically launch on Windows startup";
            this.chkEnableAutostart.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreLogIndentation
            // 
            this.chkIgnoreLogIndentation.AutoSize = true;
            this.chkIgnoreLogIndentation.Location = new System.Drawing.Point(3, 360);
            this.chkIgnoreLogIndentation.Name = "chkIgnoreLogIndentation";
            this.chkIgnoreLogIndentation.Size = new System.Drawing.Size(221, 19);
            this.chkIgnoreLogIndentation.TabIndex = 10;
            this.chkIgnoreLogIndentation.Text = "Remove indentation of log messages";
            this.chkIgnoreLogIndentation.UseVisualStyleBackColor = true;
            // 
            // chkEnableAutoscrollOnClear
            // 
            this.chkEnableAutoscrollOnClear.AutoSize = true;
            this.chkEnableAutoscrollOnClear.Location = new System.Drawing.Point(3, 285);
            this.chkEnableAutoscrollOnClear.Name = "chkEnableAutoscrollOnClear";
            this.chkEnableAutoscrollOnClear.Size = new System.Drawing.Size(325, 19);
            this.chkEnableAutoscrollOnClear.TabIndex = 7;
            this.chkEnableAutoscrollOnClear.Text = "Automatically enable Auto-Scroll when the log is cleared";
            this.chkEnableAutoscrollOnClear.UseVisualStyleBackColor = true;
            // 
            // trackMaxMessageCount
            // 
            this.trackMaxMessageCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackMaxMessageCount.LargeChange = 50000;
            this.trackMaxMessageCount.Location = new System.Drawing.Point(13, 3);
            this.trackMaxMessageCount.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
            this.trackMaxMessageCount.Maximum = 5000000;
            this.trackMaxMessageCount.Name = "trackMaxMessageCount";
            this.trackMaxMessageCount.Size = new System.Drawing.Size(289, 45);
            this.trackMaxMessageCount.SmallChange = 10000;
            this.trackMaxMessageCount.TabIndex = 0;
            this.trackMaxMessageCount.TickFrequency = 100000;
            this.trackMaxMessageCount.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackMaxMessageCount.Value = 1000000;
            this.trackMaxMessageCount.Scroll += new System.EventHandler(this.trackMaxMessageCount_Scroll);
            this.trackMaxMessageCount.ValueChanged += new System.EventHandler(this.trackMaxMessageCount_ValueChanged);
            // 
            // chkLimitMessageCount
            // 
            this.chkLimitMessageCount.AutoSize = true;
            this.chkLimitMessageCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLimitMessageCount.Location = new System.Drawing.Point(3, 101);
            this.chkLimitMessageCount.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.chkLimitMessageCount.Name = "chkLimitMessageCount";
            this.chkLimitMessageCount.Size = new System.Drawing.Size(611, 19);
            this.chkLimitMessageCount.TabIndex = 2;
            this.chkLimitMessageCount.Text = "Use a ring buffer to keep only the last N messages in the view when live-capturin" +
    "g:";
            this.chkLimitMessageCount.UseVisualStyleBackColor = true;
            this.chkLimitMessageCount.CheckedChanged += new System.EventHandler(this.chkLimitMessageCount_CheckedChanged);
            // 
            // lblMaxMessageCount
            // 
            this.lblMaxMessageCount.AutoSize = true;
            this.lblMaxMessageCount.Location = new System.Drawing.Point(3, 0);
            this.lblMaxMessageCount.Name = "lblMaxMessageCount";
            this.lblMaxMessageCount.Size = new System.Drawing.Size(98, 15);
            this.lblMaxMessageCount.TabIndex = 0;
            this.lblMaxMessageCount.Tag = "Maximum Message Count: {0:n0}";
            this.lblMaxMessageCount.Text = "{max msg count}";
            // 
            // lblEstimatedMemoryUsage
            // 
            this.lblEstimatedMemoryUsage.AutoSize = true;
            this.lblEstimatedMemoryUsage.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblEstimatedMemoryUsage.Location = new System.Drawing.Point(3, 15);
            this.lblEstimatedMemoryUsage.Name = "lblEstimatedMemoryUsage";
            this.lblEstimatedMemoryUsage.Size = new System.Drawing.Size(92, 15);
            this.lblEstimatedMemoryUsage.TabIndex = 1;
            this.lblEstimatedMemoryUsage.Tag = "Estimated max. memory usage: {0}";
            this.lblEstimatedMemoryUsage.Text = "{estim memory}";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.Controls.Add(this.txtTextEditor, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnChooseEditor, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 15);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(617, 29);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtTimeFormat, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtDateTimeFormat, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 44);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(617, 47);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.trackMaxMessageCount, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 126);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(611, 51);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.lblMaxMessageCount);
            this.flowLayoutPanel2.Controls.Add(this.lblEstimatedMemoryUsage);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(308, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(300, 30);
            this.flowLayoutPanel2.TabIndex = 1;
            this.flowLayoutPanel2.WrapContents = false;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.label4, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel3, 0, 14);
            this.tableLayoutPanel4.Controls.Add(this.chkIgnoreLogIndentation, 0, 12);
            this.tableLayoutPanel4.Controls.Add(this.chkEnableAutostart, 0, 11);
            this.tableLayoutPanel4.Controls.Add(this.chkInstallGlobalKeyboardHook, 0, 10);
            this.tableLayoutPanel4.Controls.Add(this.chkEnableAutoscrollOnClear, 0, 9);
            this.tableLayoutPanel4.Controls.Add(this.chkAutoScrollOnScrollToEnd, 0, 8);
            this.tableLayoutPanel4.Controls.Add(this.chkDisableAutoScrollOnSelection, 0, 7);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.chkLimitMessageCount, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel3, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.chkLongTermMonitoring, 0, 13);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(7, 7);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 16;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(617, 490);
            this.tableLayoutPanel4.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 180);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Logfile Path:";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel3.Controls.Add(this.btnCancel);
            this.flowLayoutPanel3.Controls.Add(this.btnApply);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 427);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(617, 35);
            this.flowLayoutPanel3.TabIndex = 11;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(533, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 29);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(446, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(81, 29);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "OK";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel5.Controls.Add(this.txtLogFilePath, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnChooseDirectory, 1, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 198);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(611, 31);
            this.tableLayoutPanel5.TabIndex = 4;
            // 
            // txtLogFilePath
            // 
            this.txtLogFilePath.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtLogFilePath.Location = new System.Drawing.Point(3, 3);
            this.txtLogFilePath.Name = "txtLogFilePath";
            this.txtLogFilePath.Size = new System.Drawing.Size(576, 23);
            this.txtLogFilePath.TabIndex = 0;
            // 
            // btnChooseDirectory
            // 
            this.btnChooseDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChooseDirectory.Image = ((System.Drawing.Image)(resources.GetObject("btnChooseDirectory.Image")));
            this.btnChooseDirectory.Location = new System.Drawing.Point(585, 3);
            this.btnChooseDirectory.Name = "btnChooseDirectory";
            this.btnChooseDirectory.Size = new System.Drawing.Size(23, 23);
            this.btnChooseDirectory.TabIndex = 1;
            this.btnChooseDirectory.UseVisualStyleBackColor = true;
            this.btnChooseDirectory.Click += new System.EventHandler(this.btnChooseDirectory_Click);
            // 
            // chooseLogFilePathDialog
            // 
            this.chooseLogFilePathDialog.Description = "Choose the path for debug log files:";
            // 
            // chkLongTermMonitoring
            // 
            this.chkLongTermMonitoring.AutoSize = true;
            this.chkLongTermMonitoring.Location = new System.Drawing.Point(3, 385);
            this.chkLongTermMonitoring.Name = "chkLongTermMonitoring";
            this.chkLongTermMonitoring.Size = new System.Drawing.Size(146, 19);
            this.chkLongTermMonitoring.TabIndex = 12;
            this.chkLongTermMonitoring.Text = "Long-term monitoring";
            this.chkLongTermMonitoring.UseVisualStyleBackColor = true;
            // 
            // SettingsDialog
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(631, 504);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.trackMaxMessageCount)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTimeFormat;
        private System.Windows.Forms.TextBox txtDateTimeFormat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTextEditor;
        private System.Windows.Forms.CheckBox chkAutoScrollOnScrollToEnd;
        private System.Windows.Forms.CheckBox chkDisableAutoScrollOnSelection;
        private System.Windows.Forms.Button btnChooseEditor;
        private System.Windows.Forms.OpenFileDialog chooseEditorDialog;
        private System.Windows.Forms.CheckBox chkInstallGlobalKeyboardHook;
        private System.Windows.Forms.CheckBox chkEnableAutostart;
        private System.Windows.Forms.CheckBox chkIgnoreLogIndentation;
        private System.Windows.Forms.CheckBox chkEnableAutoscrollOnClear;
        private System.Windows.Forms.TrackBar trackMaxMessageCount;
        private System.Windows.Forms.CheckBox chkLimitMessageCount;
        private System.Windows.Forms.Label lblMaxMessageCount;
        private System.Windows.Forms.Label lblEstimatedMemoryUsage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TextBox txtLogFilePath;
        private System.Windows.Forms.Button btnChooseDirectory;
        private System.Windows.Forms.FolderBrowserDialog chooseLogFilePathDialog;
        private System.Windows.Forms.CheckBox chkLongTermMonitoring;
    }
}