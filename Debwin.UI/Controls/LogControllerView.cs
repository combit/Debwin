using System.Windows.Forms;
using Debwin.Core;
using Debwin.Core.MessageSources;
using System.Linq;

namespace Debwin.UI.Controls
{
    public partial class LogControllerView : UserControl
    {

        private readonly ILogController _logController;

        public ILogController ObservedLogController { get { return _logController; } }

        public LogControllerView(ILogController logController)
        {
            InitializeComponent();
            _logController = logController;

            logController.AddedMessageCollector += LogController_AddedMessageCollector;
            logController.RemovedMessageCollector += LogController_RemovedMessageCollector;
            logController.PropertyChanged += LogController_PropertyChanged;
            lblControllerName.Text = logController.Name;
            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.RowCount = 0;
        }

        
        private void DisposeInternal()
        {
            _logController.AddedMessageCollector -= LogController_AddedMessageCollector;
            _logController.RemovedMessageCollector -= LogController_RemovedMessageCollector;
            _logController.PropertyChanged -= LogController_PropertyChanged;
        }

        private void LogController_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           if (e.PropertyName == nameof(ILogController.Name))
            {
                lblControllerName.Text = _logController.Name;
            }
        }

        private void LogController_RemovedMessageCollector(object sender, AddOrRemoveMessageCollectorEventArgs e)
        {
            var controlToDelete = tableLayoutPanel.Controls.OfType<MessageCollectorView>().FirstOrDefault(ctl => ctl.MessageCollector == e.MessageCollector);
            if (controlToDelete == null)
                return;

            tableLayoutPanel.Controls.Remove(controlToDelete);
            controlToDelete.Dispose();
        }

        private void LogController_AddedMessageCollector(object sender, AddOrRemoveMessageCollectorEventArgs e)
        {
            AddMessageCollectorControls(e.MessageCollector);
        }

        private void AddMessageCollectorControls(IMessageCollector messageCollector)
        {
            var messageCollectorView = new MessageCollectorView(messageCollector);

            if (tableLayoutPanel.RowCount < tableLayoutPanel.Controls.Count + 1)
            {
                tableLayoutPanel.RowCount++;
                tableLayoutPanel.RowStyles.Add(new RowStyle() { SizeType = SizeType.AutoSize});
            }

            tableLayoutPanel.Controls.Add(messageCollectorView);
            messageCollectorView.Dock = DockStyle.Top;
           
            this.Parent.PerformLayout();
        }

        private void panel1_Click(object sender, System.EventArgs e)
        {

        }
    }
}
