using System;
using System.Collections.ObjectModel;
using TraderForPoe.Classes;
using TraderForPoe.Properties;
using TraderForPoe.Windows;

namespace TraderForPoe.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private ClipboardMonitor clipboardMonitor = new ClipboardMonitor();
        private StashGridViewModel stashGridViewModel = StashGridViewModel.Instance;

        #endregion Fields

        #region Constructors

        public MainWindowViewModel()
        {
            SubscribeToEvents();
            SetUpStashGrid();
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
            if (TradeObject.IsLogTradeWhisper(e.Line))
            {
                var to = new TradeObject(e.Line);
                var tovm = new TradeObjectViewModel(to);
                TradeObjects.Add(tovm);
            }
        }

        private void SubscribeToEvents()
        {
            clipboardMonitor.OnChange += ClipMonitor_OnChange;
            LogReader.OnLineAddition += LogReader_OnLineAddition;
        }

        private void SetUpStashGrid()
        {
            WindowViewLoaderService.ShowView(typeof(StashGridViewModel));
        }
        
        #endregion Methods
    }
}