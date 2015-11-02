using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class Town : IDrawable
    {
        private static float dx = 0;
        private static float dy = 0;

        public List<Human> Citizens;

        public Town()
        {
            Citizens = new List<Human>();
        }

        public void Update(int dt)
        {

        }

        public void Draw(Graphics g)
        {
            g.DrawRectangle(Pens.Red, 100 + dx, 100 + dy, 100, 100);
        }

        public static void UpdateD(float dx, float dy)
        {
            Town.dx = dx;
            Town.dy = dy;
        }
    }
}
