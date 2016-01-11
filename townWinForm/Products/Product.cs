﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownInterfaces;

namespace products
{
    //Not food
    public class Product : Thing
    {
        public Product(float price, float weight)
        {
            Price = price;
            Weight = weight;
        }

        public Product()
        {
            Price = Util.GetRandomDistribution(Config.ProductCost, Config.ProductCostDelta);
            Weight = Util.GetRandomDistribution(Config.ProductWeight, Config.ProductWeightDelta);
        }
    }
}
