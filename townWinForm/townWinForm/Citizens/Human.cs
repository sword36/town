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
        public PointF Position { get; set; }
        public bool IsAlive { get; set; }
        public float Happiness { get; set; }
        public int Money { get; set; }
        public string CurrentProf { get; set; }
        public float Energy { get; set; }
        public Building Home { get; set; }
        public Building WorkBuilding { get; set; }
        public Bag Bag { get; set; }

        public Dictionary<String, int> ProfSkills;

        private static float dx = 0;
        private static float dy = 0;
        private int damage;
        private Human attackTarget = null;
        private Human activeTarget = null;
        private int waitTime = 0;


        public Human()
        {
            Money = Util.GetRandomDistribution(Config.StartMoney, Config.StartMoneyDelta);
            Happiness = Util.GetRandomDistribution(Config.StartHappiness, Config.StartHappinessDelta);
            Energy = Config.MaxEnergy;
            IsAlive = true;
            ProfSkills = new Dictionary<string, int>();
            Bag = new Bag();

            //set all proffesion skills to 1 level
            foreach(string prof in Config.ProfList)
            {
                ProfSkills.Add(prof, 1);
            }

            //random proffesion from Config.ProfList
            CurrentProf = Config.ProfList[Util.GetRandomFromInterval(0, Config.ProfList.Length - 1)];

            initBehaviourModel(CurrentProf);
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

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Black, Position.X, Position.Y, 30, 30);
        }

        public void Update(int dt)
        {
            Behaviour.Update(dt);
        }

        public static void UpdateD(float dx, float dy)
        {
            Human.dx = dx;
            Human.dy = dy;
        }

    }
}
