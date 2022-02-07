using Debwin.Core;
using Debwin.Core.LogWriters;
using Debwin.Core.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Debwin.UI.Forms
{
    public partial class SaveLogDialog : Form
    {

        private readonly IQueryableLogView _logView;
        private readonly bool _saveToFile;
        private readonly IEnumerable<int> _selectedIndices;
        private readonly IEnumerable<LogMessage> _logMessages;
        private readonly IList<int> _propertiesToWrite;   // List of IDs of message properties that should be included (may be ignored for some file formats)
        private string _logFilePath;
        private CancellationTokenSource _cancelTokenSource;


        /// <param name="saveToFile">True to save a file, false to write to the clipboard</param>
        /// <param name="selectedIndices">Indices of the log messages to process. Null to process all messages.</param>
        /// <param name="propertiesToWrite">IDs of the message properties to write (ie.e columns in the UI). May be ignored for non-formatted log formats.</param>
        public SaveLogDialog(IQueryableLogView logView, bool saveToFile, IEnumerable<int> selectedIndices, IEnumerable<LogMessage> logMessages, IList<int> propertiesToWrite)
        {
            _logView = logView;
            _saveToFile = saveToFile;
            _selectedIndices = selectedIndices;
            _logMessages = logMessages;
            _propertiesToWrite = propertiesToWrite;
            InitializeComponent();
            this.ShowIcon = false;   // Set this manually and not in the designer (http://stackoverflow.com/a/21935941/3680727)
            
            // To prevent displaying the window for very short saves (< 250ms), first make the window invisible
            this.Opacity = 0;
        }

        public void SetFilter(string filter)
        {
            saveFileDialog1.Filter = filter;
        }

        private void SaveLogDialog_Load(object sender, EventArgs e)
        {
            if (_saveToFile && LogFilePath == null)  // Show file save dialog?
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    LogFilePath = saveFileDialog1.FileName;
                }
                else
                {
                    this.Close();
                    return;
                }
            }

            if (_saveToFile == false)
            {
                lblSaveTarget.Text = "Clipboard";
            }

            StartSave();
        }

        public string LogFilePath
        {
            get { return _logFilePath; }
            set
            {
                _logFilePath = value;
                lblSaveTarget.Text = value;
            }
        }

        private void StartSave()
        {
            // Create a copy of the log, so the original log is not locked during the whole save process
            // (can deadlock UI and writer thread!)
            _cancelTokenSource = new CancellationTokenSource();

            IQueryableLogView logClone = new MemoryBasedLogView(-1);   // do not register this at the controller, new messages are unwanted
            if (_logMessages != null)
            {
                _logView.CopyMessagesListTo(logClone, _logMessages);
            }
            else
            {
                _logView.CopyMessagesTo(logClone, _selectedIndices);
            }

            // Start a timer that will make this window (and it`s progress bar) visible if the save is not completed within 250ms
            showWindowTimer.Start();

            if (_saveToFile)
            {
                SaveLogToFile(logClone);
            }
            else
            {
                SaveLogToClipboard(logClone);
            }
        }


        private ILogWriter GetLogWriter(TextWriter writer)
        {
            if (LogFilePath != null && Path.GetExtension(LogFilePath) == ".txt")
            {
                if (saveFileDialog1.FilterIndex == 3)
                {
                    return new BasicFormattedLogWriter(writer, new List<int>(1) { PropertyIdentifiers.PROPERTY_MESSAGE });  // Plaintext file (messages only)
                }
                else
                {
                    return new BasicFormattedLogWriter(writer, _propertiesToWrite);    // Formatted text file (all columns)
                }
            }
            else   // Debwin4 CSV format
            {
                return new Debwin4CsvLogWriter(writer);
            }
        }

        private void SaveLogToClipboard(IQueryableLogView log)
        {
            var staTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Task.Factory.StartNew(() =>
            {
                StringBuilder buffer = new StringBuilder();
                using (var sw = new StringWriter(buffer))
                {
                    var logWriter = GetLogWriter(sw);
                    log.WriteTo(logWriter, _cancelTokenSource.Token, LogWriter_ProgressChanged);
                }
                return buffer.ToString();

            }, _cancelTokenSource.Token).ContinueWith((writerTask) =>
            {
                _cancelTokenSource.Dispose();
                _cancelTokenSource = null;

                this.Invoke(new Action(() =>
                {
                    if (writerTask.IsFaulted)
                    {
                        MessageBox.Show(this, "Error while writing log data:\n\n" + writerTask.Exception.ToString());
                        this.DialogResult = DialogResult.Abort;
                    }
                    else if (writerTask.IsCompleted && !writerTask.IsCanceled)
                    {
                        Clipboard.SetText(writerTask.Result, TextDataFormat.UnicodeText);  // requires to be executed in STA thread!
                        this.DialogResult = DialogResult.OK;
                    }
                    this.Close();
                }));
            });
        }



        private void SaveLogToFile(IQueryableLogView log)
        {
            Task.Factory.StartNew(() =>
            {
                using (var fs = new FileStream(LogFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    using (var writer = new StreamWriter(fs, Encoding.UTF8))
                    {
                        var logWriter = GetLogWriter(writer);
                        log.WriteTo(logWriter, _cancelTokenSource.Token, LogWriter_ProgressChanged);
                    }
                }
            }, _cancelTokenSource.Token).ContinueWith((writerTask) =>
            {
                _cancelTokenSource.Dispose();
                _cancelTokenSource = null;

                this.Invoke(new Action(() =>
                {
                    if (writerTask.IsFaulted)
                    {
                        MessageBox.Show(this, "Error while saving log file:\n\n" + writerTask.Exception.ToString());
                        this.DialogResult = DialogResult.Abort;
                    }
                    else if (writerTask.IsCompleted && !writerTask.IsCanceled)
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                    this.Close();
                }));
            });
        }


        private void LogWriter_ProgressChanged(int progress)
        {
            if (this.IsDisposed)
                return;

            this.Invoke(new Action(() =>
            {
                progressBar1.Value = progress;
            }));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_cancelTokenSource == null)
            {
                this.Close();
            }
            else
            {
                _cancelTokenSource.Cancel();
            }
        }

        private void SaveLogDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cancelTokenSource != null)
            {
                _cancelTokenSource.Cancel();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            StartSave();
        }

        // Makes this window visible if the save is not completed within 250ms
        private void showWindowTimer_Tick(object sender, EventArgs e)
        {
            this.Opacity = 1;
            showWindowTimer.Stop();
        }
    }
}
