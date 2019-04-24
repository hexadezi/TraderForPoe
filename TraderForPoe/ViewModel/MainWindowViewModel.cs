using System.Collections.ObjectModel;
using TraderForPoe.Classes;
using TraderForPoe.Controls;
using TraderForPoe.Properties;

namespace TraderForPoe.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private ClipboardMonitor clipMonitor = new ClipboardMonitor();

        #endregion Fields

        public MainWindowViewModel()
        {
            SubscribeToEvents();
        }

        #region Properties

        public static ObservableCollection<CustomTestCtrl> TradeControlList { get; set; } = CustomTestCtrl.TradeControlList;

        public float ControlOpacity
        {
            get { return Settings.Default.ControlOpacity; }
        }

        #endregion Properties

        #region Methods

        private void ClipMonitor_OnChange(object sender, ClipboardTextEventArgs e)
        {
            if (Properties.Settings.Default.UseClipboardMonitor == true)
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
                TradeObject tItem = new TradeObject(e.Line);
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