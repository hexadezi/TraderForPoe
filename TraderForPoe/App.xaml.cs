using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TraderForPoe.Properties;

namespace TraderForPoe
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            if (Settings.Default.UpgradeSettingsRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeSettingsRequired = false;
                Settings.Default.Save();
            }
        }
    }
}
