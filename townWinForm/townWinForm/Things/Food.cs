using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm.Things
{
    public class Food : Thing
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
        }
    }
}
