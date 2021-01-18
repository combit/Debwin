using Debwin.Core;
using Debwin.Core.MessageSources;
using Debwin.UI.Properties;
using System;
using System.Windows.Forms;

namespace Debwin.UI.Controls
{
    public partial class MessageCollectorView : UserControl
    {
        private const uint PBM_SETSTATE = 1040;

        private IMessageCollector _messageCollector;

        public MessageCollectorView(IMessageCollector messageCollector)
        {
            InitializeComponent();

            _messageCollector = messageCollector;
            _messageCollector.Source.ProgressChanged += MessageSource_ProgressChanged;
            lblSourceName.Text = _messageCollector.Source.GetName();
            lblStatus.Text = "Parser: " + _messageCollector.Parser.GetType().Name;
            btnStartStop.Image = messageCollector.IsStopped ?  Resources.mm_Play : Resources.mm_Stop_Red;
        
            this.Disposed += MessageSourceView_Disposed;
        }


        public IMessageCollector MessageCollector { get { return _messageCollector; } }

        private void MessageSourceView_Disposed(object sender, EventArgs e)
        {
            if (_messageCollector != null)
            {
                _messageCollector.Source.ProgressChanged -= MessageSource_ProgressChanged;
            }
        }

        private void MessageSource_ProgressChanged(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated)
                return;

            progressBar1.Invoke(new Action(() =>
            {
                UpdateUIToMessageSourceStatus();
            }));
        }

        private void UpdateUIToMessageSourceStatus()
        {
            btnStartStop.Image = _messageCollector.IsStopped ? Resources.mm_Play : Resources.mm_Stop_Red;

            if (_messageCollector.Source.CurrentProgress == Constants.MESSAGESOURCE_PROGRESS_MARQUEE)
            {
                progressBar1.Style = ProgressBarStyle.Marquee;
            }
            else if (_messageCollector.Source.CurrentProgress == Constants.MESSAGESOURCE_PROGRESS_ERROR)
            {
                progressBar1.Value = progressBar1.Maximum;
                progressBar1.Style = ProgressBarStyle.Continuous;
                NativeMethods.SendMessage(progressBar1.Handle, PBM_SETSTATE, (IntPtr)2 /*2=error, red color*/, IntPtr.Zero);  // red color
            }
            else
            {
                progressBar1.Style = ProgressBarStyle.Continuous;
                progressBar1.Value = _messageCollector.Source.CurrentProgress;
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (_messageCollector == null)
                return;

            if (_messageCollector.IsStopped)
            {
                _messageCollector.Start();
            }
            else
            {
                _messageCollector.Stop();
            }
        }

        private void MessageCollectorView_Load(object sender, EventArgs e)
        {
            UpdateUIToMessageSourceStatus();
        }
    }
}
