﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhlShopCart
{
    public interface ISpecial
    {
        int Quantity { get; set; }
        int For { get; set; }
    }
}
