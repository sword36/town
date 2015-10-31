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
        public int Happiness { get; set; }
        public int Money { get; set; }
        public string CurrentProf { get; set; }

        public Dictionary<String, int> ProfSkills;

        private int hp;
        private int damage;
        private Human attackTarget = null;
        private Human activeTarget = null;
        private int waitTime = 0;


        public Human()
        {
            Money = Util.GetRandomDistribution(Config.StartMoney, Config.StartMoneyDelta);
            Happiness = Util.GetRandomDistribution(Config.StartHappiness, Config.StartHappinessDelta);
            IsAlive = true;

            //set all proffesion skills to 1 level
            foreach(string prof in Config.ProfList)
            {
                ProfSkills.Add(prof, 1);
            }

            CurrentProf = Config.ProfList[Util.GetRandomFromInterval(0, Config.ProfList.Length - 1)];
        }

        private void initBehaviourModel(string prof)
        {
            switch(prof)
            {
                case "craftsman":
                    Behaviour = new BehaviourModels.Craftsman(this);
                    break;
                case "farmer":
                    Behaviour = new BehaviourModels.Farmer(this);
                    break;
                case "guardian":
                    Behaviour = new BehaviourModels.Guardian(this);
                    break;
                case "thief":
                    Behaviour = new BehaviourModels.Thief(this);
                    break;
                case "trader":
                    Behaviour = new BehaviourModels.Trader(this);
                    break;
            }
        }

        public void Move(PointF p)
        {

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
            hp -= dmg;

            if (hp <= 0)
            {
                IsAlive = false;
            }
        }

        public int Hp
        {
            get
            {
                return hp;
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

    }
}
