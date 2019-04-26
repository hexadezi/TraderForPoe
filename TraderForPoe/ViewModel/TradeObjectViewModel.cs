using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraderForPoe.Classes;

namespace TraderForPoe.ViewModel
{
    public class TradeObjectViewModel : ViewModelBase
    {
        private readonly TradeObject tradeObject;

        public TradeObjectViewModel(TradeObject tradeObject)
        {
            this.tradeObject = tradeObject;
        }
        public string ItemName { get { return tradeObject.Item.ItemAsString; } }

        public decimal Amount { get { return tradeObject.Item.Amount; } }
    }
}
