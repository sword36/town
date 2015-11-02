using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm.BehaviourModels
{
    public class Trader : BehaviourModel
    {
        public StackFSM StateMachine;

        public Trader(Human h, int level)
        {
            body = h;
            Level = level;
            StateMachine = new StackFSM("idle");
            base.WorkCost = Config.TraderWorkCost;
        }

        public override void Update(int dt)
        {
            switch (StateMachine.GetCurrentState())
            {
                case "idle":
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
