using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownInterfaces;
using BehaviourModel;

namespace Behaviours
{
    public class Craftsman : BehaviourModel.BehaviourModel, IUpdatable, IBehaviourable
    {
        public Craftsman(ICitizen h, int level) : base(h, level)
        {
            WorkCost = 0.005f; //Config.CraftsmanWorkCost;
            h.Bag.MaxCapacity = 600; //Config.CraftsmanBagCapacity;
            h.Speed = 0.1f; //Config.CraftsmanSpeed;
        }

        public override void rest(int dt)
        {
            base.rest(dt);

            if (body.Energy > 80)
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
                }
                else
                {
                    StateMachine.PopState();
                    StateMachine.PushState("sleep");
                }
            } else if (body.Energy > 60 && body.Bag.Count > Config.ThingsLimitForSelling)
            {
                StateMachine.PopState();
                StateMachine.PushState("goToMarket");
                StateMachine.EnqueueState("sell");
            } else if (body.Bag.FoodCount < 3 && body.Money > Config.MoneyLimitForBuying)
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

        public override void work(int dt)
        {
            if (!isWorking)
            {
                isWorking = true;
            }

            base.work(dt);

            if (Util.GetRandomNumberF() < Config.ChanceToCraftProduct)
            {
                try
                {
                    products.Product p = new products.Product();
                    body.Bag.Add(p);
                    body.AddExp(Config.ExpForCraft * (1 + body.CurrentLevel / 10));
                }
                catch (Exception ex)
                {
                }
            }

            if (body.Energy < 30)
            {
                if (true) { }
                StateMachine.PopState();
                isWorking = false;

                if (body.Bag.Count > Config.ThingsLimitForSelling)
                {
                    StateMachine.PushState("goToMarket");
                    StateMachine.EnqueueState("sell");
                    return;
                }

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
                case "goToMarket":
                    goToMarket(dt);
                    break;
                case "sell":
                    sell(dt);
                    break;
                case "buyFood":
                    buyFood(dt);
                    break;
            }
        }
    }
}
