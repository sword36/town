﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TownInterfaces;
using BehaviourModel;
using Behaviours;

namespace townWinForm
{
    public class Human : ICitizen, IDrawable, IUpdatable
    {
        
        public BehaviourModel.BehaviourModel Behaviour { get; set; }
        public PointF Position
        {
            get { return position; }
            set { position = value; }
        }
        private PointF position;
        public bool IsAlive { get; set; }
        public float Happiness { get; set; }
        public float Money { get; set; }
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
        public IEntertainment FavoriteTavern { get; set; }
        public IBag Bag { get; set; }
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

        public IBuilding currentBuilding { get; set; }

        public int id { get; set; }
        public PointF currentRoom { get; set; }

        public PointF CurrentRoom
        {
            get { return currentRoom; }
            set { currentRoom = value; }
        }

        public Dictionary<string, int> ProfLevels { get; set; }
        public Dictionary<string, double> profExp { get; set; }

        public IResidence home { get; set; }
        public IWorkshop work { get; set; }

        public static float dx = 0;
        public static float dy = 0;
        public int damage { get; set; }
        public ICitizen attackTarget { get; set; }
        public ICitizen activeTarget { get; set; }
        public int waitTime { get; set; }
        public PointF tempTarget { get; set; }
        public ITown town { get; set; }
        public List<PointF> Path { get; set; }
        public List<PointF> originalPath { get; set; }

        public string Name { get; set; }
        public Image img { get; set; }


        public bool IsClicked
        {
            get; set;
        }

        public Human(ITown t)
        {
            id = Util.GetNewID();
            town = t;

            
            KeyValuePair<string, Image> inf = town.GetInfo(id);

            Name = inf.Key;
            img = inf.Value;

            Money = Util.GetRandomDistribution(Config.StartMoney, Config.StartMoneyDelta);
            Happiness = Util.GetRandomDistribution(Config.StartHappiness, Config.StartHappinessDelta);
            Energy = Util.GetRandomDistribution(Config.StartEnergy, Config.StartEnergyDelta); ;
            IsAlive = true;

            ProfLevels = new Dictionary<string, int>();
            profExp = new Dictionary<string, double>();

            Bag = new Bag();

            Path = new List<PointF>();
            originalPath = new List<PointF>();
            
            CurrentBuilding = home as IBuilding;

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

            Bag.Add(new Food());
            Bag.Add(new Food());
            Bag.Add(new Product());

            FavoriteTavern = town.GetTavern();
        }


        ~Human()
        {
            img.Dispose();
        }
        private void initBehaviourModel(string prof)
        {
            switch(prof)
            {
                case "craftsman":
                    Behaviour = new Craftsman(this, ProfLevels[prof]);
                    break;
                case "farmer":
                    Behaviour = new Farmer(this, ProfLevels[prof]);
                    break;
                case "guardian":
                    Behaviour = new Guardian(this, ProfLevels[prof]);
                    break;
                case "thief":
                    Behaviour = new Thief(this, ProfLevels[prof]);
                    break;
                case "trader":
                    Behaviour = new Trader(this, ProfLevels[prof]);
                    break;
                default: throw new Exception("Wrong proffession");
            }

            Log.Add("citizens:Human " + Name + " behaviour: " + prof);
        }

        public float Eat()
        {
            IFood f = Bag.GetFood();
            if (f != null)
            {
                if (Energy + f.Energy <= Config.MaxEnergy)
                {
                    Energy += f.Energy;
                }
                return f.Energy;
            }
            else
            {
                return 0;
            }
        }

        public bool Sell(ICitizen buyer, TownInterfaces.ThingType type)
        {
            float percent = (float)ProfLevels["trader"] / Config.MaxLevel;
            IThing thing = Bag.GetWithPriceLower(buyer.Money, percent, (TownInterfaces.ThingType)type);

            if (thing != null)
            {
                float priceWithPercent = thing.Price * (1 + percent);
                buyer.Money -= priceWithPercent;
                Money += priceWithPercent;
                try
                {
                    buyer.Bag.Add(thing);
                }
                catch (OverloadedBagExeption ex)
                {
                    Log.Add("citizens:Human " + buyer.Name + " haven't enougth place for new thing after buying");
                }

                string typeName = thing.GetType().Name;
                Log.Add("other:Human " + Name + " sold " + typeName + " with price: " + priceWithPercent +
                    " to " + buyer.Name );

                return true;
            }
            return false;
        }

