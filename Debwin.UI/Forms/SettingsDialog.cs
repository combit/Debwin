using Debwin.UI.Util;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Debwin.UI.Forms
{
    public partial class SettingsDialog : Form
    {

        private readonly IUserPreferences _settings;

        public SettingsDialog(IUserPreferences settings)
        {
            InitializeComponent();
            _settings = settings;

            txtTimeFormat.Text = settings.TimeFormat;
            txtDateTimeFormat.Text = settings.DateTimeFormat;
            txtTextEditor.Text = settings.EditorPath;
            txtLogFilePath.Text = String.IsNullOrEmpty(settings.LogFilePath) ? Path.GetTempPath() : settings.LogFilePath;
            chkAutoScrollOnScrollToEnd.Checked = _settings.EnableAutoScrollOnScrollToEnd;
            chkDisableAutoScrollOnSelection.Checked = _settings.DisableAutoScrollOnSelection;
            chkInstallGlobalKeyboardHook.Checked = _settings.InstallKeyboardHook;
            chkIgnoreLogIndentation.Checked = _settings.IgnoreLogIndentation;
            chkEnableAutoscrollOnClear.Checked = _settings.EnableAutoScrollOnClearLog;
            chkLimitMessageCount.Checked = _settings.MaximumMessageCount!= -1;
            trackbarChangeRecursion = true;  // disable the auto value adjustment
            trackMaxMessageCount.Value = (_settings.MaximumMessageCount != -1 ? _settings.MaximumMessageCount : trackMaxMessageCount.Value);
            trackbarChangeRecursion = false; 
            UpdateMaxMessagesControls();

            // Check if autostart is active
            bool autostartEnabled = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Debwin", null) != null;
            chkEnableAutostart.Checked = autostartEnabled;
        }


        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkEnableAutostart.Checked)
                {
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Debwin", $"\"{Assembly.GetEntryAssembly().Location}\"", RegistryValueKind.String);
                }
                else
                {
                    var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                    if (key.GetValueNames().Contains("Debwin"))
                    {
                        key.DeleteValue("Debwin");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not apply the auto-start option ({ex.Message})");
            }

            _settings.TimeFormat = txtTimeFormat.Text;
            _settings.DateTimeFormat = txtDateTimeFormat.Text;
            _settings.EditorPath = txtTextEditor.Text;
            _settings.LogFilePath = txtLogFilePath.Text;
            _settings.EnableAutoScrollOnScrollToEnd = chkAutoScrollOnScrollToEnd.Checked;
            _settings.DisableAutoScrollOnSelection = chkDisableAutoScrollOnSelection.Checked;
            _settings.InstallKeyboardHook = chkInstallGlobalKeyboardHook.Checked;
            _settings.EnableAutoScrollOnClearLog = chkEnableAutoscrollOnClear.Checked;
            _settings.IgnoreLogIndentation = chkIgnoreLogIndentation.Checked;

            int newMaxMessageCount = chkLimitMessageCount.Checked ? trackMaxMessageCount.Value : -1;
            if (_settings.MaximumMessageCount != newMaxMessageCount)
                MessageBox.Show("Please note that the changed maximum message count does not apply to logs that were already opened/active.", "Debwin4", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _settings.MaximumMessageCount = newMaxMessageCount;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnChooseEditor_Click(object sender, EventArgs e)
        {
            if (chooseEditorDialog.ShowDialog() == DialogResult.OK)
            {
                txtTextEditor.Text = chooseEditorDialog.FileName;
            }
        }

        private void trackMaxMessageCount_Scroll(object sender, EventArgs e)
        {
            UpdateMaxMessagesControls();
        }


        private bool trackbarChangeRecursion = false;

        private void trackMaxMessageCount_ValueChanged(object sender, EventArgs e)
        {
            if (trackbarChangeRecursion)
                return;

            int value = trackMaxMessageCount.Value;
            if (value % trackMaxMessageCount.SmallChange != 0)
            {
                value = (value / trackMaxMessageCount.SmallChange) * trackMaxMessageCount.SmallChange;
                trackbarChangeRecursion = true;
                trackMaxMessageCount.Value = Math.Max(trackMaxMessageCount.Minimum, value);
                UpdateMaxMessagesControls();
                trackbarChangeRecursion = false;
            }
        }

        private void UpdateMaxMessagesControls()
        {
            trackMaxMessageCount.Enabled = chkLimitMessageCount.Checked;
            lblMaxMessageCount.Visible = lblEstimatedMemoryUsage.Visible = trackMaxMessageCount.Enabled;
            lblMaxMessageCount.Text = String.Format(lblMaxMessageCount.Tag.ToString(), trackMaxMessageCount.Value);
            string estimMemory = Math.Round((trackMaxMessageCount.Value / 1000d) * 0.38961038961038) + " MB";
            lblEstimatedMemoryUsage.Text = String.Format(Thread.CurrentThread.CurrentUICulture, lblEstimatedMemoryUsage.Tag.ToString(), estimMemory);
        }

        private void chkLimitMessageCount_CheckedChanged(object sender, EventArgs e)
        {
            UpdateMaxMessagesControls();
        }

        private void btnChooseDirectory_Click(object sender, EventArgs e)
        {
            chooseLogFilePathDialog.SelectedPath = txtLogFilePath.Text;
            if (chooseLogFilePathDialog.ShowDialog() == DialogResult.OK)
            {
                txtLogFilePath.Text = chooseLogFilePathDialog.SelectedPath;
            }

        }
    }
}
