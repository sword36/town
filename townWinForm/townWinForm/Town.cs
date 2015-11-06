using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Web;
using System.Net;

namespace townWinForm
{
    public class Town : IDrawable
    {
        private int minStructCount = Config.MaxCitizens + (int)Math.Ceiling((float)(Config.MaxCitizens / 4)) + 4 + 1;

        private static float dx = 0;
        private static float dy = 0;

        private Point si = new Point(2, 2);
        private Point fi = new Point(Config.TownWidth - 3, Config.TownHeight - 3);

        private List<PointF> pathF;
        private List<Point> path;
        public Point MousePosition;
        public Point CurrentTile;

        public List<Human> Citizens;
        public List<Building> Structures;

        private int[,] matrix;
        private int[,] AstarMatrix;
        private Human h;

        private List<PointF> homeToWork = new List<PointF>();
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
            h = new Human(this);
            h.Home = Structures.ElementAt(0);
            h.WorkBuilding = Structures.ElementAt(Structures.Count - 1);
            h.Position = Util.ConvertIndexToInt(h.Home.Entrance);

            homeToWork = FindPath(Util.ConvertFromPointF(h.Position), h.WorkBuilding);

        }

        //Returns path from point start to finish building
        public List<PointF> FindPath(Point start, Building finish)
        {
            Point finishEntrance = Util.ConvertFromPointF(finish.Room);
            path = PathNode.FindPath(AstarMatrix, Util.ConvertIntToIndex(start), finishEntrance);

            List<PointF> finalPath = new List<PointF>();

            for (int i = 0; i < path.Count; i++)
            {
                PointF pathPoint = Util.ConvertIndexToInt(path[i]);
                finalPath.Add(pathPoint);
            }

            return finalPath;
        }

        /*public List<PointF> FindPath(PointF start, Building finish)
        {
            pathF = PathNode.FindPath(AstarMatrix, Util.ConvertIntToIndex(start), finish.Entrance);

            List<PointF> finalPath = new List<PointF>();

            for (int i = 0; i < pathF.Count; i++)
            {
                finalPath.Add(Util.ConvertIndexToInt(pathF[i]));
            }

            return finalPath;
        }*/

        //Initialization of matrix for A* algorithm
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
                            AstarMatrix[s.Position.X + x, s.Position.Y + y] = 2000;
                        }

                        if ((x != 0) && (y != 0) && (x != s.Position.Width - 1) && (y != s.Position.Height - 1))
                        {
                            AstarMatrix[s.Position.X + x, s.Position.Y + y] = 1;
                        }
                    }
                }
            }
        }


        //Buildings initialization
        private void InitBuildings()
        {
            Random rand = new Random();
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

                        if (idCounter.Contains(buildIndex))
                            continue;

                        while (matrix[x + w, y] == buildIndex)
                            w++;

                        while (matrix[x, y + h] == buildIndex)
                            h++;
                        
                        if ((rand.Next() % 6 != 0))
                        Structures.Add(new House(x, y, w, h));
                        idCounter.Add(buildIndex);
                    }
                    
                }
            }

            while (Structures.Count > minStructCount + 10)
            {
                Structures.RemoveAt(rand.Next(Structures.Count));
            }
        }

        //First matrix initialization
        private void MatrixInit()
        {
            for (int x = 0; x < Config.TownWidth; x++)
            {
                for (int y = 0; y < Config.TownHeight; y++)
                {
                    matrix[x, y] = 0;
                    AstarMatrix[x, y] = -1;
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
            {
                for (int i = 0; i < Config.TownWidth; i++)
                {
                    if (matrix[i, yy] == 0)
                    {
                        int w = rand.Next(Config.minBuildingSize, Config.maxBuildingSize);
                        int h = rand.Next(Config.minBuildingSize, Config.maxBuildingSize);

                        if (Config.TownWidth - i <= Config.StreetHeight + 11)
                        {
                            switch (Config.TownWidth - i)
                                {
                                case 24:
                                case 23:
                                case 16:
                                case 15:
                                case 14:
                                        {
                                        w = Config.maxBuildingSize;
                                            break;
                                        }

                                case 25:
                                case 22:
                                case 21:
                                case 20:
                                case 19:
                                case 18:
                                case 13:
                                case 12:
                                        {
                                        w = Config.minBuildingSize;
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
            h.Update((int)dt);
        }

        public void Draw(Graphics g)
        {
            
            for (int x = 0; x < Config.TownWidth; x++)
            {
                for (int y = 0; y < Config.TownHeight; y++)
                {

                    if (Util.CheckPoint(new PointF(Config.TileSize * x + Config.dx, Config.TileSize * y + Config.dy)))
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(250, 250, 250)), Config.TileSize * x + Config.dx, Config.TileSize * y + Config.dy, Config.TileSize, Config.TileSize);
                    }
                }
            }

            //foreach (var j in homeToWork)
           // {
            //    g.FillRectangle(new SolidBrush(Color.FromArgb(250, 0, 0)), j.X + Config.dx, j.Y + Config.dy, Config.TileSize, Config.TileSize);
            //
            //}

            foreach (var s in Structures)
            {
                s.Draw(g);
            }
            h.Draw(g);
        }

        public static void UpdateD(float dx, float dy)
        {
            Town.dx = dx;
            Town.dy = dy;
        }
    }
}
