using Debwin.UI.Properties;
using System;
using System.Windows.Forms;

namespace Debwin.UI
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblDockingLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(Resources.License_DockingUI);
        }

        private void lbl_combit_URL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.combit.net");
        }

        private void AboutDialog_Load(object sender, EventArgs e)
        {
            label_versionnr.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
