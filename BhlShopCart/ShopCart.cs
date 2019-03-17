using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhlShopCart
{
    public class ShopCart : IShopCart
    {
        protected Dictionary<string, Decimal> PriceList = new Dictionary<string, decimal>() { { "APPLE", 0.45M }, { "ORANGE", 0.65M } };

        public Decimal Price(string[] items)
        {
            Decimal cost = 0.0M;
            foreach (string it in items)
            {
                if (!PriceList.ContainsKey(it.ToUpper())) throw new Exception(String.Format("Item '{0}' is not a valid item.", it));
                cost += PriceList[it.ToUpper()];
            }
            return cost;
        }
    }
}
