﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhlShopCart
{
    public interface IShopCart
    {
        Decimal Price(string[] items, IPriceList priceList);
    }
}
