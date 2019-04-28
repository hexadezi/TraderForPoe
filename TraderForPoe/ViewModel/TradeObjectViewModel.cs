using TraderForPoe.Classes;

namespace TraderForPoe.ViewModel
{
    public class TradeObjectViewModel : ViewModelBase
    {
        private readonly TradeObject tradeObject;

        private StashGridViewModel stashGridViewModel = StashGridViewModel.Instance;

        public TradeObjectViewModel(TradeObject tradeObject)
        {
            this.tradeObject = tradeObject;
        }

        public string ItemName { get { return tradeObject.Item.ItemAsString; } }

        public decimal Amount { get { return tradeObject.Item.Amount; } }
    }
}