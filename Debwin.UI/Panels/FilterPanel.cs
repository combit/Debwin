using Debwin.Core;
using Debwin.UI.Util;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Debwin.UI.Panels
{

    public partial class FilterPanel : DockContent
    {
        public event EventHandler<ApplyFilterEventArgs> RequestedFilter;

        private readonly IUserPreferences _userPreferences;
        private FilterDefinition _currentFilterDefinition;


        public FilterPanel(IUserPreferences userPreferences)
        {
            InitializeComponent();

            _userPreferences = userPreferences;
            _userPreferences.SavedFilters.ItemsChanged += SavedFilters_ItemsChanged;
            UpdateSavedFilterMenu();
            LoadFilterDefinition(new FilterDefinition());
        }

        public void LoadFilterDefinition(FilterDefinition filter)
        {
            if (String.IsNullOrWhiteSpace(filter.Name))  // temporary / new filter
            {
                filterNamePanel.Visible = false;
                txtFilterName.Text = String.Empty;
                btnDeleteFilter.Enabled = false;
            }
            else   // saved filter
            {
                filterNamePanel.Visible = true;
                txtFilterName.Text = filter.Name;
                btnDeleteFilter.Enabled = true;
            }


            // Level
            if (filter.MinLevel == LogLevel.Error)
                rbError.Checked = true;
            else if (filter.MinLevel == LogLevel.Warning)
                rbWarning.Checked = true;
            else if (filter.MinLevel == LogLevel.Info)
                rbInfo.Checked = true;
            else
                rbDebug.Checked = true;

            txtTextSearch.Text = filter.MessageTextFilter ?? String.Empty;
            txtLogger.Text = filter.LoggerNames ?? String.Empty;

            if (_userPreferences.TimeFormatMode == TimeFormatMode.RelativeTime)
            {
                txtDateFrom.Text = filter.MinTimeDifference > 0 ? filter.MinTimeDifference.ToString() : string.Empty;
                txtDateTo.Text = filter.MaxTimeDifference > 0 ? filter.MaxTimeDifference.ToString() : string.Empty;
            }
            else if(_userPreferences.TimeFormatMode == TimeFormatMode.DateTime)
            {
                txtDateFrom.Text = filter.DateFrom?.ToString(_userPreferences.DateTimeFormat) ?? string.Empty;
                txtDateTo.Text = filter.DateUntil?.ToString(_userPreferences.DateTimeFormat) ?? string.Empty;
            }
            else
            {
                txtDateFrom.Text = filter.TimeFrom?.ToString(_userPreferences.TimeFormat) ?? string.Empty;
                txtDateTo.Text = filter.TimeUntil?.ToString(_userPreferences.TimeFormat) ?? string.Empty;
            }

            txtModule.Text = string.Empty;
            txtThread.Text = string.Empty;

            if (filter.Criteria != null)
            {
                foreach (var criterion in filter.Criteria)
                {
                    if (criterion.PropertyId == PropertyIdentifiers.PROPERTY_MODULE_NAME && criterion.Operator == FilterOperator.StringIncludesOrExcludes)
                    {
                        txtModule.Text = criterion.ExpectedValues;
                    }

                    if (criterion.PropertyId == PropertyIdentifiers.PROPERTY_THREAD && criterion.Operator == FilterOperator.StringIncludesOrExcludes)
                    {
                        txtThread.Text = criterion.ExpectedValues;
                    }

                }
            }

            _currentFilterDefinition = filter;
        }

        public void ChangeDate_TimeLabels(TimeFormatMode timeFormatMode)
        {
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            if(timeFormatMode == TimeFormatMode.RelativeTime)
            {
                label2.Text = "Time Difference in ms (Min):";
                label3.Text = "Time Difference in ms (Max):";
            }
            else if(timeFormatMode == TimeFormatMode.DateTime)
            {
                label2.Text = "Date/Time (From):";
                label3.Text = "Date/Time (To):";
            }
            else
            {
                label2.Text = "Time (From):";
                label3.Text = "Time (To):";
            }
        }

        private FilterDefinition BuildFilterDefinition()
        {
            FilterDefinition filter = new FilterDefinition();

            if (!string.IsNullOrWhiteSpace(txtFilterName.Text))
            {
                filter.Name = txtFilterName.Text;
            }

            // Level
            if (rbDebug.Checked)
                filter.MinLevel = LogLevel.Debug;
            else if (rbInfo.Checked)
                filter.MinLevel = LogLevel.Info;
            else if (rbWarning.Checked)
                filter.MinLevel = LogLevel.Warning;
            else if (rbError.Checked)
                filter.MinLevel = LogLevel.Error;


            // Text Search
            if (!string.IsNullOrEmpty(txtTextSearch.Text))
            {
                filter.MessageTextFilter = txtTextSearch.Text;
            }

            // Logger Search
            if (!string.IsNullOrEmpty(txtLogger.Text))
            {
                filter.LoggerNames = txtLogger.Text;
            }

            if(_userPreferences.TimeFormatMode == TimeFormatMode.RelativeTime)
            {
                // TimeDifference Min
                if (!string.IsNullOrEmpty(txtDateFrom.Text))
                {
                    if (int.TryParse(txtDateFrom.Text, out int minTime) && minTime >= 0)
                    {
                        filter.MinTimeDifference = minTime;
                    }
                    else
                    {
                        SetErrorProvider(txtDateFrom, "Time difference must be a positive integer");
                        return null;
                    }
                }

                // TimeDifference Max
                if (!string.IsNullOrEmpty(txtDateTo.Text))
                {
                    if (int.TryParse(txtDateTo.Text, out int maxTime) && maxTime >= 0)
                    {
                        filter.MaxTimeDifference = maxTime;
                    }
                    else
                    {
                        SetErrorProvider(txtDateTo, "Time difference must be a positive integer");
                        return null;
                    }
                }
            }
            else if (_userPreferences.TimeFormatMode == TimeFormatMode.DateTime)
            {
                // Date From
                if (!string.IsNullOrEmpty(txtDateFrom.Text))
                {
                    if (DateTime.TryParseExact(txtDateFrom.Text, _userPreferences.DateTimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out DateTime dateFrom))
                    {
                        filter.DateFrom = dateFrom;
                    }
                    else
                    {
                        SetErrorProvider(txtDateFrom, "Required date format is: " + _userPreferences.DateTimeFormat);
                        return null;
                    }
                }

                // Date To
                if (!string.IsNullOrEmpty(txtDateTo.Text))
                {
                    if (DateTime.TryParseExact(txtDateTo.Text, _userPreferences.DateTimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out DateTime dateTo))
                    {
                        filter.DateUntil = dateTo;
                    }
                    else
                    {
                        SetErrorProvider(txtDateTo, "Required date format is: " + _userPreferences.DateTimeFormat);
                        return null;
                    }
                }
            }
            else
            {
                // Time From
                if (!string.IsNullOrEmpty(txtDateFrom.Text))
                {
                    if (DateTime.TryParseExact(txtDateFrom.Text, _userPreferences.TimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out DateTime timeFrom))
                    {
                        filter.TimeFrom = timeFrom;
                    }
                    else
                    {
                        SetErrorProvider(txtDateFrom, "Required time format is: " + _userPreferences.TimeFormat);
                        return null;
                    }
                }

                // Time To
                if (!string.IsNullOrEmpty(txtDateTo.Text))
                {
                    if (DateTime.TryParseExact(txtDateTo.Text, _userPreferences.TimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out DateTime timeTo))
                    {
                        filter.TimeUntil = timeTo;
                    }
                    else
                    {
                        SetErrorProvider(txtDateTo, "Required time format is: " + _userPreferences.TimeFormat);
                        return null;
                    }
                }
            }

            // Thread
            if (!string.IsNullOrEmpty(txtThread.Text))
            {
                filter.AddCriterion(PropertyIdentifiers.PROPERTY_THREAD, FilterOperator.StringIncludesOrExcludes, txtThread.Text);
            }

            // Module
            string moduleFilter = txtModule.Text;
            if (!string.IsNullOrEmpty(moduleFilter))
            {
                // List of AND/OR conditions for the logger name, becomes one condition in the outer filter
                filter.AddCriterion(PropertyIdentifiers.PROPERTY_MODULE_NAME, FilterOperator.StringIncludesOrExcludes, txtModule.Text);
            }

            return filter;
        }

        private void SetErrorProvider(Control control, string value)
        {
            criteriaTableLayout.Padding = new Padding(0, 12, 15, 0);
            errorProvider.SetError(control, value);
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            ApplyToLogView();
        }

        private void ApplyToLogView()
        {
            errorProvider.Clear();
            criteriaTableLayout.Padding = new Padding(0, 12, 3, 0);
            RequestedFilter?.Invoke(this, new ApplyFilterEventArgs(BuildFilterDefinition()));
        }

        private void SavedFilters_ItemsChanged(object sender, EventArgs e)
        {
            UpdateSavedFilterMenu();
        }

        private void UpdateSavedFilterMenu()
        {
            btnEditFilter.DropDownItems.Clear();
            if (_userPreferences.SavedFilters.Count == 0)
            {
                btnEditFilter.Enabled = false;
            }
            else
            {
                foreach (var filter in _userPreferences.SavedFilters)
                {
                    var menuItem = btnEditFilter.DropDownItems.Add(filter.Name);
                    menuItem.Tag = filter;
                    menuItem.Click += editFilterMenuItem_OnClick; ;
                }
                btnEditFilter.Enabled = true;
            }
        }

        private void editFilterMenuItem_OnClick(object sender, EventArgs e)
        {
            LoadFilterDefinition((sender as ToolStripItem).Tag as FilterDefinition);
        }

        public void SetDateFromTo(bool dateFrom, DateTime value)
        {
            if (dateFrom)
            {
                txtDateFrom.Text = value.ToString(_userPreferences.DateTimeFormat);
            }
            else
            {
                txtDateTo.Text = value.ToString(_userPreferences.DateTimeFormat);
            }
            ApplyToLogView();
        }

        public void SetThreadFilter(string thread)
        {
            txtThread.Text = thread;
            ApplyToLogView();
        }

        public void SetLoggerFilter(string loggerName, bool exclude)
        {
            if (string.IsNullOrEmpty(loggerName))
                return;

            if (AmendIncludeExcludeFilter(txtLogger, loggerName, exclude))
                ApplyToLogView();
        }

        public void AddModuleFilter(string moduleName, bool exclude)
        {
            if (string.IsNullOrEmpty(moduleName))
                return;

            if (AmendIncludeExcludeFilter(txtModule, moduleName, exclude))
                ApplyToLogView();
        }

        private bool AmendIncludeExcludeFilter(TextBox textbox, string value, bool exclude)
        {
            if (!String.IsNullOrEmpty(textbox.Text))
            {
                if (exclude && !textbox.Text.StartsWith("!"))
                {
                    MessageBox.Show("The value cannot be excluded, because there is already an include-filter.", "Debwin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else if (!exclude && textbox.Text.StartsWith("!"))
                {
                    MessageBox.Show("The value cannot be included, because there is already an exclude-filter.", "Debwin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            if (exclude && String.IsNullOrEmpty(textbox.Text))
                textbox.Text = "!";

            if (textbox.Text.Length > 1)
                textbox.Text += ";";

            textbox.Text += value;
            return true;
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            LoadFilterDefinition(new FilterDefinition());
        }

        private void FilterPanel_Load(object sender, EventArgs e)
        {
#if DEBUG
            lblLoggerName.Visible = txtLogger.Visible = true;
#endif

            filterNamePanel.Visible = false;
            // Make sure the controls are in the right order, so the filter name panel docks below the toolbar
            this.Controls.SetChildIndex(criteriaTableLayout, 0);
            this.Controls.SetChildIndex(filterNamePanel, 1);
            this.Controls.SetChildIndex(toolStrip, 2);
        }

        private void btnSaveFilter_Click(object sender, EventArgs e)
        {
            if (!filterNamePanel.Visible || string.IsNullOrWhiteSpace(txtFilterName.Text))
            {
                MessageBox.Show("Please specify a name for the filter to save.");
                filterNamePanel.Visible = true;
                txtFilterName.Focus();
                return;
            }

            FilterDefinition newFilter = BuildFilterDefinition();
            if (newFilter != null)
            {
                if (_userPreferences.SavedFilters.Contains(_currentFilterDefinition))
                {
                    _userPreferences.SavedFilters.Remove(_currentFilterDefinition);
                }

                _userPreferences.SavedFilters.Add(newFilter);
                LoadFilterDefinition(newFilter);
            }
        }

        private void FilterPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            _userPreferences.SavedFilters.ItemsChanged -= SavedFilters_ItemsChanged;
        }

        private void btnDeleteFilter_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this saved filter? (Original name: " + _currentFilterDefinition.Name + ")", "Debwin4", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                _userPreferences.SavedFilters.Remove(_currentFilterDefinition);
                txtFilterName.Text = string.Empty;
                filterNamePanel.Visible = false;
                btnDeleteFilter.Enabled = false;
            }
            else
            {
                return;
            }
        }

        

        private void txtTextSearch_TextChanged(object sender, EventArgs e)
        {
            // "Instant Search" - if we have no errors in the filter, we can try to apply it while typing
            UpdateFilterDefinition();
        }

        private void UpdateFilterDefinition()
        {
            try
            {
                var filter = BuildFilterDefinition();
                RequestedFilter?.Invoke(this, new ApplyFilterEventArgs(filter) { IsAutoTriggeredFilter = true });
            }
            catch (Exception) { }
        }

        private void rbSeverity_Click(object sender, EventArgs e)
        {
            // "Instant Search"
            UpdateFilterDefinition();
        }
    }

    public class ApplyFilterEventArgs : EventArgs
    {

        //public bool ExtendExistingFilter { get; private set; }
        public bool IsAutoTriggeredFilter { get; set; }

        public FilterDefinition FilterDefinition { get; private set; }

        public ApplyFilterEventArgs(FilterDefinition filterDefinition)
        {
            this.FilterDefinition = filterDefinition;
            //  this.ExtendExistingFilter = extendExistingFilter;
        }
    }

}
