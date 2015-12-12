using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class Market : Workshop
    {
        public Market(int x, int y, int width, int height, string type) : base(x, y, width, height, type)
        {
            workers = new List<Human>();

        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }

        public static bool operator <(Market w1, Market w2)
        {
            return w1.Count < w2.Count;
        }

        public static bool operator >(Market w1, Market w2)
        {
            return w1.Count > w2.Count;
        }
    }
}
