using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TraderForPoe.Properties;
using Hardcodet.Wpf.TaskbarNotification;
using TraderForPoe.Windows;
using TraderForPoe.Controls;
using TraderForPoe.Classes;
using TraderForPoe.ViewModel;

namespace TraderForPoe
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;

        public App()
        {
            // Check if settings upgrade is needed
            if (Settings.Default.UpgradeSettingsRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeSettingsRequired = false;
                Settings.Default.Save();
            }

            RegisterViewModel();
        }

        private void RegisterViewModel()
        {
            WindowViewLoaderService.Register(typeof(LogMonitorViewModel), typeof(LogMonitor));
            WindowViewLoaderService.Register(typeof(MainWindowViewModel), typeof(MainWindow));
            WindowViewLoaderService.Register(typeof(TradeHistoryViewModel), typeof(TradeHistory));
            WindowViewLoaderService.Register(typeof(UserSettingsViewModel), typeof(UserSettings));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            FindResource("NotifyIcon");
        }

    }
}
