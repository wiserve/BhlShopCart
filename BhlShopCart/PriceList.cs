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
        protected Decimal Total = 0.0M;
        protected Dictionary<string, List<Decimal>> specLists = new Dictionary<string, List<Decimal>>();

        // the not-combined offer pricing method, prices each item independently, WILL NOT HANDLE COMBINED SPECIAL OFFERS
        public Decimal PriceNotCombined(string itemName, int qty)
        {
            if (!Items.ContainsKey(itemName.ToUpper())) throw new Exception(String.Format("Item '{0}' is not a valid item.", itemName));

            IItem item = Items[itemName.ToUpper()];
            Decimal prc = item.Price;
            string specCode = item.SpecialCode?.ToUpper();
            if (!String.IsNullOrEmpty(specCode) && Specials.ContainsKey(specCode))
            {
                ISpecial spec = Specials[specCode];
                Total += (prc * spec.For * (int)(qty / spec.Quantity)) + (prc * (int)(qty % spec.Quantity));
                return -1.0M;
            }

            // no special offer, just regular price
            return qty * prc;
        }

        public Decimal Price(string itemName, int qty)
        {
            string itemUC = itemName.ToUpper();
            if (!Items.ContainsKey(itemUC)) throw new Exception(String.Format("Item '{0}' is not a valid item.", itemName));

            Decimal prc = Items[itemName.ToUpper()].Price;
            string specCode = Items[itemUC].SpecialCode?.ToUpper();
            if (!String.IsNullOrEmpty(specCode) && Specials.ContainsKey(specCode))
            {
                if (!specLists.ContainsKey(specCode)) specLists[specCode] = new List<Decimal>();
                List<Decimal> work = specLists[specCode];
                for (int i = 0; i < qty; i++)
                    work.Add(prc);
                return -1.0M;
            }

            // this is not a special, just price this item
            Total += qty * Items[itemUC].Price;
            return -1.0M;   // this is nothing
        }

        // GetPrice() calculates and retrieves the final price
        public Decimal GetPrice()
        {
            // go through all special collections
            foreach (var sc in specLists)
            {
                ISpecial spec = Specials[sc.Key];
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

        //public Decimal Price(string itemName, int qty, bool isCombineSameOffer=true)
        //{
        //    string itemUC = itemName.ToUpper();
        //    if (!Items.ContainsKey(itemUC)) throw new Exception(String.Format("Item '{0}' is not a valid item.", itemName));

        //    Decimal prc = Items[itemName.ToUpper()].Price;
        //    string specCode = Items[itemUC].SpecialCode?.ToUpper();
        //    if (!String.IsNullOrEmpty(specCode) && Specials.ContainsKey(specCode))
        //    {
        //        ISpecial spec = Specials[specCode];
        //        return (prc * spec.For * (int)(qty / spec.Quantity)) + (prc * (int)(qty % spec.Quantity));
        //    }
        //    return qty * Items[itemUC].Price;
        //}

        //public Decimal GetPrice()
        //{
        //    // this isn't needed
        //    return 0.0M;
        //}

        public void AddItem(string name, Decimal price)
        {
            if (!Items.ContainsKey(name.ToUpper()))
                Items[name.ToUpper()] = new Item() { Name = name };
            Items[name.ToUpper()].Price = price;
        }

        public void AddSpecial(string name, int qty, int fr)
        {
            Specials.Add(name.ToUpper(), new Special(qty, fr));
        }

        public bool SetSpecial(string itemName, string specCode)
        {
            string item = itemName.ToUpper();
            if (!Items.ContainsKey(item)) return false;       // can't set special
            string spec = specCode.ToUpper();
            if (!Specials.ContainsKey(spec)) return false;    // can't set special

            Items[item].SpecialCode = spec;
            return true;
        }

        public void Clear()
        {
            Total = 0.0M;
            specLists.Clear();
        }
    }
}
