using combit.DebwinExtensions.Parsers;
using Debwin.Core.Parsers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Debwin.UI.Forms
{
    public partial class CreateUdpSourceDialog : Form
    {
        public CreateUdpSourceDialog()
        {
            InitializeComponent();
            cbMessageParser.SelectedIndex = 1;
            rbUtf8.Checked = true;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Port = (int)txtPort.Value;

            if (rbUnicode.Checked)
                Encoding = Encoding.Unicode;
            else if (rbUtf8.Checked)
                Encoding = Encoding.UTF8;
            else if (rbAscii.Checked)
                Encoding = Encoding.ASCII;
            else
                Encoding = Encoding.Default;

            if (cbMessageParser.SelectedIndex == 0)
            {
                MessageParser = new BinaryDummyMessageParser();

            }
            else if (cbMessageParser.SelectedIndex == 1)
            {
                MessageParser = new Log4jXmlLayoutParser();
            }

            else if (cbMessageParser.SelectedIndex == 2)
            {
                MessageParser = new ListLabelBinaryMessageParser();
            }
            else if (cbMessageParser.SelectedIndex == 3)
            {
                MessageParser = new Debwin4CsvParser() { CheckForMissingVersionHeader = false, LogFormatVersion = 1 };
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        public int Port { get; private set; }

        public Encoding Encoding { get; private set; }

        public bool WatchFile { get; private set; }

        public IMessageParser MessageParser { get; private set; }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
