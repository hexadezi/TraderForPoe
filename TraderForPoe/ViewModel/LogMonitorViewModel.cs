using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using TraderForPoe.Classes;

namespace TraderForPoe.ViewModel
{
    public class LogMonitorViewModel : ViewModelBase
    {
        #region Fields

        private ICollectionView linesView;
        private string filter;

        #endregion Fields

        #region Contructors

        public LogMonitorViewModel()
        {
            LogReader.OnLineAddition += LogReader_OnLineAddition;

            CmdStart = new RelayCommand(
                () => LogReader.Start());

            CmdStop = new RelayCommand(
                () => LogReader.Stop());

            CmdClear = new RelayCommand(
                () => Lines.Clear());

            linesView = CollectionViewSource.GetDefaultView(Lines);
            linesView.Filter = UserFilter;


        }

        #endregion Contructors

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        public RelayCommand CmdClear { get; private set; }

        public RelayCommand CmdStart { get; private set; }

        public RelayCommand CmdStop { get; private set; }

        public ObservableCollection<string> Lines { get; } = new ObservableCollection<string>();

        public string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                if (value != filter)
                {
                    filter = value;
                    linesView.Refresh();
                    OnPropertyChanged();
                }
            }
        }

        #endregion Properties

        #region Methods

        private void LogReader_OnLineAddition(object sender, LogReaderLineEventArgs e)
        {
            Lines.Add(e.Line);
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(Filter))
                return true;
            else
                return ((string)item).ToLower().Contains(Filter.ToLower());
        }

        #endregion Methods
    }
}