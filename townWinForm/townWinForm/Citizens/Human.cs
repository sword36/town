﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using VkNet.Model;

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

        public Dictionary<string, int> ProfSkills;

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

        private string name;


        public bool IsClicked
        {
            get; set;
        }

        public Human(Town t, string u)
        {
            name = u;
            town = t;
            Money = Util.GetRandomDistribution(Config.StartMoney, Config.StartMoneyDelta);
            Happiness = Util.GetRandomDistribution(Config.StartHappiness, Config.StartHappinessDelta);
            Energy = Config.MaxEnergy / 5;
            IsAlive = true;
            ProfSkills = new Dictionary<string, int>();
            Bag = new Bag();
            path = new List<PointF>();
            originalPath = new List<PointF>();
            id = Util.GetNewID();
            currentBuilding = home as Building;

            //set all proffesion skills to 1 level
            foreach (string prof in Config.ProfList)
            {
                ProfSkills.Add(prof, 1);
            }

            //random proffesion from Config.ProfList
            CurrentProf = Config.ProfList[Util.GetRandomFromInterval(0, Config.ProfList.Length - 1)];

            initBehaviourModel(CurrentProf); 

            Log.Add("citizens:Human" + id + " created");
        }



        private void initBehaviourModel(string prof)
        {
            switch(prof)
            {
                case "craftsman":
                    Behaviour = new BehaviourModels.Craftsman(this, ProfSkills[prof]);
                    break;
                case "farmer":
                    Behaviour = new BehaviourModels.Farmer(this, ProfSkills[prof]);
                    break;
                case "guardian":
                    Behaviour = new BehaviourModels.Guardian(this, ProfSkills[prof]);
                    break;
                case "thief":
                    Behaviour = new BehaviourModels.Thief(this, ProfSkills[prof]);
                    break;
                case "trader":
                    Behaviour = new BehaviourModels.Trader(this, ProfSkills[prof]);
                    break;
                default: throw new Exception("Wrong proffession");
            }

            Log.Add("citizens:Human" + id + " behaviour: " + prof);
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
            if (IsClicked)
            {
                g.FillRectangle(Brushes.Chartreuse, Position.X + dx,
                Position.Y + dy,
                Config.TileSize, Config.TileSize);

                using (Font f = new Font("Courier New", 12, FontStyle.Regular))
                {
                    SizeF size = g.MeasureString(name, f);

                    g.FillRectangle(new SolidBrush(Color.FromArgb(100, 255, 255, 255)),
                        Position.X + Config.TileSize + 5 + dx,
                        Position.Y + dy, size.Width + 10, size.Height + 10);

                    g.DrawRectangle(new Pen(Color.FromArgb(100, 20, 20, 20)),
                        Position.X + Config.TileSize + 5 + dx,
                        Position.Y + dy, size.Width + 10, size.Height + 10);

                    g.DrawString(name, f, Brushes.Black, position.X + dx + Config.TileSize + 5, position.Y + dy + 5);
                }

            }

            else
                g.FillRectangle(Brushes.Red, Position.X + dx,
                Position.Y + dy,
                Config.TileSize, Config.TileSize);
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
    }
}
