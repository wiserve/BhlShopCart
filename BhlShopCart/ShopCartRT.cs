using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhlShopCart
{
    /// <summary>
    /// ShopCartRT - this is a running-total shopping cart, it builds the list of 
    /// </summary>
    public class ShopCartRT
    {
        protected ShopCart _shopCart = new ShopCart();
        protected List<string> shoppingList = new List<string>();

        public void Clear()
        {
            shoppingList.Clear();
        }

        public Decimal Price(string itemName, IPriceList priceList)
        {
            shoppingList.Add(itemName);
            return _shopCart.Price(shoppingList.ToArray(), priceList);
        }

        public string DisplayList()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string it in shoppingList)
                sb.Append((sb.Length > 0 ? ", " : String.Empty) + it);
            return sb.ToString();
        }
    }
}
