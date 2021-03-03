using Debwin.Core;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace Debwin.UI.Forms
{
    public partial class LogViewPanelColumnsDialog : Form
    {

        private class PropertyInfoItem
        {
            public readonly LogMessagePropertyInfo PropInfo;
            public PropertyInfoItem(LogMessagePropertyInfo propInfo) { this.PropInfo = propInfo; }
            public override string ToString()
            {
                return PropInfo.UIName;
            }
        }

        public LogViewPanelColumnsDialog(IEnumerable<LogMessagePropertyInfo> availableProperties, IList<int> selectedPropertyIDs)
        {
            InitializeComponent();
            List<int> alreadyAddedIDs = new List<int>() { PropertyIdentifiers.PROPERTY_LEVEL };   // ignore level - is always the first column and cannot be removed

            IEnumerable<LogMessagePropertyInfo> selectedPropertiesList = GetLogMessagePropertyInfos(alreadyAddedIDs, availableProperties, selectedPropertyIDs);
            IEnumerable<LogMessagePropertyInfo> availablePropertiesList = GetLogMessagePropertyInfos(alreadyAddedIDs, availableProperties);
     
            foreach (var item in selectedPropertiesList)
            {
               lstSelectedProps.Items.Add(new PropertyInfoItem(item));
            }

            foreach (var item in availablePropertiesList)
            {
               lstAvailableProps.Items.Add(new PropertyInfoItem(item));
            }
        }

        private IEnumerable<LogMessagePropertyInfo> GetLogMessagePropertyInfos(List<int> alreadyAddedIDs, IEnumerable<LogMessagePropertyInfo> availableProperties)
        {
            List<LogMessagePropertyInfo> logMessagePropertyInfos = new List<LogMessagePropertyInfo>();
            foreach (var propInfo in availableProperties)
            {
                if (alreadyAddedIDs.Contains(propInfo.PropertyID))
                    continue;

                logMessagePropertyInfos.Add(propInfo);
                alreadyAddedIDs.Add(propInfo.PropertyID);
            }
            return logMessagePropertyInfos.ToList();
        }

        private IEnumerable<LogMessagePropertyInfo> GetLogMessagePropertyInfos(List<int> alreadyAddedIDs, IEnumerable<LogMessagePropertyInfo> availableProperties, IList<int> selectedPropertyIDs)
        {
            List<LogMessagePropertyInfo> logMessagePropertyInfos = new List<LogMessagePropertyInfo>();
            foreach (int selectedPropID in selectedPropertyIDs)
            {
                if (alreadyAddedIDs.Contains(selectedPropID))
                    continue;
 
                var propInfo = availableProperties.FirstOrDefault(p => p.PropertyID == selectedPropID);
                if (propInfo == null)
                    continue;

                logMessagePropertyInfos.Add(propInfo);
                alreadyAddedIDs.Add(selectedPropID);
            }
            return logMessagePropertyInfos.ToList();
        }

        private void btnUpDown_Click(object sender, System.EventArgs e)
        {
            bool up = (sender == btnUp);

            if (lstSelectedProps.SelectedIndex == -1)
                return;

            int targetIndex = up ? Math.Max(0, lstSelectedProps.SelectedIndex - 1) : Math.Min(lstSelectedProps.Items.Count - 1, lstSelectedProps.SelectedIndex + 1); 
            object tmp = lstSelectedProps.SelectedItem;
            lstSelectedProps.Items.RemoveAt(lstSelectedProps.SelectedIndex);
            lstSelectedProps.Items.Insert(targetIndex, tmp);
            lstSelectedProps.SetSelected(targetIndex, true);
        }

        public IEnumerable<int> SelectedPropertyIDs { get; private set; }

        private void btnApply_Click(object sender, EventArgs e)
        {
            var result = new List<int>();
            result.Add(PropertyIdentifiers.PROPERTY_LEVEL);   // level is always the first column and cannot be removed
            foreach (PropertyInfoItem selectedProperty in lstSelectedProps.Items)
            {
                result.Add(selectedProperty.PropInfo.PropertyID);
            }
            SelectedPropertyIDs = result;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveFromSelected();
        }

        private void RemoveFromSelected()
        {
            if (lstSelectedProps.SelectedIndex == -1)
                return;

            lstAvailableProps.Items.Add(lstSelectedProps.SelectedItem);
            lstSelectedProps.Items.RemoveAt(lstSelectedProps.SelectedIndex);
            UpdateButtonStates();
        }

        private void AddToSelected()
        {
            if (lstAvailableProps.SelectedIndex == -1)
                return;

            lstSelectedProps.Items.Add(lstAvailableProps.SelectedItem);
            lstAvailableProps.Items.RemoveAt(lstAvailableProps.SelectedIndex);
            UpdateButtonStates();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddToSelected();
        }

        private void lstSelectedProps_DoubleClick(object sender, EventArgs e)
        {
            RemoveFromSelected();
        }

        private void lstAvailableProps_DoubleClick(object sender, EventArgs e)
        {
            AddToSelected();
        }

        private void lstSelectedProps_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            btnUp.Enabled = (lstSelectedProps.SelectedIndex > 0);
            btnDown.Enabled = (lstSelectedProps.SelectedIndex < lstSelectedProps.Items.Count - 1);
            btnAdd.Enabled = (lstAvailableProps.Items.Count > 0 && lstAvailableProps.SelectedIndex != -1);
            btnRemove.Enabled = (lstSelectedProps.Items.Count > 0 && lstSelectedProps.SelectedIndex != -1);
        }
    }
}
