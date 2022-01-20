using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
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
			// http://www.covingtoninnovations.com/mc/SingleInstance.html

			Mutex mutex = new Mutex(true, "TraderForPoe", out bool createdNew);

			if (!createdNew)
			{
				//app is already running! Exiting the application
				Current.Shutdown();
			}

			GC.KeepAlive(mutex);

			if (Settings.Default.UpgradeSettingsRequired)
			{
				Settings.Default.Upgrade();
				Settings.Default.UpgradeSettingsRequired = false;
				Settings.Default.Save();
			}
		}
	}
}
