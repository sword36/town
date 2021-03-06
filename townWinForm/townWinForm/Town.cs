﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Web;
using System.Net;
using System.IO;
using TownInterfaces;
using System.Reflection;

namespace townWinForm
{
    public class Town : IDrawable, IUpdatable, ITown
    {
        private int minStructCount = Config.Houses + Config.Productions +
             Config.Markets + Config.Taverns;

        double avgHappiness;

        public double AverageHappiness
        {
            get { return avgHappiness; }
        }


        private static float dx = 0;
        private static float dy = 0;

        private Random rand = new Random(DateTime.Now.Millisecond);

        private List<Point> path = new List<Point>();
        public Point MousePosition { get; set; }
        public Point CurrentTile { get; set; }

        public List<ICitizen> Citizens { get; set; }
        private Dictionary<string, Image> CitizensInfo = new Dictionary<string, Image>();

        private List<IBuilding> structures;
        private List<IEntertainment> taverns;
        private List<IWorkshop> markets;
        private List<IWorkshop> barracks;
        private List<IResidence> houses;
        private List<IWorkshop> workshops;
        private List<IWorkshop> factories;
        private List<IWorkshop> farms;
        private List<IWorkshop> guilds;

        public int[,] matrix { get; set; }
        public int[,] AstarMatrix { get; set; }

        //private List<PointF> homeToWork = new List<PointF>();

        public List<IEntertainment> Taverns
        {
            get
            {
                return taverns;
            }
            set { taverns = value; }
        }

        public ICitizen god { get; set; }

        public ICitizen God
        {
            get
            {
                return god;
            }
            set { god = value; }
        }

