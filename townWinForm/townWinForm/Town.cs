using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Web;
using System.Net;
using System.IO;

namespace townWinForm
{
    public class Town : IDrawable, IUpdatable
    {
        private int minStructCount = Config.Houses + Config.Productions +
             Config.Markets + Config.Taverns;


        private static float dx = 0;
        private static float dy = 0;

        private Random rand = new Random(DateTime.Now.Millisecond);

        private List<Point> path;
        public Point MousePosition;
        public Point CurrentTile;

        public List<Human> Citizens;
        private Dictionary<string, Image> CitizensInfo = new Dictionary<string, Image>();

        private List<Building> Structures;
        private List<Tavern> taverns;
        private List<Market> Markets;
        private List<Barracks> Rax;
        private List<IResidence> Houses;
        private List<IWorkshop> Workshops;
        private List<ThievesGuild> Guilds;

        private int[,] matrix;
        private int[,] AstarMatrix;

        private List<PointF> homeToWork = new List<PointF>();

        public List<Tavern> Taverns
        {
            get
            {
                return taverns;
            }
        }

        public Town()
        {
            SetTownSize();
            

            Structures = new List<Building>();
            taverns = new List<Tavern>();
            Rax = new List<Barracks>();
            Houses = new List<IResidence>();
            Workshops = new List<IWorkshop>();
            Guilds = new List<ThievesGuild>();
            Markets = new List<Market>();

            matrix = new int[Config.TownWidth, Config.TownHeight];
            AstarMatrix = new int[Config.TownWidth, Config.TownHeight];

            InitInfo();
            MatrixInit();
            CreateStreets();
            InitBuildings();
            InitAstarMatrix();
            InitPeople();
        }

        public KeyValuePair<string, Image> GetInfo()
        {
            int index = rand.Next(CitizensInfo.Count);

            KeyValuePair<string, Image> res = CitizensInfo.ElementAt(index);

            CitizensInfo.Remove(res.Key);

            return res;
        }

        public KeyValuePair<string, Image> GetInfo(int id)
        {
            switch (id)
            {
                case 0:
                    {
                        KeyValuePair<string, Image> res = new KeyValuePair<string, Image>("Андрей Ханин", CitizensInfo["Андрей Ханин"]);
                        CitizensInfo.Remove(res.Key);
                        return res;
                    }

                case 1:
                    {
                        KeyValuePair<string, Image> res = new KeyValuePair<string, Image>("Евгений Кашин", CitizensInfo["Евгений Кашин"]);
                        CitizensInfo.Remove(res.Key);
                        return res;
                    }

                case 2:
                    {
                        KeyValuePair<string, Image> res = new KeyValuePair<string, Image>("Дмитрий Хорощенко", CitizensInfo["Дмитрий Хорощенко"]);
                        CitizensInfo.Remove(res.Key);
                        return res;
                    }

                case 3:
                    {
                        KeyValuePair<string, Image> res = new KeyValuePair<string, Image>("Кирилл Зенин", CitizensInfo["Кирилл Зенин"]);
                        CitizensInfo.Remove(res.Key);
                        return res;
                    }

                case 4:
                    {
                        KeyValuePair<string, Image> res = new KeyValuePair<string, Image>("Алексей Нужных", CitizensInfo["Алексей Нужных"]);
                        CitizensInfo.Remove(res.Key);
                        return res;
                    }

                case 5:
                    {
                        KeyValuePair<string, Image> res = new KeyValuePair<string, Image>("Игорь Бруев", CitizensInfo["Игорь Бруев"]);
                        CitizensInfo.Remove(res.Key);
                        return res;
                    }

                case 6:
                    {
                        KeyValuePair<string, Image> res = new KeyValuePair<string, Image>("Олег Бердников", CitizensInfo["Олег Бердников"]);
                        CitizensInfo.Remove(res.Key);
                        return res;
                    }

                case 7:
                    {
                        KeyValuePair<string, Image> res = new KeyValuePair<string, Image>("Олег Андрияшкин", CitizensInfo["Олег Андрияшкин"]);
                        CitizensInfo.Remove(res.Key);
                        return res;
                    }

                default:
                    {
                        int index = rand.Next(CitizensInfo.Count);

                        KeyValuePair<string, Image> res = CitizensInfo.ElementAt(index);

                        CitizensInfo.Remove(res.Key);

                        return res;
                    }
            }
        }

