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
        public List<Building> Structures;

        private int[,] matrix;
        private int[,] AstarMatrix;

        public Town()
        {
            Citizens = new List<Human>();
            Structures = new List<Building>();
            matrix = new int[Config.TownWidth, Config.TownHeight];
            AstarMatrix = new int[Config.TownWidth, Config.TownHeight];
            MatrixInit();
            CreateStreets();
            InitBuildings();
            InitAstarMatrix();

        }

        public void InitAstarMatrix()
        {
            for (int x = 0; x < Config.TownWidth; x++)
            {
                for (int y = 0; y < Config.TownHeight; y++)
                {
                    AstarMatrix[x, y] = 1;
                }
            }

            foreach (var s in Structures)
            {
                for (int x = 0; x < s.Position.Width; x++)
                {
                    for (int y = 0; y < s.Position.Height; y++)
                    {
                        
                        if (s.LocalEntrance == new PointF(x, y))
                        {
                            AstarMatrix[s.Position.X + x, s.Position.Y + y] = 1;
                        }
                        else
                        {
                            AstarMatrix[s.Position.X + x, s.Position.Y + y] = int.MaxValue;
                        }

                        if ((x != 0) && (y != 0) && (x != s.Position.Width - 1) && (y != s.Position.Height - 1))
                        {
                            AstarMatrix[s.Position.X + x, s.Position.Y + y] = 1;
                        }
                    }
                }
            }
        }

        private void InitBuildings()
        {
            List<int> idCounter = new List<int>();
            for (int x = 0; x < Config.TownWidth; x++)
            {
                
                for (int y = 0; y < Config.TownHeight; y++)
                {
                    if (matrix[x, y] != 0)
                    {
                        int w = 0;
                        int h = 0;
                        int buildIndex = matrix[x, y];

                        if (Util.IsInList(idCounter, buildIndex))
                            continue;

                        while (matrix[x + w, y] == buildIndex)
                            w++;

                        while (matrix[x, y + h] == buildIndex)
                            h++;

                        Structures.Add(new House(x, y, w, h));
                        idCounter.Add(buildIndex);
                    }
                    
                }
            }
        }

        private void MatrixInit()
        {
            for (int x = 0; x < Config.TownWidth; x++)
            {
                for (int y = 0; y < Config.TownHeight; y++)
                {
                    matrix[x, y] = 0;
                    AstarMatrix[x, y] = 2;
                }
            }

            Random rand = new Random(DateTime.Now.Millisecond);

            int buildIndex = 1;
            int[,] buildingCounter = new int[6, 6];

            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    buildingCounter[x, y] = 0;
                }
            }

            for (int yy = 0; yy < Config.TownHeight; yy += Config.StreetHeight)

            for (int i = 0; i < Config.TownWidth; i++)
            {
                //for (int j = 0; j < Config.TownHeight; j++)
                {
                    if (matrix[i, yy] == 0)
                    {
                        int w = rand.Next(5, 7);
                        int h = rand.Next(5, 7);

                        if (Config.TownWidth - i <= 11)
                        {
                            switch (Config.TownWidth - i)
                                {
                                    case 10:
                                        {
                                            w = 5;
                                            break;
                                        }
                                    case 11:
                                        {
                                            w = 6;
                                            break;
                                        }
                                    default:
                                        {
                                            w = Config.TownWidth - i;
                                            break;
                                        }
                                }
                        }

                        for (int x = 0; x < w; x++)
                        {
                            for (int y = 0; y < h; y++)
                            {
                                matrix[i + x, yy + y] = buildIndex;
                                //matrix[i + x, y + h] = buildIndex + 1;
                            }

                            for (int y = h; y < Config.StreetHeight; y++)
                            {
                                matrix[i + x, yy + y] = buildIndex + 1;
                            }
                        }

                        buildIndex += 2;
                    }
                }
            }

        }

        private void CreateStreets()
        {
            for (int x = 0; x < Config.TownWidth; x++)
            {
                AstarMatrix[x, 0] = 0;
                AstarMatrix[x, Config.TownHeight - 1] = 0;
                for (int y = 0; y < Config.TownHeight; y++)
                {
                    AstarMatrix[0, y] = 0;
                    AstarMatrix[Config.TownWidth - 1, y] = 0;

                    if (x != Config.TownWidth - 1)
                    if (matrix[x, y] != matrix[x + 1, y])
                    {
                        AstarMatrix[x, y] = 0;
                        AstarMatrix[x + 1, y] = 0;
                    }

                    if (y != Config.TownHeight - 1)
                    if (matrix[x, y] != matrix[x, y + 1])
                    {
                        AstarMatrix[x, y] = 0;
                        AstarMatrix[x, y + 1] = 0;
                    }
                }
            }

            for (int x = 0; x < Config.TownWidth; x++)
            {
                for (int y = 0; y < Config.TownHeight; y++)
                {
                    if (AstarMatrix[x, y] == 0)
                    matrix[x, y] = AstarMatrix[x, y];
                }
            }
        }

        public void Update(long dt)
        {

        }

        public void Draw(Graphics g)
        {
            /*for (int x = 0; x < Config.TownWidth; x++)
            {
                for (int y = 0; y < Config.TownHeight; y++)
                {

                    if (Util.CheckPoint(new PointF(Config.TileSize * x + Config.dx, Config.TileSize * y + Config.dy)))
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(250, 250, 250)), Config.TileSize * x + Config.dx, Config.TileSize * y + Config.dy, Config.TileSize, Config.TileSize);
                        g.DrawRectangle(Pens.Red, Config.TileSize * x + Config.dx, Config.TileSize * y + Config.dy, Config.TileSize, Config.TileSize);
                    }
                }

            }

            foreach (var s in Structures)
            {
                s.Draw(g);
            }*/

            for (int x = 0; x < Config.TownWidth; x++)
            {
                for (int y = 0; y < Config.TownHeight; y++)
                {
                    g.FillRectangle(new SolidBrush(Util.GetRandomColor(AstarMatrix[x, y])), Config.TileSize * x + Config.dx, Config.TileSize * y + Config.dy, Config.TileSize, Config.TileSize);
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
