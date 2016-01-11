using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownInterfaces;

namespace products
{
    public class Food : Thing, IFood
    {
        public float Energy { get; set; }

        public Food(float price, float energy, float weight)
        {
            Price = price;
            Energy = energy;
            Weight = weight;
        }

        public Food()
        {
            Price = Util.GetRandomDistribution(Config.FoodCost, Config.FoodCostDelta);
            Energy = Price / 10;
            Weight = Util.GetRandomDistribution(Config.FoodWeight, Config.FoodWeightDelta);
        }
    }
}
