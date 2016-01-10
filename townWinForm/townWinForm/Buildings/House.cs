using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TownInterfaces;

namespace townWinForm
{
    public class House : Residence
    {
        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }

        public House(int x, int y, int width, int height, string type) : base(x, y, width, height, type)
        {
            residents = new List<TownInterfaces.ICitizen>();
        }

        public House() { }
    }
}
