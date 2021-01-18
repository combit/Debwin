using Debwin.Core;
using Debwin.Core.Controller;
using Debwin.UI.Controls;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Debwin.UI.Panels
{
    public partial class LogControllerPanel : DockContent
    {

        private readonly IDebwinController _debwinController;

        public LogControllerPanel(IDebwinController debwinController)
        {
            _debwinController = debwinController;
         
            InitializeComponent();

            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.RowCount = 0;
            foreach (var logController in _debwinController.GetLogControllers())
            {
                AddLogControllerControls(logController);
            }

            this.FormClosed += LogControllerPanel_FormClosed;
            _debwinController.AddedLogController += debwinController_AddedLogController;
            _debwinController.RemovedLogController += debwinController_RemovedLogController;
        }

        // Handle Add/Remove Log Controllers
        private void debwinController_RemovedLogController(object sender, AddRemoveLogControllerEventArgs e)
        {
            var controllerViewToDelete = tableLayoutPanel1.Controls.OfType<LogControllerView>().FirstOrDefault(c => c.ObservedLogController == e.LogController);
            if (controllerViewToDelete == null)
                return;

            tableLayoutPanel1.Controls.Remove(controllerViewToDelete);
            controllerViewToDelete.Dispose();
            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                this.tableLayoutPanel1.SetRow(tableLayoutPanel1.Controls[i], i);
            }
        }

        private void debwinController_AddedLogController(object sender, AddRemoveLogControllerEventArgs e)
        {
            AddLogControllerControls(e.LogController);
        }

        private void AddLogControllerControls(ILogController logController)
        {
            var controllerView = new LogControllerView(logController);
            if (this.tableLayoutPanel1.RowStyles.Count < tableLayoutPanel1.Controls.Count + 1)
            {
                this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
            this.tableLayoutPanel1.Controls.Add(controllerView);
            controllerView.Dock = DockStyle.Top;
            this.tableLayoutPanel1.SetRow(controllerView, tableLayoutPanel1.Controls.Count - 1);
        }

        private void LogControllerPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Controls.Clear();
            _debwinController.AddedLogController -= debwinController_AddedLogController;
            _debwinController.RemovedLogController -= debwinController_AddedLogController;
        }

    }
}
