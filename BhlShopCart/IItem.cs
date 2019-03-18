using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhlShopCart
{
    public interface IItem
    {
        string Name { get; set; }
        Decimal Price { get; set; }
        string SpecialCode { get; set; }
    }
}
