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
        public NotifyIconViewModel()
        {
            CmdHistory = new RelayCommand(() => new TradeHistory().Show());
            CmdLog = new RelayCommand(() => new LogMonitor(StartUpClass.VM_LogMonitor).Show());
            CmdSettings = new RelayCommand(() => OpenSettings());

            CmdRestart = new RelayCommand(() => RestartApp());
            CmdUpdate = new RelayCommand(() => Updater.CheckForUpdate());
            CmdAbout = new RelayCommand(() => new About().Show());
            CmdQuit = new RelayCommand(() => Application.Current.Shutdown());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand CmdAbout { get; private set; }
        public RelayCommand CmdHistory { get; private set; }
        public RelayCommand CmdLog { get; private set; }
        public RelayCommand CmdMonitor { get; private set; }
        public RelayCommand CmdQuit { get; private set; }
        public RelayCommand CmdRestart { get; private set; }
        public RelayCommand CmdSettings { get; private set; }
        public RelayCommand CmdUpdate { get; private set; }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void RestartApp()
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            System.Windows.Application.Current.Shutdown();
        }

        private void OpenSettings()
        {
            UserSettings us = new UserSettings();
            us.Activate();
            us.Show();
            us.Activate();
        }

        //private bool isRunning = MainWindow.logReader.IsRunning;

        //public bool IsRunning
        //{
        //    get { return this.isRunning; }
        //    set
        //    {
        //        if (this.isRunning != value)
        //        {
        //            this.isRunning = value;
        //            this.NotifyPropertyChanged("IsRunning");
        //        }
        //    }
        //}

    }
}