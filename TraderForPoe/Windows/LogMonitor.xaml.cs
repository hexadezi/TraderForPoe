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
        public LogMonitor(LogMonitorViewModel lrvm)
        {
            InitializeComponent();
            this.DataContext = lrvm;
        }
    }
}
