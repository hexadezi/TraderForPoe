using System.Collections.ObjectModel;
using TraderForPoe.Classes;

namespace TraderForPoe.ViewModel
{
    public class LogMonitorViewModel
    {
        public LogMonitorViewModel()
        {
            LogReader.OnLineAddition += LogReader_OnLineAddition;

            CmdStart = new RelayCommand(
                () => LogReader.Start());

            CmdStop = new RelayCommand(
                () => LogReader.Stop());

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