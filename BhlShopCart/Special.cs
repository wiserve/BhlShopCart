using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhlShopCart
{
    public class Special : ISpecial
    {
        public int Quantity { get; set; }
        public int For { get; set; }

        public Special(int qty, int fr)
        {
            Quantity = qty;
            For = fr;
        }
    }
}
