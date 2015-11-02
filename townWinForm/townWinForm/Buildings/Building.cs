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
        protected static int idCounter = 0;
        protected int id;

        public Point Position;

        private static float dx = 0;
        private static float dy = 0;
        public abstract void Draw(Graphics g);

        public Building(int x, int y)
        {
            id = ++idCounter;
            Position = new Point(x, y);
        }

        public Building() { }

        public static void UpdateD(float dx, float dy)
        {
            Building.dx = dx;
            Building.dy = dy;
        }
        //public RectangleF Position { get; set; }
    }
}
