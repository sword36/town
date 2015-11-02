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

        private int[,] matrix;

        public Town()
        {
            Citizens = new List<Human>();
            matrix = new int[Config.TownWidth, Config.TownHeight];

        }

        private void MatrixInit()
        {
            for (int x = 0; x < Config.TownWidth; x++)
            {
                for (int y = 0; y < Config.TownHeight; y++)
                {
                    matrix[x, y] = 0;
                }
            }

            Random rand = new Random(DateTime.Now.Millisecond);



        }

        public void Update(long dt)
        {

        }

        public void Draw(Graphics g)
        {
            for (int x = 0; x < Config.TownWidth; x++)
            {
                for (int y = 0; y < Config.TownHeight; y++)
                {

                    if (Util.CheckPoint(new PointF(Config.TileSize * x + Config.dx, Config.TileSize * y + Config.dy)))
                    {
                        g.FillRectangle(new SolidBrush(Color.Gainsboro), Config.TileSize * x + Config.dx, Config.TileSize * y + Config.dy, Config.TileSize, Config.TileSize);
                        g.DrawRectangle(Pens.Red, Config.TileSize * x + Config.dx, Config.TileSize * y + Config.dy, Config.TileSize, Config.TileSize);
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
