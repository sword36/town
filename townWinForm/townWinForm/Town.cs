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

        public void Update(long dt)
        {

        }

        public void Draw(Graphics g)
        {
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {

                    //if (Util.CheckPoint(new PointF(Config.TileSize * x + Config.dx, Config.TileSize * y + Config.dy)))
                    {
                        g.FillRectangle(new SolidBrush(Color.Blue), 50 * x + Config.dx, 50 * y + Config.dy, 50, 50);
                        g.DrawRectangle(Pens.Red, 50 * x + Config.dx, 50 * y + Config.dy, 50,50);
                    }
                }

            }
        }

        public static void UpdateD(float dx, float dy)
        {
            Town.dx = dx;
            Town.dy = dy;
        }
    }
}
