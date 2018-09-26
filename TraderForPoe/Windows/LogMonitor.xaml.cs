using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using TraderForPoe.Classes;
using TraderForPoe.ViewModel;

namespace TraderForPoe.Windows
{
    /// <summary>
    /// Interaktionslogik für LogReader.xaml
    /// </summary>
    public partial class LogMonitor : Window
    {
        LogMonitorViewModel lmvr;
        public LogMonitor(LogMonitorViewModel lmvrArg)
        {
            InitializeComponent();
            this.DataContext = lmvrArg;
            lmvr = lmvrArg;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lmvr.Stop();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            lmvr.Start();
        }
    }
}