        public Town()
        {
            SetTownSize();

            structures = new List<IBuilding>();
            taverns = new List<IEntertainment>();
            barracks = new List<IWorkshop>();
            houses = new List<IResidence>();
            workshops = new List<IWorkshop>();
            factories = new List<IWorkshop>();
            farms = new List<IWorkshop>();
            guilds = new List<IWorkshop>();
            markets = new List<IWorkshop>();

            matrix = new int[Config.TownWidth, Config.TownHeight];
            AstarMatrix = new int[Config.TownWidth, Config.TownHeight];

            InitInfo();
            MatrixInit();
            CreateStreets();
            InitBuildings();
            InitAstarMatrix();
            InitPeople();
            createGod();
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
                case 8:
                    {
                        KeyValuePair<string, Image> res = new KeyValuePair<string, Image>("Маша Заволокина", CitizensInfo["Маша Заволокина"]);
                        CitizensInfo.Remove(res.Key);
                        return res;
                    }
                case 9:
                    {
                        KeyValuePair<string, Image> res = new KeyValuePair<string, Image>("Виктория Синьчук", CitizensInfo["Виктория Синьчук"]);
                        CitizensInfo.Remove(res.Key);
                        return res;
                    }
                case 10:
                    {
                        KeyValuePair<string, Image> res = new KeyValuePair<string, Image>("Ирина Серикова", CitizensInfo["Ирина Серикова"]);
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

        private void createGod()
        {
            god = new Human(this);
            god.Money = float.MaxValue;
            god.Bag.MaxCapacity = float.MaxValue;
            god.Happiness = float.MaxValue;
            god.Energy = float.MaxValue;
            god.Name = "GOD";
        }

        private void InitPeople()
        {
            Citizens = new List<ICitizen>();
            for (int i = 0; i < Config.MaxCitizens; i++)
            {
                ICitizen h = new Human(this);
                IWorkshop w = GetWorkshop(h.CurrentProf);
                w.AddWorker(h);
                //sortWorkshops();
                GetHome().AddResident(h);
                h.Position = Util.ConvertIndexToInt(h.Home.Room);
                Citizens.Add(h);
            }
        }

        public IWorkshop GetNearestMarket(ICitizen h)
        {
            return markets.OrderBy(m => Util.Distance(m.Entrance, h.Position)).ElementAt(0);
        }

        private void sortWorkshops()
        {
            /*markets.Sort(new Comparison<IWorkshop>((m1, m2) => 
            {
                if (m1.Workers.Count > m2.Workers.Count) return 1;
                if (m1.Workers.Count < m2.Workers.Count) return -1;
                return 0;
            }));

            guilds.Sort(new Comparison<IWorkshop>((m1, m2) =>
            {
                if (m1.Workers.Count > m2.Workers.Count) return 1;
                if (m1.Workers.Count < m2.Workers.Count) return -1;
                return 0;
            }));

            barracks.Sort(new Comparison<IWorkshop>((m1, m2) =>
            {
                if (m1.Workers.Count > m2.Workers.Count) return 1;
                if (m1.Workers.Count < m2.Workers.Count) return -1;
                return 0;
            }));

            farms.Sort(new Comparison<IWorkshop>((m1, m2) =>
            {
                if (m1.Workers.Count > m2.Workers.Count) return 1;
                if (m1.Workers.Count < m2.Workers.Count) return -1;
                return 0;
            }));

            factories.Sort(new Comparison<IWorkshop>((m1, m2) =>
            {
                if (m1.Workers.Count > m2.Workers.Count) return 1;
                if (m1.Workers.Count < m2.Workers.Count) return -1;
                return 0;
            }));*/
        }

        public IWorkshop GetWorkshop(string prof)
        {
            switch (prof)
            {
                case "craftsman":
                    {
                        return factories[0];
                        
                    }

                case "farmer":
                    {
                        return farms[0];
                        
                    }

                case "trader":
                    {
                        return markets[0];
                    }

                case "thief":
                    {
                        return guilds[0];
                        
                    }

                case "guardian":
                    {
                        return barracks[0];
                        
                    }

                default:
                    {
                        IWorkshop result = workshops[rand.Next(workshops.Count)];
                        while (!result.IsFree())
                        {
                            result = workshops[rand.Next(workshops.Count)];
                        }
                        return result;
                    }
            }
        }

        public IResidence GetHome()
        {
            IResidence result = houses[rand.Next(houses.Count)];
            while (!result.HavePlace())
            {
                result = houses[rand.Next(houses.Count)];
            }
            return result;
        }

        public PointF GetRandomStreetPoint()
        {
            int x = rand.Next(Config.TownWidth);
            int y = rand.Next(Config.TownHeight);

            while (AstarMatrix[x, y] != 1)
            {
                x = rand.Next(Config.TownWidth);
                y = rand.Next(Config.TownHeight);
            }

            return new PointF(x, y); //return Util.ConvertIndexToInt(new PointF(x, y));
        }

        public PointF GetRandomStreetPoint(PointF startPoint, float distance)
        {
            int x = rand.Next(Config.TownWidth);
            int y = rand.Next(Config.TownHeight);

            while ((AstarMatrix[x, y] != 1) || (Util.Distance(startPoint, Util.ConvertIndexToInt(new PointF(x, y)))) < distance)
            {
                x = rand.Next(Config.TownWidth);
                y = rand.Next(Config.TownHeight);
            }

            return new PointF(x, y);//Util.ConvertIndexToInt(new PointF(x, y));
        }

        private void SetTownSize()
        {

            Random rand = new Random();
            int sign = Math.Sign(rand.Next(-10, 10));
            while (sign == 0)
                sign = Math.Sign(rand.Next(-10, 10));
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

        public List<PointF> FindPath(Point start, PointF finish)
        {
            Point finishEntrance = Util.ConvertFromPointF(finish);
            path = PathNode.FindPath(AstarMatrix, Util.ConvertIntToIndex(start), finishEntrance);

            List<PointF> finalPath = new List<PointF>();

            for (int i = 0; i < path.Count; i++)
            {
                PointF pathPoint = Util.ConvertIndexToInt(path[i]);
                finalPath.Add(pathPoint);
            }

            return finalPath;
        }

        //Returns path from point start to finish building
        public List<PointF> FindPath(Point start, IBuilding finish)
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
            return FindPath(start, finish as IBuilding);
        }

        public List<PointF> FindPath(Point start, IWorkshop finish)
        {
            return FindPath(start, finish as IBuilding);
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

            foreach (var s in structures)
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
                            AstarMatrix[s.Position.X + x, s.Position.Y + y] = 2;
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
                                && (markets.Count < Config.Markets))
                            {
                                dynamic a = Activator.CreateInstance(Config.Marketplaces[rand.Next(0, Config.Marketplaces.Count)]);
                                //dynamic a = Activator.CreateInstance(typeof(IBuilding));

                                workshops.Add(a);
                                workshops.Last().Init(x, y, w, h, "market");

                                markets.Add(workshops[workshops.Count - 1] as IWorkshop);
                                structures.Add(workshops[workshops.Count - 1] as IBuilding);
                                idCounter.Add(buildIndex);
                                continue;
                            }

                            if ((rand.Next() % 4 == 0) && (houses.Count < Config.Houses))
                            {
                                dynamic a = Activator.CreateInstance(Config.Residences[rand.Next(0, Config.Residences.Count)]);

                                houses.Add(a);
                                houses.Last().Init(x, y, w, h, "house");

                                structures.Add(houses[houses.Count - 1] as IBuilding);
                                idCounter.Add(buildIndex);
                                continue;
                            }

                            if ((rand.Next() % 3 == 0) && (guilds.Count < Config.ThiefGuildsAmount) && (w >= 5) && (h >= 5))
                            {

                                dynamic a = Activator.CreateInstance(Config.Guilds[rand.Next(0, Config.Guilds.Count)]);

                                guilds.Add(a);
                                guilds.Last().Init(x, y, w, h, "guild");

                                
                                structures.Add(guilds[guilds.Count - 1] as IBuilding);
                                idCounter.Add(buildIndex);
                                continue;
                            }

                            if ((rand.Next() % 3 == 0) && (barracks.Count < Config.BarracksAmount) && (w >= 5) && (h >= 5))
                            {
                                dynamic a = Activator.CreateInstance(Config.Barracks[rand.Next(0, Config.Barracks.Count)]);

                                barracks.Add(a);
                                barracks.Last().Init(x, y, w, h, "barracks");

                                structures.Add(barracks[barracks.Count - 1]);
                                idCounter.Add(buildIndex);
                                continue;
                            }

                            if ((rand.Next() % 5 == 0) && (taverns.Count < Config.Taverns))
                            {
                                dynamic a = Activator.CreateInstance(Config.Entertainments[rand.Next(0, Config.Entertainments.Count)]);

                                taverns.Add(a);
                                taverns.Last().Init(x, y, w, h, "tavern");


                                structures.Add(taverns[taverns.Count - 1]);
                                idCounter.Add(buildIndex);
                                continue;
                            }

                            if (farmCount < factoryCount)
                            {
                                if (((rand.Next() % 2 == 0) || (rand.Next() % 10 == 0)) && (workshops.Count < Config.Productions))
                                {
                                    farmCount++;

                                    dynamic a = Activator.CreateInstance(Config.FoodProductions[rand.Next(0, Config.FoodProductions.Count)]);

                                    workshops.Add(a);
                                    workshops.Last().Init(x, y, w, h, "farm");

                                    farms.Add(workshops.Last() as IWorkshop);
                                    structures.Add(workshops[workshops.Count - 1] as IBuilding);
                                    idCounter.Add(buildIndex);
                                    continue;
                                }
                            }
                            else
                            { 
                                if (((rand.Next() % 2 == 0) || (rand.Next() % 10 == 0)) && (workshops.Count < Config.Productions))
                                {
                                    factoryCount++;

                                    dynamic a = Activator.CreateInstance(Config.Workshops[rand.Next(0, Config.Workshops.Count)]);

                                    workshops.Add(a);
                                    workshops.Last().Init(x, y, w, h, "factory");

                                    
                                    
                                    structures.Add(workshops[workshops.Count - 1] as IBuilding);
                                    factories.Add(structures[structures.Count - 1] as IWorkshop);
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
            /*for (int x = 0; x < Config.TownWidth; x++)
            {
                for (int y = 0; y < Config.TownHeight; y++)
                {
                    matrix[x, y] = 0;
                    AstarMatrix[x, y] = -1;
                }
            }*/

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
            double happiness = 0;
            foreach (ICitizen h in Citizens)
            {
                happiness += h.Happiness;
                h.Update(dt);
            }
            avgHappiness = happiness / Citizens.Count;
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Config.StreetColor), dx, dy, Config.TownWidth * Config.TileSize, Config.TownHeight * Config.TileSize);

            foreach (var s in structures)
            {
                s.Draw(g, Config.dx, Config.dy);
            }

            foreach (ICitizen h in Citizens)
            {
                h.Draw(g);
            }
        }

        public static void UpdateD(float dx, float dy)
        {
            Town.dx = dx;
            Town.dy = dy;
        }

        public IEntertainment GetTavern()
        {
            return taverns[rand.Next(taverns.Count)];
        }

        public void Taxes()
        {
            for (int i = 0; i < Citizens.Count; i++)
            {
                god.Money = Citizens[i].Tax;

            }
        }

        public void GuariansPayment()
        {
            for (int i = 0; i < Citizens.Count; i++)
            {
                if (Citizens[i].CurrentProf == "guardian")
                {
                    float payment = 1000f * (1 + Citizens[i].CurrentLevel / Config.MaxLevel);
                    god.Money -= payment;
                    Citizens[i].Money += payment;
                    Log.Add("citizens:Guardian " + Citizens[i].Name + " got payment " + payment.ToString());
                }
            }
        }

        public IBuilding IsHumanInBuilding(ICitizen h)
        {
            for (int i = 0; i < structures.Count; i++)
            {
                if (Util.IsInRectangle(h.Position, structures[i].Position))
                    return structures[i];
            }
            return null;
        }

        public ICitizen IsMouseOnHuman(Point p)
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