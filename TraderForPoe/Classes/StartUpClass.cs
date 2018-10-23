using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Windows;
using TraderForPoe.Classes;
using TraderForPoe.Properties;
using TraderForPoe.ViewModel;

namespace TraderForPoe
{
    public static class StartUpClass
    {

        public static LogReader LogFileReader { get; set; }

        public static LogMonitorViewModel VM_LogMonitor { get; set; }

        public static MainWindowViewModel VM_MainWindow { get; set; }

        public static TradeHistoryViewModel VM_TradeHistory { get; set; }

        public static UserSettingsViewModel VM_UserSettings { get; set; }

        public static NotifyIconViewModel VM_NotifyIcon { get; set; }

        public static void Initialize()
        {
            LogFileReader = new LogReader(Settings.Default.PathToClientTxt, TimeSpan.FromMilliseconds(200));

            VM_LogMonitor = new LogMonitorViewModel(LogFileReader);
            VM_MainWindow = new MainWindowViewModel();
            VM_TradeHistory = new TradeHistoryViewModel();
            VM_UserSettings = new UserSettingsViewModel();
            VM_NotifyIcon = new NotifyIconViewModel();

            
        }
        
    }
}