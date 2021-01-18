namespace Debwin.UI
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addUDPListenerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logStructureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobAnalyzerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.chooseColumnsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stayOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideOnMinimizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.showTabsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateTimeFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miDateFormatTimeOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.miDateFormatDateTime = new System.Windows.Forms.ToolStripMenuItem();
            this.miDateFormatRelative = new System.Windows.Forms.ToolStripMenuItem();
            this.miDateFormatRelativeToRef = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restoreMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.notifyIconContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourcesToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1083, 24);
            this.menuStrip1.TabIndex = 2;
            // 
            // sourcesToolStripMenuItem
            // 
            this.sourcesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLogFileToolStripMenuItem,
            this.addUDPListenerToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem});
            this.sourcesToolStripMenuItem.Name = "sourcesToolStripMenuItem";
            this.sourcesToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.sourcesToolStripMenuItem.Text = "File";
            // 
            // addLogFileToolStripMenuItem
            // 
            this.addLogFileToolStripMenuItem.Name = "addLogFileToolStripMenuItem";
            this.addLogFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.addLogFileToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.addLogFileToolStripMenuItem.Text = "Open Log File...";
            this.addLogFileToolStripMenuItem.Click += new System.EventHandler(this.addLogFileToolStripMenuItem_Click);
            // 
            // addUDPListenerToolStripMenuItem
            // 
            this.addUDPListenerToolStripMenuItem.Name = "addUDPListenerToolStripMenuItem";
            this.addUDPListenerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.addUDPListenerToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.addUDPListenerToolStripMenuItem.Text = "Start UDP Listener...";
            this.addUDPListenerToolStripMenuItem.Click += new System.EventHandler(this.addUDPListenerToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(216, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click_1);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logsToolStripMenuItem,
            this.logStructureToolStripMenuItem,
            this.filterToolStripMenuItem,
            this.jobAnalyzerToolStripMenuItem,
            this.startPageToolStripMenuItem,
            this.toolStripMenuItem4,
            this.chooseColumnsMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // logsToolStripMenuItem
            // 
            this.logsToolStripMenuItem.Name = "logsToolStripMenuItem";
            this.logsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.logsToolStripMenuItem.Text = "Logs";
            this.logsToolStripMenuItem.Visible = false;
            this.logsToolStripMenuItem.Click += new System.EventHandler(this.logsToolStripMenuItem_Click);
            // 
            // logStructureToolStripMenuItem
            // 
            this.logStructureToolStripMenuItem.Name = "logStructureToolStripMenuItem";
            this.logStructureToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.logStructureToolStripMenuItem.Text = "Log Structure";
            this.logStructureToolStripMenuItem.Visible = false;
            this.logStructureToolStripMenuItem.Click += new System.EventHandler(this.logStructureToolStripMenuItem_Click);
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.filterToolStripMenuItem.Text = "Filter";
            this.filterToolStripMenuItem.Click += new System.EventHandler(this.filterToolStripMenuItem_Click);
            // 
            // jobAnalyzerToolStripMenuItem
            // 
            this.jobAnalyzerToolStripMenuItem.Name = "jobAnalyzerToolStripMenuItem";
            this.jobAnalyzerToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.jobAnalyzerToolStripMenuItem.Text = "Job Analyzer";
            this.jobAnalyzerToolStripMenuItem.Click += new System.EventHandler(this.jobAnalyzerToolStripMenuItem_Click);
            // 
            // startPageToolStripMenuItem
            // 
            this.startPageToolStripMenuItem.Name = "startPageToolStripMenuItem";
            this.startPageToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.startPageToolStripMenuItem.Text = "Start Page";
            this.startPageToolStripMenuItem.Click += new System.EventHandler(this.startPageToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(171, 6);
            // 
            // chooseColumnsMenuItem
            // 
            this.chooseColumnsMenuItem.Enabled = false;
            this.chooseColumnsMenuItem.Name = "chooseColumnsMenuItem";
            this.chooseColumnsMenuItem.Size = new System.Drawing.Size(174, 22);
            this.chooseColumnsMenuItem.Text = "Choose Columns...";
            this.chooseColumnsMenuItem.Click += new System.EventHandler(this.chooseColumnsMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stayOnTopToolStripMenuItem,
            this.hideOnMinimizeMenuItem,
            this.toolStripMenuItem2,
            this.showTabsMenuItem,
            this.dateTimeFormatToolStripMenuItem,
            this.toolStripMenuItem1,
            this.settingsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // stayOnTopToolStripMenuItem
            // 
            this.stayOnTopToolStripMenuItem.CheckOnClick = true;
            this.stayOnTopToolStripMenuItem.Name = "stayOnTopToolStripMenuItem";
            this.stayOnTopToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.stayOnTopToolStripMenuItem.Text = "Stay On Top";
            this.stayOnTopToolStripMenuItem.Click += new System.EventHandler(this.stayOnTopToolStripMenuItem_Click_1);
            // 
            // hideOnMinimizeMenuItem
            // 
            this.hideOnMinimizeMenuItem.CheckOnClick = true;
            this.hideOnMinimizeMenuItem.Name = "hideOnMinimizeMenuItem";
            this.hideOnMinimizeMenuItem.Size = new System.Drawing.Size(172, 22);
            this.hideOnMinimizeMenuItem.Text = "Hide On Minimize";
            this.hideOnMinimizeMenuItem.CheckStateChanged += new System.EventHandler(this.hideOnMinimizeToolStripMenuItem_CheckStateChanged);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(169, 6);
            // 
            // showTabsMenuItem
            // 
            this.showTabsMenuItem.CheckOnClick = true;
            this.showTabsMenuItem.Name = "showTabsMenuItem";
            this.showTabsMenuItem.Size = new System.Drawing.Size(172, 22);
            this.showTabsMenuItem.Text = "Display Tabs As \'\\t\'";
            this.showTabsMenuItem.Click += new System.EventHandler(this.ShowTabsMenuItem_Click);
            // 
            // dateTimeFormatToolStripMenuItem
            // 
            this.dateTimeFormatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDateFormatTimeOnly,
            this.miDateFormatDateTime,
            this.miDateFormatRelative,
            this.miDateFormatRelativeToRef});
            this.dateTimeFormatToolStripMenuItem.Name = "dateTimeFormatToolStripMenuItem";
            this.dateTimeFormatToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.dateTimeFormatToolStripMenuItem.Text = "Date/Time Format";
            // 
            // miDateFormatTimeOnly
            // 
            this.miDateFormatTimeOnly.Checked = true;
            this.miDateFormatTimeOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miDateFormatTimeOnly.Name = "miDateFormatTimeOnly";
            this.miDateFormatTimeOnly.Size = new System.Drawing.Size(284, 22);
            this.miDateFormatTimeOnly.Text = "Time";
            this.miDateFormatTimeOnly.Click += new System.EventHandler(this.miDateFormatOption_Click);
            // 
            // miDateFormatDateTime
            // 
            this.miDateFormatDateTime.Name = "miDateFormatDateTime";
            this.miDateFormatDateTime.Size = new System.Drawing.Size(284, 22);
            this.miDateFormatDateTime.Text = "Date + Time";
            this.miDateFormatDateTime.Click += new System.EventHandler(this.miDateFormatOption_Click);
            // 
            // miDateFormatRelative
            // 
            this.miDateFormatRelative.Name = "miDateFormatRelative";
            this.miDateFormatRelative.Size = new System.Drawing.Size(284, 22);
            this.miDateFormatRelative.Text = "Time Difference (To Previous Message)";
            this.miDateFormatRelative.Click += new System.EventHandler(this.miDateFormatOption_Click);
            // 
            // miDateFormatRelativeToRef
            // 
            this.miDateFormatRelativeToRef.Name = "miDateFormatRelativeToRef";
            this.miDateFormatRelativeToRef.Size = new System.Drawing.Size(284, 22);
            this.miDateFormatRelativeToRef.Text = "Time Difference (To Reference Message)";
            this.miDateFormatRelativeToRef.Visible = false;
            this.miDateFormatRelativeToRef.Click += new System.EventHandler(this.miDateFormatOption_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(169, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dockPanel1.Location = new System.Drawing.Point(0, 24);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(1083, 622);
            this.dockPanel1.TabIndex = 6;
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipText = "Debwin was minimized";
            this.notifyIcon.ContextMenuStrip = this.notifyIconContextMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Debwin4";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon1_Click);
            // 
            // notifyIconContextMenu
            // 
            this.notifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreMenuItem,
            this.clearMenuItem,
            this.closeMenuItem});
            this.notifyIconContextMenu.Name = "notifyIconContextMenu";
            this.notifyIconContextMenu.Size = new System.Drawing.Size(114, 70);
            // 
            // restoreMenuItem
            // 
            this.restoreMenuItem.Name = "restoreMenuItem";
            this.restoreMenuItem.Size = new System.Drawing.Size(113, 22);
            this.restoreMenuItem.Text = "Restore";
            this.restoreMenuItem.Click += new System.EventHandler(this.restoreMenuItem_Click);
            // 
            // clearMenuItem
            // 
            this.clearMenuItem.Name = "clearMenuItem";
            this.clearMenuItem.Size = new System.Drawing.Size(113, 22);
            this.clearMenuItem.Text = "Clear";
            this.clearMenuItem.Click += new System.EventHandler(this.clearMenuItem_Click);
            // 
            // closeMenuItem
            // 
            this.closeMenuItem.Name = "closeMenuItem";
            this.closeMenuItem.Size = new System.Drawing.Size(113, 22);
            this.closeMenuItem.Text = "Close";
            this.closeMenuItem.Click += new System.EventHandler(this.closeMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1083, 646);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(900, 560);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debwin4";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.LocationChanged += new System.EventHandler(this.MainWindow_LocationChanged);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.notifyIconContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.ToolStripMenuItem sourcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addLogFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addUDPListenerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logStructureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miDateFormatDateTime;
        private System.Windows.Forms.ToolStripMenuItem miDateFormatTimeOnly;
        private System.Windows.Forms.ToolStripMenuItem dateTimeFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stayOnTopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miDateFormatRelative;
        private System.Windows.Forms.ToolStripMenuItem showTabsMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem hideOnMinimizeMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem startPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miDateFormatRelativeToRef;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip notifyIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem restoreMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem chooseColumnsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jobAnalyzerToolStripMenuItem;
    }
}

