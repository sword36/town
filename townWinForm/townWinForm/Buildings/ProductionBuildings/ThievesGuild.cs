using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class ThievesGuild : CombinedBuilding
    {
        public ThievesGuild(int x, int y, int width, int height, string type) : base(x, y, width, height, type)
        {
            residents = new List<Human>();
            workers = new List<Human>();
        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }

        
        public static bool operator <(ThievesGuild w1, ThievesGuild w2)
        {
            return w1.Count < w2.Count;
        }

        public static bool operator >(ThievesGuild w1, ThievesGuild w2)
        {
            return w1.Count > w2.Count;
        }
    }
}
