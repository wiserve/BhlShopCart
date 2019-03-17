using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhlShopCart
{
    public class Item : IItem
    {
        public string Name { get; set; }
        public Decimal Price { get; set; }
    }
}
