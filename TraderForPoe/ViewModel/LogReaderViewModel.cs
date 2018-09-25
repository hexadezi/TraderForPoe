using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using TraderForPoe.Classes;

namespace TraderForPoe.ViewModel
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

    public class LogReaderViewModel
    {
        LogMonitor logMonitor;

        Dispatcher dispatcher;

        private ObservableCollection<Line> lines = new ObservableCollection<Line>();

        public ObservableCollection<Line> Lines
        {
            get { return lines; }
        }


        public LogReaderViewModel(Dispatcher arg)
        {
            dispatcher = arg;
            logMonitor = new LogMonitor(@"C:\Program Files (x86)\Steam\steamapps\common\Path of Exile\logs\Client.txt");
            logMonitor.OnLineAddition += LogMonitor_OnLineAddition;
            logMonitor.Start();
        }

        private void LogMonitor_OnLineAddition(object sender, LogFileMonitorLineEventArgs e)
        {
            dispatcher.BeginInvoke((Action)(() =>
            {
                lines.Add(new Line() { PropLine = e.Line });
            }));
        }


    }
}
