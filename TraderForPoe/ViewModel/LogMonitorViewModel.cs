using System.Collections.ObjectModel;
using TraderForPoe.Classes;

namespace TraderForPoe.ViewModel
{
    public class LogMonitorViewModel
    {
        private LogReader logReader;

        public LogMonitorViewModel(LogReader logReaderArg)
        {
            logReader = logReaderArg;

            logReader.OnLineAddition += LogReader_OnLineAddition;

            CmdStart = new RelayCommand(
                () => logReader.Start());

            CmdStop = new RelayCommand(
                () => logReader.Stop());

            CmdClear = new RelayCommand(
                () => Lines.Clear());
        }

        public RelayCommand CmdClear { get; private set; }

        public RelayCommand CmdStart { get; private set; }

        public RelayCommand CmdStop { get; private set; }

        public ObservableCollection<string> Lines { get; } = new ObservableCollection<string>();

        private void LogReader_OnLineAddition(object sender, LogReaderLineEventArgs e)
        {
            Lines.Add(e.Line);
        }
    }
}