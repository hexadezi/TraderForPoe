using System.ComponentModel;
using System.Windows;
using TraderForPoe.Classes;
using TraderForPoe.Properties;
using TraderForPoe.Windows;

namespace TraderForPoe.ViewModel
{
    public class NotifyIconViewModel : INotifyPropertyChanged
    {
        #region Fields

        private bool useClipboardMonitor = Settings.Default.UseClipboardMonitor;

        #endregion Fields

        #region Constructor

        public NotifyIconViewModel()
        {
            CmdHistory = new RelayCommand(() => WindowViewLoaderService.ShowSingle(typeof(TradeHistoryViewModel)));

            CmdLog = new RelayCommand(() => WindowViewLoaderService.Show(typeof(LogMonitorViewModel)));

            CmdSettings = new RelayCommand(() => WindowViewLoaderService.ShowSingle(typeof(UserSettingsViewModel)));

            CmdRestart = new RelayCommand(() => RestartApp());

            CmdUpdate = new RelayCommand(() => Updater.CheckForUpdate());

            CmdAbout = new RelayCommand(() => new About().Show());

            CmdQuit = new RelayCommand(() => Application.Current.Shutdown());
        }

        #endregion Constructor

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        public RelayCommand CmdAbout { get; private set; }
        public RelayCommand CmdHistory { get; private set; }
        public RelayCommand CmdLog { get; private set; }
        public RelayCommand CmdMonitor { get; private set; }
        public RelayCommand CmdQuit { get; private set; }
        public RelayCommand CmdRestart { get; private set; }
        public RelayCommand CmdSettings { get; private set; }
        public RelayCommand CmdUpdate { get; private set; }

        public bool UseClipboardMonitor
        {
            get { return useClipboardMonitor; }
            set
            {
                if (useClipboardMonitor != value)
                {
                    useClipboardMonitor = value;
                    Settings.Default.UseClipboardMonitor = value;
                    Settings.Default.Save();
                    this.NotifyPropertyChanged(nameof(UseClipboardMonitor));
                }
            }
        }

        #endregion Properties

        #region Methods

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void RestartApp()
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            System.Windows.Application.Current.Shutdown();
        }

        #endregion Methods
    }
}