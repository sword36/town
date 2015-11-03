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
            h.Bag.MaxCapacity = Config.CraftsmanBagCapacity;
            h.Speed = Config.CraftsmanSpeed;
        }

        private void rest(int dt)
        {
            if (body.Energy > 80)
            {
                StateMachine.PopState();
                StateMachine.PushState("goToWork");
            } else if (body.Energy < 50)
            {
                eat();
            }
            else if (body.Energy < 20)
            {
                if (body.DistanceToHome() < Config.HomeNear)
                {
                    StateMachine.PopState();
                    StateMachine.PushState("goHome");
                    StateMachine.EnqueueState("sleep");
                } else
                {
                    StateMachine.PopState();
                    StateMachine.PushState("sleep");
                }
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
                StateMachine.EnqueueState("rest");
            }
            base.work(dt);
        }

        private void goToWork(int dt)
        {
            bool isAtHome = base.goToWork(dt);

            if (body.Energy < 5 || isAtHome)
            {
                StateMachine.PushState("rest");
            }
        }

        private void goHome(int dt)
        {
            bool isAtHome = base.goHome(dt);

            if (body.Energy < 5 || isAtHome)
            {
                StateMachine.PushState("rest");
            }
        }

        private void sleep(int dt)
        {
            base.sleep(dt);

            if (body.Energy > 90)
            {
                StateMachine.PopState();
                StateMachine.PushState("rest");
            }
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
