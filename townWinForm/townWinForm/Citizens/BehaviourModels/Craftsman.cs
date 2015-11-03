using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm.BehaviourModels
{
    public class Craftsman : BehaviourModel
    {
        public StackFSM StateMachine;

        public Craftsman(Human h, int level)
        {
            body = h;
            Level = level;
            StateMachine = new StackFSM("rest");
            WorkCost = Config.CraftsmanWorkCost;
        }

        private void rest(int dt)
        {
            if (body.Energy > 30)
            {
                StateMachine.PopState();
                StateMachine.PushState("goToWork");
            } else if (body.Energy > 10)
            {
                StateMachine.PopState();
                StateMachine.PushState("goHome");
            } else
            {
                StateMachine.PushState("sleep");
            }
            base.rest(dt);
        }

        private void work(int dt)
        {
            if (body.Energy < 30)
            {
                if (true) { }
                StateMachine.PopState();
                StateMachine.PushState("goHome");
            }
            base.work(dt);
        }

        private void goToWork(int dt)
        {
            base.goToWork(dt);
        }

        private void goHome(int dt)
        {
            base.goHome(dt);
        }

        private void sleep(int dt)
        {
            if (body.Energy > 90)
            {
                StateMachine.PopState();
                StateMachine.PushState("idle");
            }
            base.sleep(dt);
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
