using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhlShopCart
{
    public class PriceList : IPriceList
    {
        public Dictionary<string, IItem> Items = new Dictionary<string, IItem>();
        public Dictionary<string, ISpecial> Specials = new Dictionary<string, ISpecial>();

        public Decimal Price(string itemName, int qty)
        {
            if (!Items.ContainsKey(itemName.ToUpper())) throw new Exception(String.Format("Item '{0}' is not a valid item.", itemName));

            Decimal prc = Items[itemName.ToUpper()].Price;
            if (Specials.ContainsKey(itemName.ToUpper()))
            {
                ISpecial spec = Specials[itemName.ToUpper()];
                return (prc * spec.For * (int)(qty / spec.Quantity)) + (prc * (int)(qty % spec.Quantity));
            }
            return qty * Items[itemName.ToUpper()].Price;
        }
    }
}