        private void InitInfo()
        {
            FileStream fs = new FileStream("..\\..\\Photos\\Info.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            string name = sr.ReadLine();

            while (name != null)
            {
                Image image = Image.FromFile("..\\..\\Photos\\" + name + ".jpg");
                {
                    if (!CitizensInfo.ContainsKey(name))
                    CitizensInfo.Add(name, image);
                }
                name = sr.ReadLine();
            }

            sr.Close();
            fs.Close();
        }

        private void InitPeople()
        {
            Citizens = new List<Human>();
            for (int i = 0; i < Config.MaxCitizens; i++)
            {
                Human h = new Human(this);
                GetWorkshop(h.CurrentProf).AddWorker(h);
                GetHome().AddResident(h);
                h.Position = Util.ConvertIndexToInt(h.Home.Room);
                Citizens.Add(h);
            }
        }

        public IWorkshop GetGuild()
        {
            if (Guilds.Count == 0)
                return null;
            int index = 0;
            int n = Guilds[0].Workers.Count;
            for (int i = 1; i < Guilds.Count; i++)
            {
                n = Math.Min(n, Guilds[i].Workers.Count);
                if (n == Guilds[i].Workers.Count) index = i;
            }
            return Guilds[index];
        }

        public IWorkshop GetBarracks()
        {
            if (Rax.Count == 0)
                return null;
            int index = 0;
            int n = Rax[0].Workers.Count;
            for (int i = 1; i < Rax.Count; i++)
            {
                n = Math.Min(n, Rax[i].Workers.Count);
                if (n == Rax[i].Workers.Count) index = i;
            }
            return Rax[index];
        }



        public IWorkshop GetWorkshop(string prof)
        {
            switch (prof)
            {
                case "craftsman":
                    {
                        IWorkshop result = Workshops[rand.Next(Workshops.Count)];
                        while ((!result.IsFree()) || !(result is Factory))
                        {
                            result = Workshops[rand.Next(Workshops.Count)];
                        }
                        return result;
                    }

                case "farmer":
                    {
                        IWorkshop result = Workshops[rand.Next(Workshops.Count)];
                        while ((!result.IsFree()) || !(result is Farm))
                        {
                            result = Workshops[rand.Next(Workshops.Count)];
                        }
                        return result;
                    }

                case "trader":
                    {
                        return Markets[rand.Next(Markets.Count)];
                        /*IWorkshop result = Workshops[rand.Next(Workshops.Count)];
                        while ((!result.IsFree()) || !(result is Market))
                        {
                            result = Workshops[rand.Next(Workshops.Count)];
                        }
                        return result;*/
                    }

                case "thief":
                    {
                        return GetGuild();
                    }

                case "guardian":
                    {
                        return GetBarracks();
                    }

                default:
                    {
                        IWorkshop result = Workshops[rand.Next(Workshops.Count)];
                        while (!result.IsFree())
                        {
                            result = Workshops[rand.Next(Workshops.Count)];
                        }
                        return result;
                    }
            }
        }

        public IResidence GetHome()
        {
            IResidence result = Houses[rand.Next(Houses.Count)];
            while (!result.HavePlace())
            {
                result = Houses[rand.Next(Houses.Count)];
            }
            return result;
        }

        private void SetTownSize()
        {
            Random rand = new Random();
            int sign = Math.Sign(rand.Next(-10, 10));
            int n = 0;
            int avg = Config.StreetHeight / 2;

            while (n < minStructCount + minStructCount / 3)
            {
                if (sign > 0)
                    Config.Blocks++;

                sign *= -1;

                Config.TownWidth += avg;

                n = Config.Blocks * 2 * Config.TownWidth / avg;
            }
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

        public List<PointF> FindPath(Point start, IResidence finish)
        {
            return FindPath(start, finish as Building);
        }

        public List<PointF> FindPath(Point start, IWorkshop finish)
        {
            return FindPath(start, finish as Building);
        }

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
                        
                        if (s.Entrance == new PointF(x, y))
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
            List<int> addIdCounter = new List<int>();

            int farmCount = 0;
            int factoryCount = 0;

            for (int z = 0; z < 5; z++)
            {
                addIdCounter.Clear();
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

                            if (addIdCounter.Contains(buildIndex))
                                continue;

                            while (matrix[x + w, y] == buildIndex)
                                w++;

                            while (matrix[x, y + h] == buildIndex)
                                h++;

                            
                            

                            if ((rand.Next() % 4 == 0) && (w >= 5) && (h >= 5) 
                                && (Markets.Count < Config.Markets))
                            {
                                Workshops.Add(new Market(x, y, w , h, "market"));
                                Markets.Add(Workshops[Workshops.Count - 1] as Market);
                                Structures.Add(Workshops[Workshops.Count - 1] as Building);
                                idCounter.Add(buildIndex);
                                continue;
                            }

                            if ((rand.Next() % 4 == 0) && (Houses.Count < Config.Houses))
                            {
                                Houses.Add(new House(x, y, w, h, "house"));
                                Structures.Add(Houses[Houses.Count - 1] as Building);
                                idCounter.Add(buildIndex);
                                continue;
                            }

                            if ((rand.Next() % 3 == 0) && (Guilds.Count < Config.ThiefGuildsAmount) && (w >= 5) && (h >= 5))
                            {
                                Guilds.Add(new ThievesGuild(x, y, w, h, "guild"));
                                Structures.Add(Guilds[Guilds.Count - 1] as Building);
                                idCounter.Add(buildIndex);
                                continue;
                            }

                            if ((rand.Next() % 3 == 0) && (Rax.Count < Config.BarracksAmount) && (w >= 5) && (h >= 5))
                            {
                                Rax.Add(new Barracks(x, y, w, h, "barracks"));
                                Structures.Add(Rax[Rax.Count - 1]);
                                idCounter.Add(buildIndex);
                                continue;
                            }

                            if ((rand.Next() % 5 == 0) && (taverns.Count < Config.Taverns))
                            {
                                taverns.Add(new Tavern(x, y, w, h, "tavern"));
                                Structures.Add(taverns[taverns.Count - 1]);
                                idCounter.Add(buildIndex);
                                continue;
                            }

                            if (farmCount < factoryCount)
                            {
                                if (((rand.Next() % 2 == 0) || (rand.Next() % 10 == 0)) && (Workshops.Count < Config.Productions))
                                {
                                    farmCount++;
                                    Workshops.Add(new Farm(x, y, w, h, "farm"));
                                    Structures.Add(Workshops[Workshops.Count - 1] as Building);
                                    idCounter.Add(buildIndex);
                                    continue;
                                }
                            }
                            else
                            { 
                                if (((rand.Next() % 2 == 0) || (rand.Next() % 10 == 0)) && (Workshops.Count < Config.Productions))
                                {
                                    factoryCount++;
                                    Workshops.Add(new Factory(x, y, w, h, "factory"));
                                    Structures.Add(Workshops[Workshops.Count - 1] as Building);
                                    idCounter.Add(buildIndex);
                                    continue;
                                }
                            }
                            addIdCounter.Add(buildIndex);

                        }

                    }
                }
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

        public void Update(int dt)
        {
            foreach (Human h in Citizens)
            {
                h.Update(dt);
            }
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Config.StreetColor), dx, dy, Config.TownWidth * Config.TileSize, Config.TownHeight * Config.TileSize);

            foreach (var s in Structures)
            {
                s.Draw(g);
            }

            foreach (Human h in Citizens)
            {
                h.Draw(g);
            }
        }

        public static void UpdateD(float dx, float dy)
        {
            Town.dx = dx;
            Town.dy = dy;
        }

        public Tavern GetTavern()
        {
            return taverns[rand.Next(taverns.Count)];
        }

        public Building IsHumanInBuilding(Human h)
        {
            for (int i = 0; i < Structures.Count; i++)
            {
                if (Util.IsInRectangle(h.Position, Structures[i].Position))
                    return Structures[i];
            }
            return null;
        }

        public Human IsMouseOnHuman(Point p)
        {
            for (int i = 0; i < Citizens.Count; i++)
            {
                if (Util.IsPointInRectangle(p, new RectangleF(Citizens[i].Position, new SizeF(Config.TileSize, Config.TileSize))))
                    return Citizens[i];
            }
            return null;
        }

    }
}
