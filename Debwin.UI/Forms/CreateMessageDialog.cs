using System;
using System.Windows.Forms;

namespace Debwin.UI.Forms
{
    public partial class CreateMessageDialog : Form
    {
        public CreateMessageDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Message = txtMessage.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string Message { get; private set; }
    }
}
