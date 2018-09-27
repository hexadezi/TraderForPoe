using System.Collections.ObjectModel;
using TraderForPoe.Classes;

namespace TraderForPoe.ViewModel
{
    public class LogMonitorViewModel
    {
        private ObservableCollection<string> lines = new ObservableCollection<string>();

        public RelayCommand CmdStart { get; private set; }
        public RelayCommand CmdStop { get; private set; }

        private LogReader logReader;

        public LogMonitorViewModel(LogReader logReaderArg)
        {
            logReader = logReaderArg;

            logReader.OnLineAddition += LogReader_OnLineAddition;

            CmdStart = new RelayCommand(
                () => logReader.Start());

            CmdStop= new RelayCommand(
                () => logReader.Stop());
        }

        public ObservableCollection<string> Lines
        {
            get { return lines; }
        }

        private void LogReader_OnLineAddition(object sender, LogReaderLineEventArgs e)
        {
            lines.Add(e.Line);
        }
        


    }
}