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

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            StartUpClass.Initialize();

            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            new MainWindow().Show();


        }

    }
}
