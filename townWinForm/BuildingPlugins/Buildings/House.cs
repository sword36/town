using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TownInterfaces;

namespace BuildingPlugins
{
    public class House : Residence, INotIgnore
    {


        public House(int x, int y, int width, int height, string type) : base(x, y, width, height, type)
        {
            residents = new List<TownInterfaces.ICitizen>();
        }

        public House() { }
    }
}
