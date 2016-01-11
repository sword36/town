using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownInterfaces
{
    public enum ThingType
    {
        FOOD,
        PRODUCT, 
        ANY
    }
    public interface IBag
    {
        float Weight
        {
            get;
            
        }

        float Count
        {
            get;
        }

        int FoodCount
        {
            get;
        }

        int ProductCount
        {
            get;
        }

        float MaxCapacity
        {
            get;
            set;
        }

        void Add(IThing th);

        IFood GetFood();

        IProduct GetProduct();

        IThing DropRandom();

        IThing GetWithPriceLower(float price, float percect, ThingType type);
    }
}
