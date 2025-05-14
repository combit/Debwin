namespace Debwin.UI.Panels
{
    partial class FilterPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterPanel));
            this.label1 = new System.Windows.Forms.Label();
            this.rbDebug = new System.Windows.Forms.RadioButton();
            this.rbInfo = new System.Windows.Forms.RadioButton();
            this.rbWarning = new System.Windows.Forms.RadioButton();
            this.rbError = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDateFrom = new System.Windows.Forms.TextBox();
            this.txtDateTo = new System.Windows.Forms.TextBox();
            this.txtTextSearch = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLogger = new System.Windows.Forms.TextBox();
            this.lblLoggerName = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtModule = new System.Windows.Forms.TextBox();
            this.txtFilterName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtThread = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.logLevelsTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.criteriaTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnApplyFilter = new System.Windows.Forms.ToolStripButton();
            this.btnSaveFilter = new System.Windows.Forms.ToolStripButton();
            this.btnEditFilter = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnDeleteFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClearFilter = new System.Windows.Forms.ToolStripButton();
            this.filterNamePanel = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.logLevelsTableLayout.SuspendLayout();
            this.criteriaTableLayout.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.filterNamePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Minimum Level:";
            // 
            // rbDebug
            // 
            this.rbDebug.AutoSize = true;
            this.rbDebug.Checked = true;
            this.rbDebug.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDebug.Image = global::Debwin.UI.Properties.Resources.Tip;
            this.rbDebug.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbDebug.Location = new System.Drawing.Point(3, 3);
            this.rbDebug.Name = "rbDebug";
            this.rbDebug.Size = new System.Drawing.Size(51, 24);
            this.rbDebug.TabIndex = 1;
            this.rbDebug.TabStop = true;
            this.rbDebug.Text = "  ";
            this.rbDebug.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rbDebug.UseVisualStyleBackColor = true;
            this.rbDebug.Click += new System.EventHandler(this.rbSeverity_Click);
            // 
            // rbInfo
            // 
            this.rbInfo.AutoSize = true;
            this.rbInfo.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbInfo.Image = global::Debwin.UI.Properties.Resources.Information;
            this.rbInfo.Location = new System.Drawing.Point(168, 3);
            this.rbInfo.Name = "rbInfo";
            this.rbInfo.Size = new System.Drawing.Size(47, 24);
            this.rbInfo.TabIndex = 2;
            this.rbInfo.Text = " ";
            this.rbInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rbInfo.UseVisualStyleBackColor = true;
            this.rbInfo.Click += new System.EventHandler(this.rbSeverity_Click);
            // 
            // rbWarning
            // 
            this.rbWarning.AutoSize = true;
            this.rbWarning.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbWarning.Image = global::Debwin.UI.Properties.Resources.Warning;
            this.rbWarning.Location = new System.Drawing.Point(333, 3);
            this.rbWarning.Name = "rbWarning";
            this.rbWarning.Size = new System.Drawing.Size(47, 24);
            this.rbWarning.TabIndex = 3;
            this.rbWarning.Text = " ";
            this.rbWarning.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rbWarning.UseVisualStyleBackColor = true;
            this.rbWarning.Click += new System.EventHandler(this.rbSeverity_Click);
            // 
            // rbError
            // 
            this.rbError.AutoSize = true;
            this.rbError.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbError.Image = global::Debwin.UI.Properties.Resources.Error;
            this.rbError.Location = new System.Drawing.Point(498, 3);
            this.rbError.Name = "rbError";
            this.rbError.Size = new System.Drawing.Size(47, 24);
            this.rbError.TabIndex = 4;
            this.rbError.Text = " ";
            this.rbError.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rbError.UseVisualStyleBackColor = true;
            this.rbError.Click += new System.EventHandler(this.rbSeverity_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 203);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label2.Size = new System.Drawing.Size(104, 21);
            this.label2.TabIndex = 5;
            this.label2.Text = "Date/Time (From):";
            // 
            // txtDateFrom
            // 
            this.txtDateFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDateFrom.Location = new System.Drawing.Point(3, 227);
            this.txtDateFrom.Name = "txtDateFrom";
            this.txtDateFrom.Size = new System.Drawing.Size(661, 23);
            this.txtDateFrom.TabIndex = 6;
            // 
            // txtDateTo
            // 
            this.txtDateTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDateTo.Location = new System.Drawing.Point(3, 277);
            this.txtDateTo.Name = "txtDateTo";
            this.txtDateTo.Size = new System.Drawing.Size(661, 23);
            this.txtDateTo.TabIndex = 8;
            // 
            // txtTextSearch
            // 
            this.txtTextSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTextSearch.Location = new System.Drawing.Point(3, 77);
            this.txtTextSearch.Name = "txtTextSearch";
            this.txtTextSearch.Size = new System.Drawing.Size(661, 23);
            this.txtTextSearch.TabIndex = 10;
            this.toolTip1.SetToolTip(this.txtTextSearch, "Enclose in slashes to enable regex search:  /regex/");
            this.txtTextSearch.TextChanged += new System.EventHandler(this.txtTextSearch_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 57);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.label4.Size = new System.Drawing.Size(56, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Message:";
            // 
            // txtLogger
            // 
            this.txtLogger.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogger.Location = new System.Drawing.Point(3, 127);
            this.txtLogger.Name = "txtLogger";
            this.txtLogger.Size = new System.Drawing.Size(661, 23);
            this.txtLogger.TabIndex = 13;
            this.toolTip1.SetToolTip(this.txtLogger, "Separate multiple values with ; and exclude with !\r\nExamples:\r\nShow only A and B -" +
        "-> A;B\r\nShow all but A and B --> !A;B");
            // 
            // lblLoggerName
            // 
            this.lblLoggerName.AutoSize = true;
            this.lblLoggerName.Location = new System.Drawing.Point(3, 103);
            this.lblLoggerName.Name = "lblLoggerName";
            this.lblLoggerName.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblLoggerName.Size = new System.Drawing.Size(52, 21);
            this.lblLoggerName.TabIndex = 12;
            this.lblLoggerName.Text = "Loggers:";
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 150;
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 150;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 30;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Help";
            // 
            // txtModule
            // 
            this.txtModule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModule.Location = new System.Drawing.Point(3, 327);
            this.txtModule.Name = "txtModule";
            this.txtModule.Size = new System.Drawing.Size(661, 23);
            this.txtModule.TabIndex = 22;
            this.toolTip1.SetToolTip(this.txtModule, "Separate multiple values with ; and exlude with !\r\nExamples:\r\nShow only A and B -" +
        "-> A;B\r\nShow all but A and B --> !A;B");
            // 
            // txtFilterName
            // 
            this.txtFilterName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtFilterName.Location = new System.Drawing.Point(5, 25);
            this.txtFilterName.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.txtFilterName.Name = "txtFilterName";
            this.txtFilterName.Size = new System.Drawing.Size(660, 23);
            this.txtFilterName.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 253);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label3.Size = new System.Drawing.Size(88, 21);
            this.label3.TabIndex = 7;
            this.label3.Text = "Date/Time (To):";
            // 
            // txtThread
            // 
            this.txtThread.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtThread.Location = new System.Drawing.Point(3, 177);
            this.txtThread.Name = "txtThread";
            this.txtThread.Size = new System.Drawing.Size(661, 23);
            this.txtThread.TabIndex = 18;
            this.toolTip1.SetToolTip(this.txtThread, "Separate multiple values with ; and exclude with !\r\nExamples:\r\nShow only A and B -" +
        "-> A;B\r\nShow all but A and B --> !A;B");

            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 153);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label7.Size = new System.Drawing.Size(46, 21);
            this.label7.TabIndex = 17;
            this.label7.Text = "Thread:";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // logLevelsTableLayout
            // 
            this.logLevelsTableLayout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logLevelsTableLayout.AutoSize = true;
            this.logLevelsTableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.logLevelsTableLayout.ColumnCount = 4;
            this.logLevelsTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.logLevelsTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.logLevelsTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.logLevelsTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.logLevelsTableLayout.Controls.Add(this.rbDebug, 0, 0);
            this.logLevelsTableLayout.Controls.Add(this.rbInfo, 1, 0);
            this.logLevelsTableLayout.Controls.Add(this.rbWarning, 2, 0);
            this.logLevelsTableLayout.Controls.Add(this.rbError, 3, 0);
            this.logLevelsTableLayout.Location = new System.Drawing.Point(3, 27);
            this.logLevelsTableLayout.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.logLevelsTableLayout.Name = "logLevelsTableLayout";
            this.logLevelsTableLayout.RowCount = 1;
            this.logLevelsTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.logLevelsTableLayout.Size = new System.Drawing.Size(661, 30);
            this.logLevelsTableLayout.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 303);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label5.Size = new System.Drawing.Size(51, 21);
            this.label5.TabIndex = 21;
            this.label5.Text = "Module:";
            // 
            // criteriaTableLayout
            // 
            this.criteriaTableLayout.ColumnCount = 1;
            this.criteriaTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.criteriaTableLayout.Controls.Add(this.label4, 0, 2);
            this.criteriaTableLayout.Controls.Add(this.txtTextSearch, 0, 3);
            this.criteriaTableLayout.Controls.Add(this.txtModule, 0, 13);
            this.criteriaTableLayout.Controls.Add(this.label5, 0, 12);
            this.criteriaTableLayout.Controls.Add(this.txtDateFrom, 0, 9);
            this.criteriaTableLayout.Controls.Add(this.txtDateTo, 0, 11);
            this.criteriaTableLayout.Controls.Add(this.txtThread, 0, 7);
            this.criteriaTableLayout.Controls.Add(this.lblLoggerName, 0, 4);
            this.criteriaTableLayout.Controls.Add(this.txtLogger, 0, 5);
            this.criteriaTableLayout.Controls.Add(this.label3, 0, 10);
            this.criteriaTableLayout.Controls.Add(this.label7, 0, 6);
            this.criteriaTableLayout.Controls.Add(this.label2, 0, 8);
            this.criteriaTableLayout.Controls.Add(this.label1, 0, 1);
            this.criteriaTableLayout.Controls.Add(this.logLevelsTableLayout, 0, 1);
            this.criteriaTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.criteriaTableLayout.Location = new System.Drawing.Point(0, 81);
            this.criteriaTableLayout.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.criteriaTableLayout.Name = "criteriaTableLayout";
            this.criteriaTableLayout.Padding = new System.Windows.Forms.Padding(0, 12, 3, 0);
            this.criteriaTableLayout.RowCount = 16;
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.criteriaTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.criteriaTableLayout.Size = new System.Drawing.Size(670, 527);
            this.criteriaTableLayout.TabIndex = 23;
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnApplyFilter,
            this.btnSaveFilter,
            this.btnEditFilter,
            this.btnDeleteFilter,
            this.toolStripSeparator1,
            this.btnClearFilter});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(670, 23);
            this.toolStrip.TabIndex = 24;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnApplyFilter
            // 
            this.btnApplyFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnApplyFilter.Image")));
            this.btnApplyFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApplyFilter.Name = "btnApplyFilter";
            this.btnApplyFilter.Size = new System.Drawing.Size(87, 20);
            this.btnApplyFilter.Text = "Apply Filter";
            this.btnApplyFilter.Click += new System.EventHandler(this.btnApplyFilter_Click);
            // 
            // btnSaveFilter
            // 
            this.btnSaveFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveFilter.Image")));
            this.btnSaveFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveFilter.Name = "btnSaveFilter";
            this.btnSaveFilter.Size = new System.Drawing.Size(80, 20);
            this.btnSaveFilter.Text = "Save Filter";
            this.btnSaveFilter.Click += new System.EventHandler(this.btnSaveFilter_Click);
            // 
            // btnEditFilter
            // 
            this.btnEditFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnEditFilter.Image")));
            this.btnEditFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditFilter.Name = "btnEditFilter";
            this.btnEditFilter.Size = new System.Drawing.Size(94, 20);
            this.btnEditFilter.Text = "Edit Filter...";
            // 
            // btnDeleteFilter
            // 
            this.btnDeleteFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteFilter.Image")));
            this.btnDeleteFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteFilter.Name = "btnDeleteFilter";
            this.btnDeleteFilter.Size = new System.Drawing.Size(23, 20);
            this.btnDeleteFilter.Text = "Delete Filter";
            this.btnDeleteFilter.Click += new System.EventHandler(this.btnDeleteFilter_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(39, 19);
            this.btnClearFilter.Text = "Reset";
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // filterNamePanel
            // 
            this.filterNamePanel.AutoSize = true;
            this.filterNamePanel.BackColor = System.Drawing.Color.AliceBlue;
            this.filterNamePanel.Controls.Add(this.txtFilterName);
            this.filterNamePanel.Controls.Add(this.label6);
            this.filterNamePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterNamePanel.Location = new System.Drawing.Point(0, 23);
            this.filterNamePanel.Name = "filterNamePanel";
            this.filterNamePanel.Padding = new System.Windows.Forms.Padding(5, 5, 5, 10);
            this.filterNamePanel.Size = new System.Drawing.Size(670, 58);
            this.filterNamePanel.TabIndex = 25;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(5, 5);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(0, 2, 0, 3);
            this.label6.Size = new System.Drawing.Size(71, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "Filter Name:";
            // 
            // FilterPanel
            // 
            this.AutoHidePortion = 265D;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 608);
            this.Controls.Add(this.criteriaTableLayout);
            this.Controls.Add(this.filterNamePanel);
            this.Controls.Add(this.toolStrip);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "FilterPanel";
            this.Text = "Filter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FilterPanel_FormClosed);
            this.Load += new System.EventHandler(this.FilterPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.logLevelsTableLayout.ResumeLayout(false);
            this.logLevelsTableLayout.PerformLayout();
            this.criteriaTableLayout.ResumeLayout(false);
            this.criteriaTableLayout.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.filterNamePanel.ResumeLayout(false);
            this.filterNamePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbDebug;
        private System.Windows.Forms.RadioButton rbInfo;
        private System.Windows.Forms.RadioButton rbWarning;
        private System.Windows.Forms.RadioButton rbError;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDateFrom;
        private System.Windows.Forms.TextBox txtDateTo;
        private System.Windows.Forms.TextBox txtTextSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLogger;
        private System.Windows.Forms.Label lblLoggerName;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtThread;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.TableLayoutPanel logLevelsTableLayout;
        private System.Windows.Forms.TextBox txtModule;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel criteriaTableLayout;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnApplyFilter;
        private System.Windows.Forms.ToolStripButton btnClearFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnSaveFilter;
        private System.Windows.Forms.Panel filterNamePanel;
        private System.Windows.Forms.TextBox txtFilterName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripDropDownButton btnEditFilter;
        private System.Windows.Forms.ToolStripButton btnDeleteFilter;
    }
}