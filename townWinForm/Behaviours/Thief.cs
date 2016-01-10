using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownInterfaces;
using BehaviourModel;

namespace Behaviours
{
    public class Thief : BehaviourModel.BehaviourModel, IUpdatable, IBehaviourable
    {
        public Thief(ICitizen h, int level) : base(h, level)
        {
            base.WorkCost = 0.002f; //Config.ThiefWorkCost;
            h.Bag.MaxCapacity = 750; //Config.ThiefBagCapacity;
            h.Speed = 0.125f; //Config.ThiefSpeed;
        }

        public override void rest(int dt)
        {
            base.rest(dt);

            if (body.Energy > 95)
            {
                StateMachine.PopState();
                StateMachine.PushState("goToWork");
            }
            else if (body.Energy < 20)
            {
                if (body.DistanceToHome() < Config.HomeNear && body.CurrentBuilding as IResidence != body.Home)
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
            else if (body.Energy < 60)
            {
                eat(dt);
            }
        }

        private bool isWorking = false;

        public override void work(int dt)
        {
            if (!isWorking)
            {
                isWorking = true;
            }

            base.work(dt);

            if (body.Energy < 30)
            {
                if (true) { }
                StateMachine.PopState();
                isWorking = false;

                if (body.Happiness < Config.LowerBoundHappyToDrink)
                {
                }
                else
                {
                    StateMachine.PushState("goHome");
                }
            }
            else if (body.Happiness < 20)
            {
                isWorking = false;
                StateMachine.PopState();
                StateMachine.PushState("goToTavern");
            }
        }

        public override void Update(int dt)
        {
            if (body.Energy <= 0 && body.IsAlive)
            {
                body.WaitTime = Config.DyingTime;
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
                case "goToTavern":
                    goToTavern(dt);
                    break;
                case "tavernDrink":
                    tavernDrink(dt);
                    break;
            }
        }
    }
}
