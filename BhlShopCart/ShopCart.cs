using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhlShopCart
{
    public class ShopCart : IShopCart
    {
        //protected Dictionary<string, Decimal> PriceList = new Dictionary<string, decimal>() { { "APPLE", 0.45M }, { "ORANGE", 0.65M } };

        public Decimal Price(string[] items, IPriceList priceList)
        {
            // prep for combined special pricing
            priceList.Clear();

            var unique = items.Distinct();
            foreach (var u in unique)
            {
                priceList.Price(u, items.Where(x => x == u).Count());
            }

            return priceList.GetPrice();
        }
    }
}
