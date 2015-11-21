using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace townWinForm
{
    public abstract class Building : IDrawable
    {
        private string buildingType = "";
        public Color BuildingColor = Color.Black;

        static Random rand = new Random(DateTime.Now.Millisecond);
        protected static int idCounter = 0;
        protected int id;

        private static float dx = 0;
        private static float dy = 0;

        //Building id
        public int Id
        {
            get { return id; }
        }

        private Bag chest;

        public Bag Chest
        {
            get { return chest; }
        }

        protected PointF entrance;
        protected PointF room;
        protected PointF localEntrance;
        protected List<PointF> OccupiedRooms;
        protected List<PointF> FreeRooms;
        protected List<Human> PeopleIn;

        //Entrance in town
        public virtual PointF Entrance
        {
            get { return entrance; }
        }

        public PointF GlobalEntrance
        {
            get { return new PointF(Position.Location.X + entrance.X, Position.Location.Y + entrance.Y); }
        }

        public PointF Room
        {
            get
            {
                PointF res = FreeRooms[rand.Next(FreeRooms.Count)];
                OccupiedRooms.Add(res);
                return res;
            }
        }

        //Left upper point's position
        public PointF LeftPosition
        {
            get { return Util.ConvertIndexToInt(Position.Location); }
        }

        public Rectangle Position { get; set; }

        public virtual void Draw(Graphics g)
        {

            g.FillRectangle(new SolidBrush(BuildingColor), Position.Location.X * Config.TileSize + dx,
                Position.Location.Y * Config.TileSize + dy, 
                Position.Width * Config.TileSize,
                Position.Height * Config.TileSize);

            g.FillRectangle(Brushes.LightGreen, (Entrance.X + Position.Location.X) * Config.TileSize + dx, (Entrance.Y + Position.Location.Y) * Config.TileSize + dy, Config.TileSize, Config.TileSize);
            g.DrawString(PeopleIn.Count.ToString(), SystemFonts.DialogFont, Brushes.Black, Position.Location.X * Config.TileSize + dx, Position.Location.Y * Config.TileSize + dy);
        }

        protected virtual void SetEntrance()
        {
            int side = rand.Next(0, 2);

            switch (side)
            {
                //UP, DOWN
                case 0:
                    {
                        bool up = rand.Next(0, 2) == rand.Next(0, 2);
                        if (up)
                        {
                            entrance = new PointF(Position.Location.X + rand.Next(1, Position.Width - 1), Position.Location.Y);
                        }
                        else
                        {
                            entrance = new PointF(Position.Location.X + rand.Next(1, Position.Width - 1), Position.Location.Y + Position.Height - 1);
                        }
                        break;
                    }

                //LEFT, RIGHT
                default:
                    {
                        bool left = rand.Next(0, 2) == rand.Next(0, 2);
                        if (left)
                        {
                            entrance = new PointF(Position.Location.X, Position.Location.Y + rand.Next(1, Position.Height - 1));
                        }
                        else
                        {
                            entrance = new PointF(Position.Location.X + Position.Width - 1, Position.Location.Y + rand.Next(1, Position.Height - 1));
                        }
                        break;
                    }
            }
            entrance = entrance - new Size(Position.Location.X, Position.Location.Y);
        }

        public void AddHuman(Human h)
        {
            PeopleIn.Add(h);
            if (h.Path.Count != 0)
            h.CurrentRoom = Util.ConvertIntToIndex(h.Path.Last());
        }

        public void RemoveHuman(Human h)
        {
            PeopleIn.Remove(h);
        }

        public Building(int x, int y, int width, int height, string type)
        {
            chest = new Bag(Config.BuildingBagCapacity);
            FreeRooms = new List<PointF>();
            OccupiedRooms = new List<PointF>();
            PeopleIn = new List<Human>();

            buildingType = type;
            id = ++idCounter;
            Point p = new Point(x, y);
            Size s = new Size(width, height);
            Position = new Rectangle(p, s);

            switch (buildingType)
            {
                case "house":
                    {
                        BuildingColor = Config.HouseColor;
                        break;
                    }

                case "tavern":
                    {
                        BuildingColor = Config.TavernColor;
                        break;
                    }

                case "barracks":
                    {
                        BuildingColor = Config.BarracksColor;
                        break;
                    }

                case "market":
                    {
                        BuildingColor = Config.MarketColor;
                        break;
                    }

                case "guild":
                    {
                        BuildingColor = Config.GuildColor;
                        break;
                    }

                case "farm":
                    {
                        BuildingColor = Config.FarmColor;
                        break;
                    }

                case "factory":
                    {
                        BuildingColor = Config.FactoryColor;
                        break;
                    }

                default:
                    BuildingColor = Color.Black;
                    break;

            }

            SetEntrance();

            for (int xx = 1; xx < Position.Width - 1; xx++)
            {
                for (int yy = 1; yy < Position.Height - 1; yy++)
                {
                    FreeRooms.Add(new PointF(xx + Position.Location.X, yy + Position.Location.Y));
                }
            }
        }

        public static void UpdateD(float dx, float dy)
        {
            Building.dx = dx;
            Building.dy = dy;
        }


        
    }
}
