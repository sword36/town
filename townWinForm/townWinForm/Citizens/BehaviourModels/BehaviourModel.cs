using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class BehaviourModel : IUpdatable
    {
        public float WorkCost { get; set; }
        protected Human body;
        public int Level { get; set; }

        public virtual void Update(int dt) { }

        //do nothing
        protected virtual void idle(int dt)
        {

        }

        protected virtual void goHome(int dt)
        {
            body.Move(body.Home.Position);
        }

        protected virtual void goToWork(int dt)
        {
            body.Move(body.WorkBuilding.Position);
        }

        protected virtual void work(int dt)
        {
            body.Work(dt);
        }

        protected virtual void attack(int dt)
        {
            body.Attack();
        }

        protected virtual void sleep(int dt)
        {
            body.Energy += Config.EnergyForSleep * dt;
        }
    }
}
