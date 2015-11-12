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
        private string buildingType;
        private static float dx;
        private static float dy;

        private int x;
        private int y;

        private bool Entrance;

        public Tile(int x, int y, string type, bool entrance = false)
        {
            buildingType = type;
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
                Color c;
                switch (buildingType)
                {
                    case "house":
                        {
                            c = Config.HouseColor;
                            break;
                        }
                    case "tavern":
                        {
                            c = Config.TavernColor;
                            break;
                        }
                    case "barracks":
                        {
                            c = Config.BarracksColor;
                            break;
                        }
                    case "market":
                        {
                            c = Config.MarketColor;
                            break;
                        }
                    case "guild":
                        {
                            c = Config.GuildColor;
                            break;
                        }
                    case "farm":
                        {
                            c = Config.FarmColor;
                            break;
                        }
                    case "factory":
                        {
                            c = Config.FactoryColor;
                            break;
                        }
                    default:
                        c = Color.Black;
                        break;

                }

                if (IsEntrance())
                    c = Color.LightGreen;

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
