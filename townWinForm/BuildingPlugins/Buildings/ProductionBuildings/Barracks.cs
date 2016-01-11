using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TownInterfaces;

namespace BuildingPlugins
{
    public class Barracks : CombinedBuilding, IBarracks, INotIgnore
    {
        public Barracks(int x, int y, int width, int height, string type) : base(x, y, width, height, type)
        {
            residents = new List<ICitizen>();
            workers = new List<ICitizen>();
        }

        public Barracks() { }

        public static bool operator <(Barracks w1, Barracks w2)
        {
            return w1.Count < w2.Count;
        }

        public static bool operator >(Barracks w1, Barracks w2)
        {
            return w1.Count > w2.Count;
        }
    }
}
