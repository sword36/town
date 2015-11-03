using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm
{
    public class OverloadedBagExeption : Exception
    {

    }

    public class Bag
    {
        private List<Thing> things;
        private float weight;
        private float maxCapacity;

        public float Weight
        {
            get
            {
                return weight;
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
            things = new List<Thing>();
        }

        public Bag()
        {
            weight = 0;
            maxCapacity = 0;
            things = new List<Thing>();
        }

        public void Add(Thing th)
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

        public Food GetFood()
        {
            for (int i = 0; i < things.Count; i++)
            {
                if (things[i] is Food)
                {
                    Food f = things[i] as Food;
                    things.RemoveAt(i);
                    return f;
                }
            }
            return null;
        }

        public Product GetProduct()
        {
            for (int i = 0; i < things.Count; i++)
            {
                if (things[i] is Product)
                {
                    Product p = things[i] as Product;
                    things.RemoveAt(i);
                    return p;
                }
            }
            return null;
        }

        public void DropRandom()
        {
            int l = things.Count;
            if (l != 0)
            {
                int i = Util.GetRandomFromInterval(0, l);
                things.RemoveAt(i);
            }
        }
    }
}
