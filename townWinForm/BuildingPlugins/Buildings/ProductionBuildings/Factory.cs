using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TownInterfaces;

namespace BuildingPlugins
{
    public class Factory : Workshop, INotIgnore
    {
        public Factory(int x, int y, int width, int height, string type) : base(x, y, width, height, type)
        {
            workers = new List<ICitizen>();
        }

        public Factory() { }

        public static bool operator <(Factory w1, Factory w2)
        {
            return w1.Count < w2.Count;
        }

        public static bool operator >(Factory w1, Factory w2)
        {
            return w1.Count > w2.Count;
        }
    }
}
