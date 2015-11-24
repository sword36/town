using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class Human : IDrawable, IUpdatable
    {
        
        public BehaviourModel Behaviour { get; set; }
        public PointF Position
        {
            get { return position; }
            set { position = value; }
        }
        private PointF position;
        public bool IsAlive { get; set; }
        public float Happiness { get; set; }
        public int Money { get; set; }
        public string CurrentProf { get; set; }
        public float Energy { get; set; }
        public IWorkshop WorkBuilding
        {
            get { return work; }
            set
            {
                if (work != null)
                    work.RemoveWorker(this);
                work = value;
            }
        }
        public Tavern FavoriteTavern { get; set; }
        public Bag Bag { get; set; }
        public float Speed { get; set; }
        public IResidence Home
        {
            get { return home; }
            set
            {
                if (home != null)
                    home.RemoveResident(this);
                home = value;
            }
        }

        protected Building currentBuilding;

        private int id;
        protected PointF currentRoom;

        public PointF CurrentRoom
        {
            get { return currentRoom; }
            set { currentRoom = value; }
        }

        public Dictionary<string, int> ProfLevels;
        private Dictionary<string, double> profExp;

        private IResidence home;
        private IWorkshop work;

        public static float dx = 0;
        public static float dy = 0;
        private int damage;
        private Human attackTarget = null;
        private Human activeTarget = null;
        private int waitTime = 0;
        private PointF tempTarget;
        private Town town;
        private List<PointF> path;
        private List<PointF> originalPath;

        public string Name;
        private Image img;


        public bool IsClicked
        {
            get; set;
        }

        public Human(Town t)
        {
            id = Util.GetNewID();
            town = t;

            
            KeyValuePair<string, Image> inf = town.GetInfo(id);

            Name = inf.Key;
            img = inf.Value;

            Money = Util.GetRandomDistribution(Config.StartMoney, Config.StartMoneyDelta);
            Happiness = Util.GetRandomDistribution(Config.StartHappiness, Config.StartHappinessDelta);
            Energy = 1;
            IsAlive = true;

            ProfLevels = new Dictionary<string, int>();
            profExp = new Dictionary<string, double>();

            Bag = new Bag();

            path = new List<PointF>();
            originalPath = new List<PointF>();
            
            currentBuilding = home as Building;

            //set all proffesion skills to 1 level
            foreach (string prof in Config.ProfList)
            {
                ProfLevels.Add(prof, 1);
            }

            foreach (string prof in Config.ProfList)
            {
                profExp.Add(prof, 0);
            }

            //random proffesion from Config.ProfList
            CurrentProf = Config.ProfList[Util.GetRandomFromInterval(0, Config.ProfList.Length - 1)];

            initBehaviourModel(CurrentProf); 

            Log.Add("citizens:Human " + Name + " created");

            Food f = new Food();
            Bag.Add(f);

            FavoriteTavern = town.GetTavern();
        }



        private void initBehaviourModel(string prof)
        {
            switch(prof)
            {
                case "craftsman":
                    Behaviour = new BehaviourModels.Craftsman(this, ProfLevels[prof]);
                    break;
                case "farmer":
                    Behaviour = new BehaviourModels.Farmer(this, ProfLevels[prof]);
                    break;
                case "guardian":
                    Behaviour = new BehaviourModels.Guardian(this, ProfLevels[prof]);
                    break;
                case "thief":
                    Behaviour = new BehaviourModels.Thief(this, ProfLevels[prof]);
                    break;
                case "trader":
                    Behaviour = new BehaviourModels.Trader(this, ProfLevels[prof]);
                    break;
                default: throw new Exception("Wrong proffession");
            }

            Log.Add("citizens:Human " + Name + " behaviour: " + prof);
        }

        public float Eat()
        {
            Food f = Bag.GetFood();
            if (f != null)
            {
                if (Energy + f.Energy <= Config.MaxEnergy)
                {
                    Energy += f.Energy;
                }
                return f.Energy;
            } else
            {
                return 0;
            }
        }

        public void Move(PointF p, int dt)
        {

        }

        public void Move(List<PointF> pN, int dt)
        {
            if (pN.Count != 0)
            {
                path = pN;
                tempTarget = path.First();
                path.RemoveAt(0);
                MoveAlongThePath(dt);
            } else //last point
            {
                MoveAlongThePath(dt);
            }
        }

        public bool MoveAlongThePath(int dt)
        {
            if (path == null)
            {
                throw new Exception("Wrong path");
            }

            if (Util.Distance(Position, tempTarget) < Config.MovePrecision)
            {
                Position = tempTarget;
                if (path.Count != 0)
                {
                    tempTarget = path.First();
                    path.RemoveAt(0);
                } else
                {
                    return true;
                }
            }
            moveToTempTarget(dt);
            return false;   
        }

        public void moveToTempTarget(int dt)
        {
            double angle = Math.Atan2(tempTarget.Y - Position.Y, tempTarget.X - Position.X);
            double dx = Speed * dt * Math.Cos(angle);
            double dy = Speed * dt * Math.Sin(angle);
            Position = new PointF(Position.X + (float)dx, Position.Y + (float)dy);
        }

        public void Move(RectangleF rect, int dt)
        {
            Move(new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2), dt);
        }

        public float DistanceToHome()
        {
            return Util.Distance(Home.Position, Position);
        }

        public float DistanceToWork()
        {
            return Util.Distance(WorkBuilding.Position, Position);
        }

        public void Attack(Human target)
        {
            attackTarget = target;
            Attack();
        }

        public void Attack()
        {
            if (attackTarget == null)
            {
                throw new Exception("Wrong target to attack");
            }

            attackTarget.TakeDamage(Damage);
        }

        public void TakeDamage(int dmg)
        {
            Energy -= dmg;

            if (Energy <= 0)
            {
                IsAlive = false;
            }
        }

        public int Damage
        {
            get
            {
                return damage;
            }

            set
            {
                damage = value;
            }
        }

        public List<PointF> Path
        {
            get
            {
                return path;
            }
        }

        public Town Town
        {
            get
            {
                return town;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public void Draw(Graphics g)
        {
            if (!Util.CheckPoint(Position))
                return;

            if (IsClicked)
            {
                g.FillRectangle(Brushes.Chartreuse, Position.X + dx,
                Position.Y + dy,
                Config.TileSize, Config.TileSize);

                DrawInfo(g);

            }

            else
                g.FillRectangle(new SolidBrush(Config.ProfColors[CurrentProf]), Position.X + dx,
                Position.Y + dy,
                Config.TileSize, Config.TileSize);

            if (img != null)
            g.DrawImage(img, position.X + 2 + dx, position.Y + 2 + dy, Config.TileSize - 4, Config.TileSize - 4);
        }

        private void DrawInfo(Graphics g)
        {
            using (Font f = new Font("Courier New", 12, FontStyle.Regular))
            {
                int step = 5;
                int barHeight = 20;

                int profPercent = (int)Math.Round((float)profExp[CurrentProf] / 
                    Config.exp[ProfLevels[CurrentProf] - 1] * 100);

                int energyPercent = (int)Math.Round(Energy / Config.MaxEnergy * 100);
                int happinessPercent = (int)Math.Round(Happiness / Config.MaxHappiness * 100);

                string expString = "XP " + profPercent.ToString() + "%";
                string energyString = "Energy " + energyPercent.ToString() + "%";
                string happinessString = "Happiness " + happinessPercent.ToString() + "%";
                string professionString = CurrentProf + " " + ProfLevels[CurrentProf] + " lvl";
                string stateString = Behaviour.State + " " + Money;

                SizeF profPercentSize = g.MeasureString(expString, f);
                SizeF energyPercentSize = g.MeasureString(energyString, f);
                SizeF happinessPercentSize = g.MeasureString(happinessString, f);
                SizeF nameSize = g.MeasureString(Name, f);
                SizeF profSize = g.MeasureString(professionString, f);
                SizeF stateSize = g.MeasureString(stateString, f);

                float width = Math.Max(Math.Max(nameSize.Width, happinessPercentSize.Width), Math.Max(profSize.Width, stateSize.Width));

                SolidBrush background = new SolidBrush(Color.FromArgb(100, 255, 255, 255));
                SolidBrush expBackground = new SolidBrush(Color.FromArgb(50, 255, 255, 0));
                SolidBrush energyBackground = new SolidBrush(Color.FromArgb(50, 255, 0, 0));
                SolidBrush happinessBackground = new SolidBrush(Color.FromArgb(50, 0, 255, 0));

                float drawingX = position.X + dx + Config.TileSize + step;


                g.FillRectangle(background,
                    Position.X + Config.TileSize + step + dx,
                    Position.Y + dy,
                    width + step * 2,
                    nameSize.Height + profSize.Height + stateSize.Height + step * 4 + barHeight * 3);

                g.DrawRectangle(new Pen(Color.FromArgb(100, 20, 20, 20), 2),
                    Position.X + Config.TileSize + step + dx,
                    Position.Y + dy,
                    width + step * 2,
                    nameSize.Height + profSize.Height + stateSize.Height + step * 4 + barHeight * 3);

                g.DrawString(Name, f, Brushes.Black, drawingX, position.Y + dy + step);

                g.DrawString(professionString, f,
                    Brushes.Black, drawingX, position.Y + nameSize.Height + dy + step);

                g.DrawString(stateString, f, 
                    Brushes.Black, drawingX, position.Y + nameSize.Height + stateSize.Height + dy + step);

                g.FillRectangle(expBackground, position.X + Config.TileSize + step * 2 + dx, 
                    position.Y + dy + step + profSize.Height + nameSize.Height + stateSize.Height, width, barHeight);

                g.FillRectangle(Brushes.Gold, position.X + Config.TileSize + step * 2 + dx, 
                    position.Y + dy + step + profSize.Height + nameSize.Height + stateSize.Height, 
                    (float)profExp[CurrentProf] / Config.exp[ProfLevels[CurrentProf] - 1] * width, barHeight);

                g.FillRectangle(energyBackground, position.X + Config.TileSize + step * 2 + dx, position.Y + dy + step + profSize.Height + stateSize.Height + barHeight + nameSize.Height + 5, width, barHeight);
                g.FillRectangle(Brushes.Red, position.X + Config.TileSize + step * 2 + dx, position.Y + dy + step + profSize.Height + nameSize.Height + stateSize.Height + barHeight + step, Energy / Config.MaxEnergy * width, barHeight);

                g.FillRectangle(happinessBackground, position.X + Config.TileSize + step * 2 + dx, 
                    position.Y + dy + profSize.Height + stateSize.Height + nameSize.Height + step * 3 + barHeight * 2,
                    width, barHeight);

                g.FillRectangle(Brushes.Chartreuse, position.X + Config.TileSize + step * 2 + dx, 
                    position.Y + dy + profSize.Height + stateSize.Height + nameSize.Height + step * 3 + barHeight * 2,
                    Happiness / Config.MaxHappiness * width, barHeight);

                g.DrawString(expString, f, Brushes.Black, drawingX + width / 2 - profPercentSize.Width / 2, 
                    position.Y + nameSize.Height + stateSize.Height + profSize.Height + dy + step);

                g.DrawString(energyString, f, Brushes.Black, drawingX + width / 2 - energyPercentSize.Width / 2, 
                    position.Y + step + nameSize.Height + stateSize.Height + profSize.Height + dy + step + barHeight);

                g.DrawString(happinessString, f, Brushes.Black, step + drawingX + width / 2 - happinessPercentSize.Width / 2,
                    position.Y + step * 2 + nameSize.Height + stateSize.Height + profSize.Height + dy + step + barHeight * 2);
            }
        }

        

        private void UpdateBuilding()
        {
            Building b = town.IsHumanInBuilding(this);

            if ((currentBuilding == null) && (b != null))
            {
                b.AddHuman(this);
                currentBuilding = b;
                return;
            }

            if ((currentBuilding != null) && (b == null))
            {
                currentBuilding.RemoveHuman(this);
                currentBuilding = null;
                return;
            }
        }

        public void Update(int dt)
        {
            Behaviour.Update(dt);
            UpdateBuilding();
        }

        public static void UpdateD(float dx, float dy)
        {
            Human.dx = dx - Config.TileSize / 2;
            Human.dy = dy - Config.TileSize / 2;
            
        }

        public override string ToString()
        {
            return "Id: " + id + " X:" + Position.X + " Y:" + Position.Y;
        }

        public void AddExp(double exp)
        {
            profExp[CurrentProf] += exp;
            if (ProfLevels[CurrentProf] < Config.MaxLevel - 1 &&
                profExp[CurrentProf] > Config.exp[ProfLevels[CurrentProf] - 1])
            {
                profExp[CurrentProf] -= Config.exp[ProfLevels[CurrentProf] - 1];
                ProfLevels[CurrentProf]++;
                Log.Add("levels: Human " + Name + "(" + CurrentProf + ") " + "level up to " + ProfLevels[CurrentProf]);
            }

            if ((ProfLevels[CurrentProf] == Config.MaxLevel - 1) && 
                (profExp[CurrentProf] >= Config.exp[ProfLevels[CurrentProf] - 1]))
            {
                ProfLevels[CurrentProf]++;
            }

            if (ProfLevels[CurrentProf] == Config.MaxLevel)
            {
                profExp[CurrentProf] = 1;
            }
        }
    }
}
