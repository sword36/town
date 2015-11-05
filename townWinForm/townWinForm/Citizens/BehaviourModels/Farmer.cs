using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm.BehaviourModels
{
    public class Farmer : BehaviourModel
    {
        public StackFSM StateMachine;

        public Farmer(Human h, int level)
        {
            body = h;
            Level = level;
            StateMachine = new StackFSM("rest");
            base.WorkCost = Config.FarmerWorkCost;
            h.Bag.MaxCapacity = Config.FarmerBagCapacity;
            h.Speed = Config.FarmerSpeed;
        }

        public override void Update(int dt)
        {
            switch (StateMachine.GetCurrentState())
            {
                case "rest":
                    rest(dt);
                    break;
                case "work":
                    work(dt);
                    break;
                case "goToWork":
                    goToWork(dt);
                    break;
                case "goHome":
                    goHome(dt);
                    break;
                case "sleep":
                    sleep(dt);
                    break;
            }
        }
    }
}
