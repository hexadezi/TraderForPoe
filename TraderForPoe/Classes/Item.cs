using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraderForPoe.Classes
{
    public class Item : ItemBase
    {
        public Item(string itemArg, decimal amountArg = 0) : base(itemArg, amountArg)
        {
            //TODO
            //throw new NotImplementedException();

        }

        private ItemBase price;

        public ItemBase Price
        {
            get { return price; }
            set { price = value; }
        }

        private decimal ratio;

        public decimal Ratio
        {
            //TODO Implement ratio
            get { return ratio; }
            set { ratio = value; }
        }



    }
}