        public bool Buy(ICitizen trader, TownInterfaces.ThingType type)
        {
            float percent = (float)trader.ProfLevels["trader"] / Config.MaxLevel;
            IThing thing = trader.Bag.GetWithPriceLower(Money, percent, type);

            if (thing != null)
            {
                float priceWithPercent = thing.Price * (1 + percent);
                Money -= priceWithPercent;
                trader.Money += priceWithPercent;

                try
                {
                    Bag.Add(thing);
                }
                catch (OverloadedBagExeption ex)
                {
                    Log.Add("citizens:Human " + Name + " haven't enougth place for new thing after buying");
                }

                string typeName = thing.GetType().Name;
                Log.Add("other:Human " + Name + " bought " + typeName + " with price: " + priceWithPercent +
                    " at " + trader.Name);

                return true;
            }
            return false;
        }

        public void Move(List<PointF> pN, int dt)
        {
            if (pN.Count != 0)
            {
                Path = pN;
                tempTarget = Path.First();
                Path.RemoveAt(0);
                MoveAlongThePath(dt);
            } else //last point
            {
                MoveAlongThePath(dt);
            }
            Energy -= Config.EnergyMoveCost;
        }

        public bool MoveAlongThePath(int dt)
        {
            if (Path == null)
            {
                throw new Exception("Wrong path");
            }

            if (Util.Distance(Position, tempTarget) < Config.MovePrecision)
            {
                Position = tempTarget;
                if (Path.Count != 0)
                {
                    tempTarget = Path.First();
                    Path.RemoveAt(0);
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

        public float DistanceToHome()
        {
            return Util.Distance(Home.Position, Position);
        }

        public float DistanceToWork()
        {
            return Util.Distance(WorkBuilding.Position, Position);
        }

        public void Attack(ICitizen target)
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

        public int CurrentLevel
        {
            get
            {
                return ProfLevels[CurrentProf];
            }
        }

        public ITown Town
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

        public IBuilding CurrentBuilding
        {
            get
            {
                return currentBuilding;
            }

            set
            {
                currentBuilding = value;
            }
        }

        public float Tax
        {
            get
            {
                float res = Money / 10;
                Money -= res;
                return res;
            }
        }

        public int WaitTime
        {
            get
            {
                return waitTime;
            }

            set
            {
                waitTime = value;
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
            g.DrawImage(img, position.X + 3 + dx, position.Y + 3 + dy, Config.TileSize - 6, Config.TileSize - 6);
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

                string expString = "XP " + ((int)profExp[CurrentProf]).ToString() + "/" + Config.exp[CurrentLevel - 1];
                string energyString = "Energy " + energyPercent.ToString() + "%";
                string happinessString = "Happiness " + happinessPercent.ToString() + "%";
                string professionString = CurrentProf + " " + ProfLevels[CurrentProf] + " lvl";
                string stateString = Behaviour.State + " " + (int)Money + "Њ";

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

                /*g.FillEllipse(new SolidBrush(Color.FromArgb(100, 0, 200, 0)),
                    Position.X + dx - Config.VisionRadius,
                    Position.Y + dy - Config.VisionRadius,
                    2 * Config.VisionRadius,
                    2 * Config.VisionRadius);*/

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
            IBuilding b = town.IsHumanInBuilding(this);

            if ((CurrentBuilding == null) && (b != null))
            {
                b.AddHuman(this);
                CurrentBuilding = b;
                return;
            }

            if ((CurrentBuilding != null) && (b == null))
            {
                CurrentBuilding.RemoveHuman(this);
                CurrentBuilding = null;
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
                Log.Add("levels: Human " + Name + " (" + CurrentProf + ") " + "leveled up to " + ProfLevels[CurrentProf]);
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
