using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhlShopCart
{
    public interface IPriceList
    {
        Decimal Price(string itemName, int qty);
        Decimal GetPrice();
        void AddItem(string name, Decimal price);
        void AddSpecial(string name, int qty, int fr);
        bool SetSpecial(string itemName, string specCode);
        void Clear();
    }
}
