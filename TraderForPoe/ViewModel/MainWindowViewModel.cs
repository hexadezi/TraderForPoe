using System.Collections.ObjectModel;
using TraderForPoe.Classes;
using TraderForPoe.Controls;

namespace TraderForPoe.ViewModel
{
    public class MainWindowViewModel
    {
        #region Fields

        private ClipboardMonitor clipMoni = new ClipboardMonitor();

        #endregion Fields

        public MainWindowViewModel()
        {
            SubscribeToEvents();
        }

        #region Properties

        public static ObservableCollection<CustomTestCtrl> TradeControlList { get; set; } = CustomTestCtrl.TradeControlList;

        #endregion Properties

        #region Methods

        private void ClipMoni_OnChange(object sender, ClipboardTextEventArgs e)
        {
            if (TradeObject.IsTradeWhisper(e.Line))
            {
                Poe.SendCommand(e.Line);
            }
        }

        private void SubscribeToEvents()
        {
            clipMoni.OnChange += ClipMoni_OnChange;
        }

        #endregion Methods
    }
}