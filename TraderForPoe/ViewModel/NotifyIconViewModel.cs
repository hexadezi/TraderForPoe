using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TraderForPoe.Classes;
using TraderForPoe.Windows;

namespace TraderForPoe.ViewModel
{
    public class NotifyIconViewModel : INotifyPropertyChanged
    {
        #region Constructor
        public NotifyIconViewModel()
        {
            CmdHistory = new RelayCommand(() => WindowViewLoaderService.Show(typeof(TradeHistoryViewModel)));

            CmdLog = new RelayCommand(() => WindowViewLoaderService.Show(typeof(LogMonitorViewModel)));

            CmdSettings = new RelayCommand(() => WindowViewLoaderService.Show(typeof(UserSettingsViewModel)));

            CmdRestart = new RelayCommand(() => RestartApp());

            CmdUpdate = new RelayCommand(() => Updater.CheckForUpdate());

            CmdAbout = new RelayCommand(() => new About().Show());

            CmdQuit = new RelayCommand(() => Application.Current.Shutdown());

        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public RelayCommand CmdAbout { get; private set; }
        public RelayCommand CmdHistory { get; private set; }
        public RelayCommand CmdLog { get; private set; }
        public RelayCommand CmdMonitor { get; private set; }
        public RelayCommand CmdQuit { get; private set; }
        public RelayCommand CmdRestart { get; private set; }
        public RelayCommand CmdSettings { get; private set; }
        public RelayCommand CmdUpdate { get; private set; }
        #endregion

        #region Methods
        private void RestartApp()
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            System.Windows.Application.Current.Shutdown();
        }
        #endregion
    }
}