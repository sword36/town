using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownInterfaces;

namespace townWinForm
{
    public class OverloadedBagExeption : Exception { }

    public class NoFoodExeption : Exception { }

    public class NoProductExeption : Exception { }

    public enum ThingType
    {
        FOOD,
        PRODUCT,
        ANY
    }

    public class Bag : IBag
    {
        public enum ThingType
        {
            FOOD,
            PRODUCT,
            ANY
        }

        private List<IThing> things;
        private float weight;
        private float maxCapacity;

        public float Weight
        {
            get
            {
                return weight;
            }
        }

        public float Count
        {
            get
            {
                return things.Count;
            }
        }

        public int FoodCount
        {
            get
            {
                return things.Count(t => t.GetType().Name == "Food");
            }
        }

        public int ProductCount
        {
            get
            {
                var b = things[0].GetType();
                var m = b.Name == "Food";
                var l = b.Name == "Product";
                return things.Count(t => t.GetType().Name == "Product");
            }
        }

        public float MaxCapacity
        {
            get
            {
                return maxCapacity;
            }
            //if setted MaxCapacity smaller then current weight - drop some things
            set
            {
                while (weight > value)
                {
                    DropRandom();
                }   
                maxCapacity = value;
            }
        }

        public Bag(float maxCapacity_)
        {
            weight = 0;
            maxCapacity = maxCapacity_;
            things = new List<IThing>();
        }

        public Bag()
        {
            weight = 0;
            maxCapacity = 0;
            things = new List<IThing>();
        }

        public void Add(IThing th)
        {
            if (weight + th.Weight > maxCapacity)
            {
                throw new OverloadedBagExeption();
            } else
            {
                things.Add(th);
                weight += th.Weight;
            }
        }

        public IFood GetFood()
        {
            for (int i = 0; i < things.Count; i++)
            {
                if (things[i] is IFood)
                {
                    IFood f = things[i] as IFood;
                    things.RemoveAt(i);
                    return f;
                }
            }
            throw new NoFoodExeption();
        }

        public IProduct GetProduct()
        {
            for (int i = 0; i < things.Count; i++)
            {
                if (things[i] is IProduct)
                {
                    IProduct p = things[i] as IProduct;
                    things.RemoveAt(i);
                    return p;
                }
            }
            throw new NoProductExeption();
        }

        public IThing DropRandom()
        {
            IThing thing = null;
            int l = things.Count;
            if (l != 0)
            {
                int i = Util.GetRandomFromInterval(0, l);
                thing = things.ElementAt(i);
                things.RemoveAt(i);
            }
            return thing;
        }

        public IThing GetWithPriceLower(float price, float percect, TownInterfaces.ThingType type)
        {
            IThing thing;
            if (type == TownInterfaces.ThingType.ANY)
            {
                thing = things.Find(t => t.Price * (1 + percect) < price);
            }
            else
            {
                thing = things.Find(t => t.Price * (1 + percect) < price &&
                   ((t.GetType().Name == "Food" && type == TownInterfaces.ThingType.FOOD) ||
                   (t.GetType().Name == "Product" && type == TownInterfaces.ThingType.PRODUCT)));
            }

            if (things.Contains(thing))
            {
                things.Remove(thing);
                return thing;
            }
            return null;
        }
    }
}
