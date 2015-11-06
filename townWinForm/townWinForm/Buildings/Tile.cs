using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class Tile : IDrawable
    {
        private static float dx;
        private static float dy;

        private int x;
        private int y;

        private bool Entrance;

        public Tile(int x, int y, bool entrance = false)
        {
            Entrance = entrance;
            this.x = x;
            this.y = y;
        }

        public void SetEntrance()
        {
            Entrance = true;
        }

        public bool IsEntrance()
        {
            return Entrance;
        }

        public void Draw(Graphics g)
        {
            if (Util.CheckPoint(new PointF(Config.TileSize * x + Config.dx, Config.TileSize * y + Config.dy)))
            {
                Color c = Color.FromArgb(20, 20, 20);
                if (Entrance)
                    c = Color.LawnGreen;

                g.FillRectangle(new SolidBrush(c), x * Config.TileSize + dx, y * Config.TileSize + dy, Config.TileSize, Config.TileSize);
            }
        }

        public static void UpdateD(float dx, float dy)
        {
            Tile.dx = dx;
            Tile.dy = dy;
        }



    }
}
