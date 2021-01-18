namespace Debwin.UI.Panels
{
    partial class LogStructurePanel
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("BackgroundWorker");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("HealthMonitor");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Infrastructure", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Authentication");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Services", new System.Windows.Forms.TreeNode[] {
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("GUI");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Model");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Datasources", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("ReportServer", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode5,
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Dataproviders");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("SystemInfo");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Licensing");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("PrinterInfo");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("ListLabel", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode11,
            treeNode12,
            treeNode13});
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Root", new System.Windows.Forms.TreeNode[] {
            treeNode9,
            treeNode14});
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.CheckBoxes = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Node2";
            treeNode1.Text = "BackgroundWorker";
            treeNode2.Name = "Node3";
            treeNode2.Text = "HealthMonitor";
            treeNode3.Name = "Node1";
            treeNode3.Text = "Infrastructure";
            treeNode4.Name = "Node5";
            treeNode4.Text = "Authentication";
            treeNode5.Name = "Node4";
            treeNode5.Text = "Services";
            treeNode6.Name = "Node7";
            treeNode6.Text = "GUI";
            treeNode7.Name = "Node8";
            treeNode7.Text = "Model";
            treeNode8.Name = "Node6";
            treeNode8.Text = "Datasources";
            treeNode9.Name = "Node0";
            treeNode9.Text = "ReportServer";
            treeNode10.Name = "Node11";
            treeNode10.Text = "Dataproviders";
            treeNode11.Name = "Node12";
            treeNode11.Text = "SystemInfo";
            treeNode12.Name = "Node13";
            treeNode12.Text = "Licensing";
            treeNode13.Name = "Node14";
            treeNode13.Text = "PrinterInfo";
            treeNode14.Name = "Node10";
            treeNode14.Text = "ListLabel";
            treeNode15.Name = "Node9";
            treeNode15.Text = "Root";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode15});
            this.treeView1.Size = new System.Drawing.Size(311, 366);
            this.treeView1.TabIndex = 0;
            // 
            // LogStructurePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 366);
            this.Controls.Add(this.treeView1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "LogStructurePanel";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide;
            this.Text = "Log Structure";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
    }
}