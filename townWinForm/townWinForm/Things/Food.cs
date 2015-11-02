using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm.Things
{
    public class Food : Thing
    {
        public float Energy;

        public Food(float price, float energy)
        {
            Price = price;
            Energy = energy;
        }

        public Food()
        {
            Price = Util.GetRandomDistribution(Config.FoodCost, Config.FoodCostDelta);
            Energy = Price / 10;
        }
    }
}
