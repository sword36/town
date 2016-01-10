using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownInterfaces;

namespace townWinForm
{
    public class Tavern : Building, IEntertainment
    {
        public Tavern(int x, int y, int width, int height, string type) : base(x, y, width, height, type)
        {

        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);


        }

        public Tavern() { }
    }
}
