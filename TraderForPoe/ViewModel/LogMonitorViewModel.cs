using System;
using System.Collections.ObjectModel;
using TraderForPoe.Classes;

namespace TraderForPoe.ViewModel
{
    public class LogMonitorViewModel
    {
        private ObservableCollection<string> lines = new ObservableCollection<string>();

        private LogReader logReader;

        public LogMonitorViewModel(LogReader logReaderArg)
        {
            logReader = logReaderArg;

            logReader.OnLineAddition += LogReader_OnLineAddition;
        }

        public ObservableCollection<string> Lines
        {
            get { return lines; }
        }

        private void LogReader_OnLineAddition(object sender, LogReaderLineEventArgs e)
        {
            lines.Add(e.Line);
        }

        public void Stop()
        {
            logReader.Stop();
        }
        public void Start()
        {
            logReader.Start();
        }
    }
}