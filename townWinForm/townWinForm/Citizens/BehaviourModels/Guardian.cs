using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownInterfaces;

namespace townWinForm.BehaviourModels
{
    public class Guardian : BehaviourModel
    {
        public Guardian(ICitizen h, int level) : base(h, level)
        {
            base.WorkCost = 0.004f; //Config.GuardianWorkCost;
            h.Bag.MaxCapacity = 300; //Config.GuardianBagCapacity;
            h.Speed = 0.125f; //Config.GuardianSpeed;
        }

        protected override void rest(int dt)
        {
            base.rest(dt);

            if (body.Energy > 95)
            {
                StateMachine.PopState();
                StateMachine.PushState("goToWork");
            }
            else if (body.Energy < 25)
            {
                if (body.DistanceToHome() < Config.HomeNear && body.CurrentBuilding as IResidence != body.Home)
                {
                    StateMachine.PopState();
                    StateMachine.PushState("goHome");
                    //StateMachine.EnqueueState("sleep");
                }
                else
                {
                    Log.Add("citizens:Humant " + body.Name + " sleeping");
                    StateMachine.PopState();
                    StateMachine.PushState("sleep");
                }
            }
            else if (body.Bag.FoodCount < 3 && body.Money > Config.MoneyLimitForBuying)
            {
                StateMachine.PopState();
                StateMachine.PushState("goToMarket");
                StateMachine.EnqueueState("buyFood");
            }
            else if (body.Energy < 60)
            {
                eat(dt);
            }
        }

        private bool isWorking = false;

        protected override void work(int dt)
        {
            if (!isWorking)
            {
                isWorking = true;
                Log.Add("citizens:Human " + body.Name + " working(guardian)");
            }

            base.patrol(dt);
         
            if (body.Energy < 30)
            {
                if (true) { }
                StateMachine.PopState();
                isWorking = false;
                base.isGoing = false;
                Log.Add("citizens:Human " + body.Name + " finish work(guardian), energy too low");

                if (body.Bag.FoodCount < 3 && body.Money > Config.MoneyLimitForBuying)
                {
                    StateMachine.PushState("goToMarket");
                    StateMachine.EnqueueState("buyFood");
                    return;
                }

                if (body.Happiness < Config.LowerBoundHappyToDrink)
                {
                    StateMachine.PushState("goToTavern");
                }
                else
                {
                    StateMachine.PushState("goHome");
                }
            }
            else if (body.Happiness < 20)
            {
                isWorking = false;
                Log.Add("citizens:Human " + body.Name + " finish work(guardian), happy too low");
                StateMachine.PopState();

                base.isGoing = false;
                StateMachine.PushState("goToTavern");
            }
        }

        public override void Update(int dt)
        {
            if (body.Energy <= 0 && body.IsAlive)
            {
                Log.Add("citizens:Human " + body.Name + " died during: " + StateMachine.GetCurrentState());

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
                case "buyFood":
                    buyFood(dt);
                    break;
                case "goToMarket":
                    goToMarket(dt);
                    break;
            }
        }
    }
}
