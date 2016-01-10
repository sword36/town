using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TownInterfaces
{
    public interface ICitizen : IUpdatable, IDrawable
    {
        IWorkshop WorkBuilding
        {
            get; set;
        }

        IResidence Home
        {
            get; set;
        }

        PointF Position
        {
            get;
            set;
        }

        bool IsAlive { get; set; }
        float Happiness { get; set; }
        float Money { get; set; }
        string CurrentProf { get; set; }
        float Energy { get; set; }

        IEntertainment FavoriteTavern { get; set; }
        IBag Bag { get; set; }
        float Speed { get; set; }

        IBuilding currentBuilding
        { get; set; }

        int id
        { get; set; }

        PointF currentRoom
        { get; set; }

        PointF CurrentRoom { get; set; }

        Dictionary<string, int> ProfLevels
        { get; set; }
        Dictionary<string, double> profExp
        { get; set; }

        IResidence home
        { get; set; }
        IWorkshop work
        { get; set; }

        int damage
        { get; set; }
        ICitizen attackTarget { get; set; }
        ICitizen activeTarget { get; set; }
        int waitTime { get; set; }
        PointF tempTarget { get; set; }
        ITown town { get; set; }
        List<PointF> Path { get; set; }
        List<PointF> originalPath { get; set; }

        string Name { get; set; }
        Image img { get; set; }

        int Damage { get; set; }


        bool IsClicked
        {
            get; set;
        }

        void TakeDamage(int damage);

        int WaitTime { get; set; }

        float Tax
        {
            get;
        }

        IBuilding CurrentBuilding { get; set; }

        void Move(List<PointF> pN, int dt);

        bool MoveAlongThePath(int dt);
        void moveToTempTarget(int dt);

        float DistanceToHome();

        float DistanceToWork();

        void Attack(ICitizen target);
        void Attack();

        bool Sell(ICitizen buyer, ThingType type);
        void AddExp(double exp);

        bool Buy(ICitizen trader, ThingType type);

        float Eat();

        int CurrentLevel { get; }
    }
}
