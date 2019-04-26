using System.Collections.ObjectModel;
using TraderForPoe.Classes;
using TraderForPoe.Properties;

namespace TraderForPoe.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private ClipboardMonitor clipMonitor = new ClipboardMonitor();

        #endregion Fields

        #region Constructors

        public MainWindowViewModel()
        {
            SubscribeToEvents();
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<TradeObjectViewModel> TradeObjects { get; set; } = new ObservableCollection<TradeObjectViewModel>();

        public float ControlOpacity
        {
            get { return Settings.Default.ControlOpacity; }
        }

        #endregion Properties

        #region Methods

        private void ClipMonitor_OnChange(object sender, ClipboardTextEventArgs e)
        {
            if (Settings.Default.UseClipboardMonitor == true)
            {
                if (TradeObject.IsTradeWhisper(e.Line))
                {
                    Poe.SendCommand(e.Line);
                }
            }
        }

        private void LogReader_OnLineAddition(object sender, LogReaderLineEventArgs e)
        {
            //TODO Implementieren
            //throw new System.NotImplementedException();

            if (TradeObject.IsLogTradeWhisper(e.Line))
            {
                var to = new TradeObject(e.Line);
                var tovm = new TradeObjectViewModel(to);
                TradeObjects.Add(tovm);
            }
        }

        private void SubscribeToEvents()
        {
            clipMonitor.OnChange += ClipMonitor_OnChange;
            LogReader.OnLineAddition += LogReader_OnLineAddition;
        }

        #endregion Methods
    }
}