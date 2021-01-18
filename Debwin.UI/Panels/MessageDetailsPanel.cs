using Debwin.Core;
using Debwin.Core.LogWriters;
using Debwin.UI.Util;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Debwin.UI.Panels
{
    public partial class MessageDetailsPanel : DockContent
    {
        private readonly IMainWindow _mainWindow;
        private readonly IUserPreferences _userPreferences;
        private LogMessage _currentMessage;

        public MessageDetailsPanel(IUserPreferences userPreferences, IMainWindow mainWindow)
        {
            _userPreferences = userPreferences;
            _mainWindow = mainWindow;

            InitializeComponent();
        }

        public void ShowMessageDetails(LogMessage message)
        {
            if (message == null)
            {
                txtMessageText.Text = string.Empty;
                btnCopyMessage.Enabled = false;
                return;
            }

            _currentMessage = message;
            btnCopyMessage.Enabled = true;
            txtMessageText.Text = message.Message.TrimStart(' ', '\t');
        }

        private void btnCopyMessage_Click(object sender, EventArgs e)
        {
            if (_currentMessage == null)
                return;

            StringBuilder buffer = new StringBuilder();
            using (var sw = new StringWriter(buffer))
            {
                var logWriter = new Debwin4CsvLogWriter(sw);
                logWriter.WriteBeginLog();
                logWriter.WriteLogEntry(_currentMessage, CancellationToken.None);
                logWriter.WriteEndLog();
            }

            Clipboard.SetText(buffer.ToString());
        }

        private void chkWordWrap_CheckedChanged(object sender, EventArgs e)
        {
            txtMessageText.WordWrap = chkWordWrap.Checked;
        }

        private void MessageDetailsPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F && e.Control)
            {
                _mainWindow.GetActiveLogView()?.InvokeTextSearch();
                e.Handled = true;
            }
        }
    }
}
