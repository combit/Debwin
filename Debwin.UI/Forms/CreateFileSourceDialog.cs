using combit.DebwinExtensions.Parsers;
using Debwin.Core.MessageSources;
using Debwin.Core.Parsers;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Debwin.UI.Forms
{
    public partial class CreateFileSourceDialog : Form
    {

        private readonly string _initialFilePath;
        private Tuple<FileMessageSource, IMessageParser> _result;

        public CreateFileSourceDialog(string initialLogFilePath)
        {
            _initialFilePath = initialLogFilePath;
            InitializeComponent();
        }


        public Tuple<FileMessageSource, IMessageParser> CreateMessageSource()
        {
            panelEncodingOptions.Enabled = false;
            lstLogType.Enabled = false;

            Encoding encoding;
            string logFilePath = txtFilePath.Text;

            if (String.IsNullOrEmpty(logFilePath) || !File.Exists(logFilePath))
            {
                MessageBox.Show("No file specified or file not found.");
                return null;
            }

            if (rbUnicode.Checked)
                encoding = Encoding.Unicode;
            else if (rbUtf8.Checked)
                encoding = Encoding.UTF8;
            else if (rbAscii.Checked)
                encoding = Encoding.ASCII;
            else
                encoding = Encoding.Default;

            //bool watchFile = chkWatchFile.Checked;

            IMessageParser messageParser;

            if (lstLogType.SelectedIndex == 0)  // No Parsing
            {
                messageParser = new BasicStringMessageParser();
            }
            else if (lstLogType.SelectedIndex == 1)
            {
                messageParser = new Debwin4CsvParser() { CheckForMissingVersionHeader = false };   // Debwin4
            }
            else if (lstLogType.SelectedIndex == 2 || lstLogType.SelectedIndex == 3)   // Debwin3 (LL22+, legacy LL, cRM)
            {
                messageParser = new CombitLogFileMessageParser();
            }
            else
            {
                MessageBox.Show("Please select a log format.");
                panelEncodingOptions.Enabled = true;
                lstLogType.Enabled = true;
                return null;
            }

            return new Tuple<FileMessageSource, IMessageParser>(new FileMessageSource(logFilePath, encoding), messageParser);
        }

        public Tuple<FileMessageSource, IMessageParser> Result { get { return _result; } }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                SetSelectedFile(openFileDialog1.FileName);
            }
        }

        private void SetSelectedFile(string filePath)
        {
            lstLogType.Enabled = panelEncodingOptions.Enabled = true;

            txtFilePath.Text = filePath;
            if (TryDetectFileFormat(filePath))
            {
                btnApply.PerformClick();
            }
        }


        private bool TryDetectFileFormat(string filepath)
        {
            lstLogType.SelectedIndex = 0;

            if (Path.GetExtension(filepath) == ".log4")   // Debwin4 format is clear
            {
                lstLogType.SelectedIndex = 1;
                rbUtf8.PerformClick();

                return true;
            }
            else if (Path.GetFileName(filepath) == "Debwin.log")
            {
                rbUtf8.PerformClick();
            }

            // Sneak into file
            byte[] buffer = new byte[250];

            using (var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (fs.Length < buffer.Length)
                    return false;

                fs.Read(buffer, 0, buffer.Length);


                // GUESS ENCODING
                Encoding encoding;

                if (buffer != null && buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)   // UTF8 BOM, probably saved from Debwin3
                {
                    rbUtf8.PerformClick();
                    encoding = Encoding.UTF8;
                }
                else
                {
                    // try unicode
                    string sample = Encoding.Unicode.GetString(buffer).TrimStart('\uFEFF');  // \uFEFF = byte order mark
                    if (sample[0] > 127)  // we got some chinese char --> unicode was wrong, only ANSI remains
                    {
                        encoding = Encoding.Default;
                        rbDefaultEncoding.PerformClick();
                    }
                    else
                    {
                        encoding = Encoding.Unicode;   // If it`s not from Debwin3/Debwin4, it`s probably written by LL -> Unicode
                        rbUnicode.PerformClick();
                    }
                }

                // GUESS FORMAT

                string log = encoding.GetString(buffer).TrimStart('\uFEFF');  // \uFEFF = byte order mark

                // Remove welcome message of Debwin3
                const string debwin3Header = "Welcome to DEBWIN V. 3.7";
                if (log.StartsWith(debwin3Header))
                    log = log.Substring(debwin3Header.Length + 1).TrimStart();

                // a Debwin4 log with a custom file name (no .log4)?
                if (log.StartsWith("Debwin4::CSV::V1"))
                {
                    lstLogType.SelectedIndex = 1;
                    return true;
                }

                // cRM format?    14:57:27.400 00000B3C  >CLSearchServerPath('C:\combit\cRM9\')
                if (log[2] == ':' && log[5] == ':' && log[8] == '.' && log[12] == ' ')
                {
                    lstLogType.SelectedIndex = 3;
                    return true;
                }

                // cRM or has the CXUT22 / CMLL31 / ... format?
                if (Regex.IsMatch(log, @"^cRM.+:", RegexOptions.Singleline) || Regex.IsMatch(log, @"^C[MX][A-Z]{2}\d{2}.{2}:.*", RegexOptions.Singleline))
                {
                    lstLogType.SelectedIndex = 3;
                    return true;
                }
            }

            return false;
        }

        private void NewFileListenerDialog_Shown(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_initialFilePath) || !File.Exists(_initialFilePath))
            {
                btnBrowseFile.PerformClick();
            }
            else
            {
                SetSelectedFile(_initialFilePath);
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            _result = CreateMessageSource();
            if (_result != null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CreateFileSourceDialog_Load(object sender, EventArgs e)
        {
            lstLogType.SelectedIndex = 0;
        }


        private void lstLogType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!panelEncodingOptions.Enabled)
            //    return;

            //if (lstLogType.SelectedIndex == 1 || lstLogType.SelectedIndex == 3)   // cRM 8  / Debwin4 CSV  / Debwin3
            //{
            //    rbUtf8.Checked = true;
            //    panelEncodingOptions.Enabled = false;
            //}
            //else if (lstLogType.SelectedIndex == 2)    // List & Label
            //{
            //    rbUnicode.Checked = true;
            //    panelEncodingOptions.Enabled = false;
            //}
            //else
            //{
            //    panelEncodingOptions.Enabled = true;
            //}
        }
    }
}
