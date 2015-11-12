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
        static Random rand = new Random(DateTime.Now.Millisecond);
        protected static int idCounter = 0;
        protected int id;

        public int Id
        {
            get { return id; }
        }

        protected Tile[,] Grid;
        protected PointF entrance;
        protected PointF room;
        protected PointF localEntrance;

        //Entrance in town
        public virtual PointF Entrance
        {
            get { return entrance; }
        }

        public PointF Room
        {
            get { return room; }
        }

        //Entrance in building
        public virtual PointF LocalEntrance
        {
            get { return localEntrance; }
        }

        //Left upper point's position
        public PointF LeftPosition
        {
            get { return Util.ConvertIndexToInt(Position.Location); }
        }

        public Rectangle Position { get; set; }

        private static float dx = 0;
        private static float dy = 0;
        public virtual void Draw(Graphics g)
        {

            for (int x = 0; x < Position.Size.Width; x++)
            {
                for (int y = 0; y < Position.Size.Height; y++)
                {
                    Grid[x, y].Draw(g);
                }
            }
        }

        public Building(int x, int y, int width, int height, string type)
        {
            id = ++idCounter;
            Point p = new Point(x, y);
            Size s = new Size(width, height);
            Position = new Rectangle(p, s);

            Grid = new Tile[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    Grid[i, k] = new Tile(x + i, y + k, type);
                }
            }

            int side = rand.Next(0, 2);

            switch (side)
            {
                //UP, DOWN
                case 0:
                    {
                        bool up = rand.Next(0, 2) == rand.Next(0, 2);
                        if (up)
                        {
                            Grid[rand.Next(1, width - 1), 0].SetEntrance();
                        }
                        else
                        {
                            Grid[rand.Next(1, width - 1), height - 1].SetEntrance();
                        }
                        break;
                    }

                //LEFT, RIGHT
                default:
                    {
                        bool left = rand.Next(0, 2) == rand.Next(0, 2);
                        if (left)
                        {
                            Grid[0, rand.Next(1, height - 1)].SetEntrance();
                        }
                        else
                        {
                            Grid[width - 1, rand.Next(1, height - 1)].SetEntrance();
                        }
                        break;
                    }
            }

            room = new PointF(Position.X + 1, Position.Y + 1);

            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    if (Grid[i, k].IsEntrance())
                    {
                        entrance = new PointF(i + Position.X, k + Position.Y);
                        localEntrance = new PointF(i, k);
                        return;
                    }
                }
            }
        }

        public Building() { }

        public static void UpdateD(float dx, float dy)
        {
            Building.dx = dx;
            Building.dy = dy;
        }


        
    }
}
