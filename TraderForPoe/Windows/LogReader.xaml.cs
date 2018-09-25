using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using TraderForPoe.Classes;

namespace TraderForPoe.Windows
{
    public class Line : INotifyPropertyChanged
    {
        private string line;
        public string PropLine
        {
            get { return this.line; }
            set
            {
                if (this.line != value)
                {
                    this.line = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    /// <summary>
    /// Interaktionslogik für LogReader.xaml
    /// </summary>
    public partial class LogReader : Window
    {
        private ObservableCollection<Line> lines = new ObservableCollection<Line>();

        LogMonitor logMonitor;

        public LogReader(LogMonitor arg)
        {
            InitializeComponent();
            this.lbLines.ItemsSource = this.Lines;

            logMonitor = arg;
            logMonitor.OnLineAddition += LogMonitor_OnLineAddition;
        }

        public ObservableCollection<Line> Lines
        {
            get { return lines; }
        }
        
        private void LogMonitor_OnLineAddition(object sender, LogFileMonitorLineEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                lines.Add(new Line() { PropLine = e.Line });
            }));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logMonitor.Stop();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            logMonitor.Start();
        }
    }
}
