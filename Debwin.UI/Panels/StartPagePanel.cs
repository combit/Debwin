using combit.DebwinExtensions.MessageSources;
using combit.DebwinExtensions.Parsers;
using Debwin.Core;
using Debwin.Core.Controller;
using Debwin.Core.MessageSources;
using Debwin.Core.Parsers;
using Debwin.Core.Views;
using Debwin.UI.Forms;
using Debwin.UI.Util;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Debwin.UI.Panels
{
    public partial class StartPagePanel : DockContent
    {
        private const string WIN32_WINDOWCLASS_STARTPAGE = "DEBWIN4_STARTPAGE";
        private const int WM_USER = 0x0400;
        private const int WM_USER_DEBWIN4_LOGCRM = WM_USER + 1;

        private readonly IMainWindow _mainWindow;
        private readonly IDebwinController _debwinController;
        private readonly IUserPreferences _userPreferences;
        private CustomWin32Window _externalMessageListener;
        private static bool _hasProcessedCommandLineArgs;

        public StartPagePanel(IMainWindow mainWindow, IDebwinController debwinController, IUserPreferences userPreferences)
        {
            _debwinController = debwinController;
            _mainWindow = mainWindow;
            _userPreferences = userPreferences;

            InitializeComponent();
            try
            {
                _externalMessageListener = new CustomWin32Window("DEBWIN4_STARTPAGE", ExternalMessageListener);
            }
            catch (InvalidOperationException) { }

            if (IsElevatedUser())   // Drag/Drop is not available for elevated processes!
            {
                btnOpenLogFile.Text += "\n(Drag & Drop of Log Files is disabled - not available for elevated process)";
            }
            else
            {
                btnOpenLogFile.Text += "\n(or Drag & Drop Log File here)";
            }
        }

        /// <summary>Handles messages that the panel receives from other applications, e.g. commands from the cRM.</summary>
        private IntPtr ExternalMessageListener(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WM_USER_DEBWIN4_LOGCRM)    // Sent by the cRM to start logging
            {

                // LL (stand-alone or within the RS) and the cRM use the same port as they all use the same mem-mapped file to publish the port  -> only one can be active
                foreach (ILogController logControl in _debwinController.GetLogControllers())
                {
                    bool hasEvilMessageSource = logControl.GetMessageCollectors().Any(m => m.Source is CombitUdpMessageSource && !m.IsStopped);

                    if (!hasEvilMessageSource)
                        continue;

                    // So stop all active logs on the same port
                    foreach (var messageSource in logControl.GetMessageCollectors())
                    {
                        messageSource.Stop();
                    }

                    // And add a warning that log has been redirected.
                    // #34020: But only if the cRM-Tag is not still active.
                    if (logControl.Name != "cRM")
                    {
                        foreach (var logView in logControl.GetLogViews())
                        {
                            logView.AppendMessage(new LogMessage("[DEBWIN4] This log has been disabled because the cRM logging was started on the same UDP port. The log is continued in the cRM tab.")
                            {
                                Level = LogLevel.UserComment,
                                Thread = "[DEBWIN4]",
                                LoggerName = "[DEBWIN4]",
                                Timestamp = DateTime.Now
                            });
                        }
                    }
                }

                // Open the cRM if not yet open
                if (!_debwinController.GetLogControllers().Any(c => c.Name == "cRM"))
                {
                    btnStartCrmLog_Click(this, new EventArgs());
                }
                else
                {
                    // or ensure the capturing is active in the existing cRM tab
                    foreach (var msgCollector in _debwinController.GetLogControllers().First(c => c.Name == "cRM").GetMessageCollectors())
                    {
                        msgCollector.Start();
                    }
                }

            }
            return IntPtr.Zero;
        }

        private void btnOpenLogFile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                btnOpenLogFile.BackColor = Color.Azure;
            }
        }

        private void btnOpenLogFile_DragDrop(object sender, DragEventArgs e)
        {
            btnOpenLogFile.BackColor = SystemColors.ControlLight;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var filePath in files)
            {
                TryOpenFile(_mainWindow, filePath, false);
            }
        }

        /// <summary>
        /// Shows the Open-Log dialog for the given file path and returns the created log controller or null, if the operation was canceled.
        /// </summary>
        /// <param name="deleteFileAfterLoading"><see cref="FileMessageSource.DeleteFileOnClose"/></param>
        /// <returns></returns>
        public static ILogController TryOpenFile(IMainWindow mainWindow, string defaultFilePath, bool deleteFileAfterLoading)
        {
            var newFileSourceDialog = new CreateFileSourceDialog(defaultFilePath);
            if (mainWindow.ShowDialogEx(newFileSourceDialog) == DialogResult.OK)
            {
                FileMessageSource fileMessageSource = newFileSourceDialog.Result.Item1;
                fileMessageSource.DeleteFileOnClose = deleteFileAfterLoading;
                IMessageParser parser = newFileSourceDialog.Result.Item2;
                ILogController controller = DebwinController.GetNewLogController(mainWindow.GetDebwinController(), fileMessageSource.GetName(), fileMessageSource, parser);
                var rootView = new MemoryBasedLogView(-1);
                controller.AddView(rootView);
                mainWindow.OpenNewLogView(controller, rootView, new LogViewPanelOptions() { LiveCaptureMode = false });
                controller.GetMessageCollectors().First().Start();
                return controller;
            }

            return null;
        }

        private void btnOpenLogFile_DragLeave(object sender, EventArgs e)
        {
            btnOpenLogFile.BackColor = SystemColors.ControlLight;
        }

        private bool IsElevatedUser()
        {
            return WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
        }

        private void StartPagePanel_Load(object sender, EventArgs e)
        {
            if (Registry.ClassesRoot.OpenSubKey("cRM.Application") == null)
            {
                string exeName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "cRM.exe");
                if (!File.Exists(exeName))
                {
                    tableLayoutPanel1.Controls.Remove(this.btnStartCrmLog);
                    tableLayoutPanel1.ColumnCount = 2;
                }
            }
            if (!_hasProcessedCommandLineArgs)
            {
                _hasProcessedCommandLineArgs = true;

                if (Environment.CommandLine.Contains("/listlabel"))
                {
                    btnStartLLLog_Click(null, null);
                }
                else if (Environment.CommandLine.Contains("/reportserver"))
                {
                    btnStartRSLog_Click(null, null);
                }
                else
                {
                    string[] cmdArgs = Environment.GetCommandLineArgs();
                    for (int i = 1; i < cmdArgs.Length; i++)
                    {
                        if (File.Exists(cmdArgs[i]))
                        {
                            TryOpenFile(_mainWindow, cmdArgs[i], false);
                        }
                    }
                }
            }
        }

        private void btnStartLLLog_Click(object sender, EventArgs e)
        {
            var defaultColumns = GetDefaultListLabelLogColumns();
            var logController = DebwinController.GetNewLogController(_debwinController, "List & Label", new CombitUdpMessageSource() { Port = 9174 }, new ListLabelBinaryMessageParser());
            var rootView = new MemoryBasedLogView(_userPreferences.MaximumMessageCount);
            logController.AddView(rootView);
            rootView.EnableLogToFileOnOverflow(logController, Path.Combine(_userPreferences.LogFilePath, "ListLabel.log4"));
            _mainWindow.OpenNewLogView(logController, rootView, new LogViewPanelOptions() { DefaultColumns = defaultColumns, LiveCaptureMode = true });
            logController.GetMessageCollectors().First().Start();
        }

        private LogViewPanelColumnSet GetDefaultListLabelLogColumns()
        {
            var columns = new List<LogViewPanelColumnDefinition>()
                {
                    new LogViewPanelColumnDefinition(PropertyIdentifiers.PROPERTY_LEVEL, 25),
                    new LogViewPanelColumnDefinition(PropertyIdentifiers.PROPERTY_MODULE_NAME, 80),
                    new LogViewPanelColumnDefinition(PropertyIdentifiers.PROPERTY_LOGGER_NAME, 120),
                    new LogViewPanelColumnDefinition(PropertyIdentifiers.PROPERTY_THREAD, 60),
                    new LogViewPanelColumnDefinition(PropertyIdentifiers.PROPERTY_TIMESTAMP, 170),
                    new LogViewPanelColumnDefinition(PropertyIdentifiers.PROPERTY_MESSAGE, 350)
                };

            return new LogViewPanelColumnSet() { Columns = columns };
        }

        private void btnStartRSLog_Click(object sender, EventArgs e)
        {
            var defaultColumns = new LogViewPanelColumnSet()
            {
                Columns = new List<LogViewPanelColumnDefinition>()
                {
                    new LogViewPanelColumnDefinition(PropertyIdentifiers.PROPERTY_LEVEL, 25),
                    new LogViewPanelColumnDefinition(PropertyIdentifiers.PROPERTY_USER_PRINCIPAL, 150),
                    new LogViewPanelColumnDefinition(PropertyIdentifiers.PROPERTY_LOGGER_NAME, 120),
                    new LogViewPanelColumnDefinition(PropertyIdentifiers.PROPERTY_THREAD, 60),
                    new LogViewPanelColumnDefinition(PropertyIdentifiers.PROPERTY_TIMESTAMP, 170),
                    new LogViewPanelColumnDefinition(PropertyIdentifiers.PROPERTY_MESSAGE, 350)
                }
            };
            var logController = DebwinController.GetNewLogController(_debwinController, "Report Server", new UdpMessageSource() { Port = 4791 }, new Debwin4CsvParser() { CheckForMissingVersionHeader = false, LogFormatVersion = 1 });
            var rootView = new MemoryBasedLogView(_userPreferences.MaximumMessageCount);
            logController.AddView(rootView);

            // Source+Parser for NLog messages from Report Server:
            _mainWindow.OpenNewLogView(logController, rootView, new LogViewPanelOptions() { DefaultColumns = defaultColumns, LiveCaptureMode = true });
            logController.GetMessageCollectors().First().Start();

            // Add additional Source+Parser for default debugging of LL (for LL jobs without a .NET logger)
            var nativeMessageCollector = new DefaultMessageCollector() { Source = new CombitUdpMessageSource() { Port = 9174 }, Parser = new ListLabelBinaryMessageParser() };
            logController.AddMessageCollector(nativeMessageCollector);
            nativeMessageCollector.Start();
        }

        private void btnStartCrmLog_Click(object sender, EventArgs e)
        {
            var defaultColumns = GetDefaultListLabelLogColumns();
            var logController = new LogController() { Name = "cRM" };
            _debwinController.AddLogController(logController);

            // randomize the port in order to support terminal server environments
            int port = UdpMessageSource.GetAvailablePort();

            // cRM uses a specialized message collector which handles the cRM-specific control messages
            var messageCollector = new CRMMessageCollector() { Source = new CombitUdpMessageSource() { Port = port }, Parser = new ListLabelBinaryMessageParser(), LogFilePath = _userPreferences.LogFilePath, EnableLongTermMonitoring = _userPreferences.EnableLongTermMonitoring };
            var rootView = new MemoryBasedLogView(_userPreferences.MaximumMessageCount);
            logController.AddView(rootView);
            logController.AddMessageCollector(messageCollector);

            _mainWindow.OpenNewLogView(logController, rootView, new LogViewPanelOptions() { DefaultColumns = defaultColumns, LiveCaptureMode = true });
            messageCollector.Start();
        }

        private void btnOpenLogFile_Click(object sender, EventArgs e)
        {
            TryOpenFile(_mainWindow, null, false);
        }

        private void StartPagePanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V   // Paste Shortcut
               || e.Shift && e.KeyCode == Keys.Insert)
            {
                OpenLogFromClipboard(_mainWindow);
            }
        }

        public static void OpenLogFromClipboard(IMainWindow mainWindow)
        {
            string content = Clipboard.GetText(TextDataFormat.UnicodeText);
            if (String.IsNullOrEmpty(content))
            {
                MessageBox.Show("Clipboard is empty", "Debwin4", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var tempFile = Path.ChangeExtension(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()), ".log4");
            File.WriteAllText(tempFile, content);
            var controller = TryOpenFile(mainWindow, tempFile, true);
            if (controller != null)
                controller.Name = "Clipboard";

        }

        private void btnOpenFromClipboard_Click(object sender, EventArgs e)
        {
            OpenLogFromClipboard(_mainWindow);
        }

        private void StartPagePanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_externalMessageListener != null)
            {
                _externalMessageListener.Dispose();
                _externalMessageListener = null;
            }
        }
    }
}
