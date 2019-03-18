using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhlShopCart
{
    /// <summary>
    /// CombinedPriceList is a class designed to handle the situation where multiple items can be 
    /// combined together to get specials such as Buy-one get-one-free, 3 for the price of 2, etc.
    /// </summary>
    public class CombinedPriceList : IPriceList
    {
        protected PriceList BasePriceList { get; set; }
        protected Decimal Total = 0.0M;
        protected Dictionary<string, List<Decimal>> specLists = new Dictionary<string, List<Decimal>>();

        public CombinedPriceList()
        {
            BasePriceList = new PriceList();
        }

        public void Clear()
        {
            Total = 0.0M;
            specLists.Clear();
        }

        public void AddItem(string name, Decimal price)
        {
            BasePriceList.AddItem(name, price);
        }

        public void AddSpecial(string name, int qty, int fr)
        {
            BasePriceList.AddSpecial(name, qty, fr);
        }

        public bool SetSpecial(string itemName, string specCode)
        {
            return BasePriceList.SetSpecial(itemName, specCode);
        }

        // Price() continues to add to the price prep for this new item
        public Decimal Price(string itemName, int qty)
        {
            string itemUC = itemName.ToUpper();
            if (!BasePriceList.Items.ContainsKey(itemUC)) throw new Exception(String.Format("Item '{0}' is not a valid item.", itemName));

            Decimal prc = BasePriceList.Items[itemName.ToUpper()].Price;
            string specCode = BasePriceList.Items[itemUC].SpecialCode?.ToUpper();
            if (!String.IsNullOrEmpty(specCode) && BasePriceList.Specials.ContainsKey(specCode))
            {
                if (!specLists.ContainsKey(specCode)) specLists[specCode] = new List<Decimal>();
                List<Decimal> work = specLists[specCode];
                for (int i = 0; i < qty; i++)
                    work.Add(prc);
                return -1.0M;
            }

            // this is not a special, just price this item
            Total += qty * BasePriceList.Items[itemUC].Price;
            return -1.0M;   // this is nothing
        }

        // GetPrice() calculates and retrieves the final price
        public Decimal GetPrice()
        {
            // go through all special collections
            foreach (var sc in specLists)
            {
                ISpecial spec = BasePriceList.Specials[sc.Key];
                List<Decimal> sorted = sc.Value.OrderByDescending(x => x).ToList();

                int len = sorted.Count();
                int payFor = 0;
                int free = 0;
                for (int i = 0; i < len;)
                {
                    if (payFor < spec.For)
                    {
                        Total += sorted[i];
                        payFor++;
                        i++;
                    }
                    else
                    {
                        if (free < spec.Quantity - spec.For)
                        {
                            len--;      // the cheapest one is free
                            free++;
                        }
                        else
                        {
                            // we have given away the free ones, now they need to buy again
                            payFor = free = 0;
                        }
                    }
                }
            }

            // we have the total price now
            return Total;
        }
    }
}
