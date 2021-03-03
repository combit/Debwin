using Debwin.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Debwin.UI.Util
{

    public enum TimeFormatMode
    {
        TimeOnly,
        DateTime,
        RelativeTime,
        RelativeToReference
    }

    public interface IUserPreferences : INotifyPropertyChanged
    {
        string DateTimeFormat { get; set; }

        string TimeFormat { get; set; }

        TimeFormatMode TimeFormatMode { get; set; }

        string EditorPath { get; set; }

        bool EnableAutoScrollOnScrollToEnd { get; set; }

        bool DisableAutoScrollOnSelection { get; set; }

        bool EnableAutoScrollOnClearLog { get; set; }

        bool IgnoreLogIndentation { get; set; }

        bool ShowTabChars { get; set; }
        
        bool EnableLongTermMonitoring { get; set; }

        List<int> SelectedPropertyIDs { get; set; }

        Rectangle MainWindowBounds { get; set; }

        FormWindowState MainWindowState { get; set; }

        bool MainWindowTopMost { get; set; }

        bool MainWindowHideOnMinimize { get; set; }

        bool InstallKeyboardHook { get; set; }

        /// <summary>Stores the layout of the LogViewPanel's columns (per log type).</summary>
        List<LogViewPanelColumnSet> LogViewPanelColumnSets { get; }

        float ListLogMessageFontSize { get; set; }

        ObservableList<FilterDefinition> SavedFilters { get; }

        int MaximumMessageCount { get; set; }

        string LogFilePath { get; set; }

        ArrayList SearchHistory { get; set; }
    }

    [Serializable]
    public class LogViewPanelColumnSet
    {
        public string ControllerName { get; set; }

        public DateTime ModificationDate { get; set; }

        public List<LogViewPanelColumnDefinition> Columns { get; set; }

        public LogViewPanelColumnSet()
        {
            ModificationDate = DateTime.Now;
        }
    }

    public class ObservableList<T> : ICollection<T>
    {
        private readonly List<T> _internalList;

        public ObservableList(List<T> wrappedList)
        {
            _internalList = wrappedList;
        }

        public event EventHandler ItemsChanged;

        public int Count => _internalList.Count;

        public bool IsReadOnly => ((ICollection<T>)_internalList).IsReadOnly;

        public void Add(T item)
        {
            _internalList.Add(item);
            _internalList.Sort();
            ItemsChanged(this, new EventArgs());

        }

        public void Clear()
        {
            _internalList.Clear();
            ItemsChanged(this, new EventArgs());
        }

        public bool Contains(T item)
        {
            return _internalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _internalList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((ICollection<T>)_internalList).GetEnumerator();
        }

        public bool Remove(T item)
        {
            var temp = _internalList.Remove(item);
            ItemsChanged(this, new EventArgs());
            return temp;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<T>)_internalList).GetEnumerator();
        }
    }


    public class LogViewPanelColumnDefinition
    {
        public int MessagePropertyID { get; set; }

        public int Width { get; set; }

        public LogViewPanelColumnDefinition() { }

        public LogViewPanelColumnDefinition(int propertyID, int width)
        {
            this.MessagePropertyID = propertyID;
            this.Width = width;
        }
    }


    public class UserPreferences : IUserPreferences
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _dateTimeFormat;
        private bool _showTabChars;
        private string _timeFormat;
        private TimeFormatMode _timeFormatMode;
        private List<FilterDefinition> _savedFilters;

        public UserPreferences()
        {
            DateTimeFormat = "dd.MM.yyyy HH:mm:ss.fff";
            TimeFormat = "HH:mm:ss.fff";
            EditorPath = "notepad.exe";
            DisableAutoScrollOnSelection = true;
            MainWindowState = FormWindowState.Normal;
            TimeFormatMode = TimeFormatMode.TimeOnly;
            MainWindowHideOnMinimize = true;
            InstallKeyboardHook = false;
            LogViewPanelColumnSets = new List<LogViewPanelColumnSet>();
            MaximumMessageCount = 1000000;
            ListLogMessageFontSize = 8.25F;
            SearchHistory = new ArrayList();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string EditorPath { get; set; }

        public bool EnableAutoScrollOnScrollToEnd { get; set; }

        public bool DisableAutoScrollOnSelection { get; set; }

        public bool EnableAutoScrollOnClearLog { get; set; }

        public bool IgnoreLogIndentation { get; set; }

        public bool EnableLongTermMonitoring { get; set; }

        public List<int> SelectedPropertyIDs { get; set; }

        public Rectangle MainWindowBounds { get; set; }

        public FormWindowState MainWindowState { get; set; }

        public bool MainWindowTopMost { get; set; }

        public bool MainWindowHideOnMinimize { get; set; }

        public bool InstallKeyboardHook { get; set; }

        public List<LogViewPanelColumnSet> LogViewPanelColumnSets { get; set; }

        public float ListLogMessageFontSize { get; set; }

        public string DateTimeFormat
        {
            get { return _dateTimeFormat; }
            set
            {
                _dateTimeFormat = value;
                OnPropertyChanged(nameof(DateTimeFormat));
            }
        }

        public bool ShowTabChars
        {
            get { return _showTabChars; }
            set
            {
                _showTabChars = value;
                OnPropertyChanged(nameof(ShowTabChars));
            }
        }

        public string TimeFormat
        {
            get { return _timeFormat; }
            set
            {
                _timeFormat = value;
                OnPropertyChanged(nameof(TimeFormat));
            }
        }

        public TimeFormatMode TimeFormatMode
        {
            get { return _timeFormatMode; }
            set
            {
                _timeFormatMode = value;
                OnPropertyChanged(nameof(TimeFormatMode));
            }
        }

        // Internal Filter List for XML Serialization
        public List<FilterDefinition> SavedFiltersInternal
        {
            get
            {
                _savedFilters = _savedFilters ?? new List<FilterDefinition>();
                return _savedFilters;
            }
            set => _savedFilters = value;
        }

        // SavedFiltersInternal wrapped as public observable filter list for use by other classes  (non-serialized member)
        private ObservableList<FilterDefinition> savedFiltersWrapper;

        [XmlIgnore]
        public ObservableList<FilterDefinition> SavedFilters
        {
            get
            {
                savedFiltersWrapper = savedFiltersWrapper ?? new ObservableList<FilterDefinition>(SavedFiltersInternal);
                return savedFiltersWrapper;
            }
        }

        public ArrayList SearchHistory
        {
            get; set;
        }

        public int MaximumMessageCount { get; set; }
        private string _logFilePath;
        public string LogFilePath
        {
            get
            {
                return String.IsNullOrEmpty(_logFilePath) ? Path.GetTempPath() : _logFilePath;
            }
            set
            {
                _logFilePath = value;
            }
        }


        public void SaveAsXml(string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            XmlSerializer xz = new XmlSerializer(typeof(UserPreferences));
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xz.Serialize(fs, this);
            }
        }

        public static UserPreferences LoadFromFile(string filePath)
        {
            XmlSerializer xz = new XmlSerializer(typeof(UserPreferences));
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (UserPreferences)xz.Deserialize(fs);
            }
        }

    }
}
