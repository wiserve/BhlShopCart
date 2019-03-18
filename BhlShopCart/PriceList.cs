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

        // this was the original price method, it was based on special connected to item rather than offer
        //public Decimal Price(string itemName, int qty)
        //{
        //    if (!Items.ContainsKey(itemName.ToUpper())) throw new Exception(String.Format("Item '{0}' is not a valid item.", itemName));

        //    Decimal prc = Items[itemName.ToUpper()].Price;
        //    if (Specials.ContainsKey(itemName.ToUpper()))
        //    {
        //        ISpecial spec = Specials[itemName.ToUpper()];
        //        return (prc * spec.For * (int)(qty / spec.Quantity)) + (prc * (int)(qty % spec.Quantity));
        //    }
        //    return qty * Items[itemName.ToUpper()].Price;
        //}

        public Decimal Price(string itemName, int qty)
        {
            string itemUC = itemName.ToUpper();
            if (!Items.ContainsKey(itemUC)) throw new Exception(String.Format("Item '{0}' is not a valid item.", itemName));

            Decimal prc = Items[itemName.ToUpper()].Price;
            string specCode = Items[itemUC].SpecialCode?.ToUpper();
            if (!String.IsNullOrEmpty(specCode) && Specials.ContainsKey(specCode))
            {
                ISpecial spec = Specials[specCode];
                return (prc * spec.For * (int)(qty / spec.Quantity)) + (prc * (int)(qty % spec.Quantity));
            }
            return qty * Items[itemUC].Price;
        }

        public Decimal GetPrice()
        {
            // this isn't needed
            return 0.0M;
        }

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

        public void Clear() { }
    }
}
