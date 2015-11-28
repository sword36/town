﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm.BehaviourModels
{
    public class Craftsman : BehaviourModel
    {
        public StackFSM StateMachine;
        public override string State
        {
            get { return StateMachine.GetCurrentState(); }
        }

        public Craftsman(Human h, int level)
        {
            body = h;
            Level = level;
            StateMachine = new StackFSM("rest");
            WorkCost = Config.CraftsmanWorkCost;
            h.Bag.MaxCapacity = Config.CraftsmanBagCapacity;
            h.Speed = Config.CraftsmanSpeed;
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
                    Log.Add("citizens:Humant " + body.Name + " sleeping");
                    StateMachine.PopState();
                    StateMachine.PushState("sleep");
                }
            } else if (body.Energy > 60 && body.Bag.Count > Config.ThingsLimitForSelling)
            {
                StateMachine.PopState();
                StateMachine.PushState("goToMarket");
            }
            else if (body.Energy < 60)
            {
                eat(dt);
            }
        }

        private bool isWorking = false;

        private void work(int dt)
        {
            if (!isWorking)
            {
                isWorking = true;
                Log.Add("citizens:Human " + body.Name + " working(craftsman)");
            }

            base.work(dt);

            if (Util.GetRandomNumberF() < Config.ChanceToCraftProduct)
            {
                try
                {
                    Product p = new Product();
                    body.Bag.Add(p);
                    Log.Add("things:Product with price: " + p.Price + " crafted by craftsman, " + this.body.Name);
                    Log.Add("citizens:Human" + body.Name + " crafted new product with price: " + p.Price);
                    body.AddExp(Config.ExpForCraft * (1 + body.CurrentLevel / 10));
                }
                catch (OverloadedBagExeption ex)
                {
                    Log.Add("citizens:Human " + body.Name + " haven't enougth place for new product");
                }
            }

            if (body.Energy < 30)
            {
                if (true) { }
                StateMachine.PopState();
                isWorking = false;
                Log.Add("citizens:Human " + body.Name + " finish work(craftsman), energy too low");

                if (body.Bag.Count > Config.ThingsLimitForSelling)
                {
                    StateMachine.PushState("goToMarket");
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
                Log.Add("citizens:Human " + body.Name + " finish work(craftsman), happy too low");
                StateMachine.PopState();
                StateMachine.PushState("goToTavern");
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

            if (body.Energy > 90)
            {
                StateMachine.PopState();
                StateMachine.PushState("rest");
            }
        }

        private void goToTavern(int dt)
        {
            bool isAtTavern = base.goToTavern(dt);
            if (isAtTavern)
            {
                StateMachine.PopState();
                StateMachine.PushState("tavernDrink");
                body.Money -= Config.DrinkInTavernCost;
                Log.Add("citizens:Human " + body.Name + " drinking!");
                return;
            }

            if (body.Energy < 5)
            {
                StateMachine.PushState("rest");
            }
        }

        private void tavernDrink(int dt)
        {
            base.tavernDrink(dt);
            if (body.Happiness > Config.LimitHappyInTavern)
            {
                Log.Add("citizens:Human " + body.Name + " drunk, go home!");
                StateMachine.PopState();
                StateMachine.PushState("goHome");
            }
        }

        private void goToMarket(int dt)
        {
            bool isAtMarket = base.goToMarket(dt);
            if (isAtMarket)
            {
                StateMachine.PopState();
                StateMachine.PushState("sell");
                Log.Add("citizens:Human " + body.Name + " at market");
                return;
            }

            if (body.Energy < 5)
            {
                StateMachine.PushState("rest");
            }
        }

        private void sell(int dt)
        {
            bool isGoOut;
            bool isSold = base.sell(dt, out isGoOut);
            if (isSold)
            {
                if (body.Money < Config.MoneyLimitForSelling && body.Bag.Count > Config.ThingsLimitForSelling)
                {
                    //continue selling
                } else
                {
                    StateMachine.PopState();
                    StateMachine.PushState("goHome");
                }
            } else if (isGoOut)
            {
                StateMachine.PopState();
                StateMachine.PushState("goHome");
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
                Log.Add("citizens:Human " + body.Name + " died during: " + StateMachine.GetCurrentState());
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
            }
        }
    }
}
