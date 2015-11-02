using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm.Things
{
    //Not food
    public class Product : Thing
    {
        public Product(float price)
        {
            Price = price;
        }

        public Product()
        {
            Price = Util.GetRandomDistribution(Config.ProductCost, Config.ProductCostDelta);
        }
    }
}
