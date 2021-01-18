using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Debwin.UI.Panels
{
    public partial class LogStructurePanel : DockContent
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName,
                                            string pszSubIdList);


        public LogStructurePanel()
        {
            InitializeComponent();
            SetWindowTheme(treeView1.Handle, "explorer", null);
            this.treeView1.ExpandAll();
        }
    }
}
