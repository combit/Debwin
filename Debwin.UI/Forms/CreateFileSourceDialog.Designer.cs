namespace Debwin.UI.Forms
{
    partial class CreateFileSourceDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.rbUnicode = new System.Windows.Forms.RadioButton();
            this.rbUtf8 = new System.Windows.Forms.RadioButton();
            this.rbAscii = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.rbDefaultEncoding = new System.Windows.Forms.RadioButton();
            this.lstLogType = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelEncodingOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelEncodingOptions.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Log file path:";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtFilePath, 2);
            this.txtFilePath.Location = new System.Drawing.Point(3, 23);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(437, 23);
            this.txtFilePath.TabIndex = 1;
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseFile.Location = new System.Drawing.Point(353, 52);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(87, 28);
            this.btnBrowseFile.TabIndex = 2;
            this.btnBrowseFile.Text = "Browse...";
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 183);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 15, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Encoding:";
            // 
            // rbUnicode
            // 
            this.rbUnicode.AutoSize = true;
            this.rbUnicode.Location = new System.Drawing.Point(218, 3);
            this.rbUnicode.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.rbUnicode.Name = "rbUnicode";
            this.rbUnicode.Size = new System.Drawing.Size(118, 19);
            this.rbUnicode.TabIndex = 6;
            this.rbUnicode.Text = "Unicode (UTF-16)";
            this.rbUnicode.UseVisualStyleBackColor = true;
            // 
            // rbUtf8
            // 
            this.rbUtf8.AutoSize = true;
            this.rbUtf8.Location = new System.Drawing.Point(3, 3);
            this.rbUtf8.Name = "rbUtf8";
            this.rbUtf8.Size = new System.Drawing.Size(57, 19);
            this.rbUtf8.TabIndex = 7;
            this.rbUtf8.Text = "UTF-8";
            this.rbUtf8.UseVisualStyleBackColor = true;
            // 
            // rbAscii
            // 
            this.rbAscii.AutoSize = true;
            this.rbAscii.Location = new System.Drawing.Point(78, 3);
            this.rbAscii.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.rbAscii.Name = "rbAscii";
            this.rbAscii.Size = new System.Drawing.Size(53, 19);
            this.rbAscii.TabIndex = 8;
            this.rbAscii.Text = "ASCII";
            this.rbAscii.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 83);
            this.label4.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Log Format";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(152, 28);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.btnCancel.Size = new System.Drawing.Size(67, 25);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.AutoSize = true;
            this.btnApply.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnApply.Location = new System.Drawing.Point(83, 28);
            this.btnApply.Name = "btnApply";
            this.btnApply.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.btnApply.Size = new System.Drawing.Size(63, 25);
            this.btnApply.TabIndex = 13;
            this.btnApply.Text = "OK";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.AddExtension = false;
            this.openFileDialog1.Filter = "Log Files (*.log, *.txt, *.log4)|*.log;*.txt;*.log4|All Files (*.*)|*.*";
            this.openFileDialog1.Title = "Open Log File";
            // 
            // rbDefaultEncoding
            // 
            this.rbDefaultEncoding.AutoSize = true;
            this.rbDefaultEncoding.Checked = true;
            this.rbDefaultEncoding.Location = new System.Drawing.Point(149, 3);
            this.rbDefaultEncoding.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.rbDefaultEncoding.Name = "rbDefaultEncoding";
            this.rbDefaultEncoding.Size = new System.Drawing.Size(51, 19);
            this.rbDefaultEncoding.TabIndex = 14;
            this.rbDefaultEncoding.TabStop = true;
            this.rbDefaultEncoding.Text = "ANSI";
            this.rbDefaultEncoding.UseVisualStyleBackColor = true;
            // 
            // lstLogType
            // 
            this.lstLogType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.lstLogType, 2);
            this.lstLogType.FormattingEnabled = true;
            this.lstLogType.ItemHeight = 15;
            this.lstLogType.Items.AddRange(new object[] {
            "Unknown Format (displays the raw file content)",
            "Debwin4 CSV (.log4)",
            "List & Label Debug File (.log)",
            "cRM / Debwin3 Log File (.log)"});
            this.lstLogType.Location = new System.Drawing.Point(3, 101);
            this.lstLogType.Name = "lstLogType";
            this.lstLogType.Size = new System.Drawing.Size(437, 64);
            this.lstLogType.TabIndex = 15;
            this.lstLogType.SelectedIndexChanged += new System.EventHandler(this.lstLogType_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lstLogType, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtFilePath, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnBrowseFile, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panelEncodingOptions, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(443, 284);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // panelEncodingOptions
            // 
            this.panelEncodingOptions.AutoSize = true;
            this.panelEncodingOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.panelEncodingOptions, 2);
            this.panelEncodingOptions.Controls.Add(this.rbUtf8);
            this.panelEncodingOptions.Controls.Add(this.rbAscii);
            this.panelEncodingOptions.Controls.Add(this.rbDefaultEncoding);
            this.panelEncodingOptions.Controls.Add(this.rbUnicode);
            this.panelEncodingOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEncodingOptions.Location = new System.Drawing.Point(3, 201);
            this.panelEncodingOptions.Name = "panelEncodingOptions";
            this.panelEncodingOptions.Size = new System.Drawing.Size(437, 25);
            this.panelEncodingOptions.TabIndex = 16;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(this.btnApply);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(221, 229);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(222, 56);
            this.flowLayoutPanel1.TabIndex = 17;
            // 
            // CreateFileSourceDialog
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(453, 294);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateFileSourceDialog";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open Log File";
            this.Load += new System.EventHandler(this.CreateFileSourceDialog_Load);
            this.Shown += new System.EventHandler(this.NewFileListenerDialog_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panelEncodingOptions.ResumeLayout(false);
            this.panelEncodingOptions.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowseFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbUnicode;
        private System.Windows.Forms.RadioButton rbUtf8;
        private System.Windows.Forms.RadioButton rbAscii;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RadioButton rbDefaultEncoding;
        private System.Windows.Forms.ListBox lstLogType;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel panelEncodingOptions;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}