﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm.BehaviourModels
{
    public class Guardian : BehaviourModel
    {
        public StackFSM StateMachine;

        public Guardian(Human h, int level)
        {
            body = h;
            Level = level;
            StateMachine = new StackFSM("rest");
            base.WorkCost = Config.GuardianWorkCost;
            h.Bag.MaxCapacity = Config.GuardianBagCapacity;
            h.Speed = Config.GuardianSpeed;
        }

        private void dying(int dt)
        {
            bool isAlive = base.dying(dt);
            if (isAlive)
            {
                StateMachine.PopState();
                StateMachine.PushState("sleep");
            }
        }

        private void rest(int dt)
        {
            base.rest(dt);

            if (body.Energy > 95)
            {
                StateMachine.PopState();
                StateMachine.PushState("goToWork");
            }
            else if (body.Energy < 40)
            {
                eat(dt);
            }
            else if (body.Energy < 25)
            {
                if (body.DistanceToHome() < Config.HomeNear)
                {
                    StateMachine.PopState();
                    StateMachine.PushState("goHome");
                    //StateMachine.EnqueueState("sleep");
                }
                else
                {
                    StateMachine.PopState();
                    StateMachine.PushState("sleep");
                }
            }
        }

        private bool isWorking = false;

        private void work(int dt)
        {
            base.work(dt);

            if (!isWorking)
            {
                isWorking = true;
                Log.Add("citizens:Human" + body.Id + " working(guardian)");
            }

            if (body.Energy < 30)
            {
                if (true) { }
                StateMachine.PopState();
                StateMachine.PushState("goHome");
                isWorking = false;
                Log.Add("citizens:Human" + body.Id + " finish work(guardian)");
                //StateMachine.EnqueueState("rest");
            }
        }

        private void goToWork(int dt)
        {
            bool isAtWork = base.goToWork(dt);
            if (isAtWork)
            {
                StateMachine.PopState();
                StateMachine.PushState("work");
                return;
            }

            if (body.Energy < 5)
            {
                //not pop state
                StateMachine.PushState("rest");
            }
        }

        private void goHome(int dt)
        {
            bool isAtHome = base.goHome(dt);

            if (isAtHome)
            {
                StateMachine.PopState();
                StateMachine.PushState("rest");
                return;
            }

            if (body.Energy < 5)
            {
                StateMachine.PopState();
                StateMachine.PushState("rest");
            }
        }

        private void sleep(int dt)
        {
            base.sleep(dt);

            if (body.Energy > 95)
            {
                StateMachine.PopState();
                StateMachine.PushState("rest");
            }
        }

        public override void Update(int dt)
        {
            if (body.Energy <= 0 && body.IsAlive)
            {
                body.IsAlive = false;
                StateMachine.PopState();
                StateMachine.PushState("dying");
            }

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
                case "dying":
                    dying(dt);
                    break;
            }
        }
    }
}
