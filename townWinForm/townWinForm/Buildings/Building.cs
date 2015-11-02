using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public abstract class Building : IDrawable
    {
        protected static float dx = 0;
        protected static float dy = 0;
        public abstract void Draw(Graphics g);

        public static void UpdateD(float dx, float dy)
        {
            Building.dx = dx;
            Building.dy = dy;
        }
        public RectangleF Position { get; set; }
    }
}
