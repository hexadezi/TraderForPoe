using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TraderForPoe.Controls
{
    class CustMenuItem : System.Windows.Forms.ToolStripMenuItem
    {
        private static int countItems = 0;

        public static event EventHandler OnItemCountExceed;

        public TradeItemControl GetTradeItemCtrl { get; set; }

        public CustMenuItem(TradeItemControl tradeItemControl)
        {
            countItems++;

            GetTradeItemCtrl = tradeItemControl;
            Text = GetTradeItemCtrl.tItem.Customer + ": " + GetTradeItemCtrl.tItem.Item;
            if (GetTradeItemCtrl.tItem.TradeType == TradeItem.TradeTypes.BUY)
            {
                Image = Properties.Resources.arrowBuy;
            }
            else if (GetTradeItemCtrl.tItem.TradeType == TradeItem.TradeTypes.SELL)
            {
                Image = Properties.Resources.arrowSell;
            }

            if (countItems > 3)
            {
                if (OnItemCountExceed != null)
                {
                    OnItemCountExceed(this, EventArgs.Empty);
                }
            }

        }

    }
}
