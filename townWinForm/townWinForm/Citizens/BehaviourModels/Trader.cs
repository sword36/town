﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownInterfaces;

namespace townWinForm.BehaviourModels
{
    public class Trader : BehaviourModel
    {
        public StackFSM StateMachine;

        public override string State
        {
            get { return StateMachine.GetCurrentState(); }
        }

        public Trader(ICitizen h, int level)
        {
            body = h;
            Level = level;
            StateMachine = new StackFSM("rest");
            base.WorkCost = Config.TraderWorkCost;
            h.Bag.MaxCapacity = Config.ThiefBagCapacity;
            h.Speed = Config.TraderSpeed;
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
            else if (body.Energy < 35)
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
                Log.Add("citizens:Human " + body.Name + " working(trader)");
            }

            base.work(dt);

            if (body.Bag.ProductCount > Config.MaxProductForTrader)
            {
                body.Sell(body.town.God, TownInterfaces.ThingType.PRODUCT);
            }

            if (body.Energy < 30)
            {
                if (true) { }
                StateMachine.PopState();
                isWorking = false;
                Log.Add("citizens:Human " + body.Name + " finish work(trader), energy too low");

                if (body.Happiness < Config.LowerBoundHappyToDrink)
                {
                    StateMachine.PushState("goToTavern");
                    Log.Add("citizens:Human " + body.Name + " go to tavern");
                }
                else
                {
                    StateMachine.PushState("goHome");
                }
            }
            else if (body.Happiness < 20)
            {
                isWorking = false;
                Log.Add("citizens:Human " + body.Name + " finish work(trader), happy too low");
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
                //StateMachine.PopState();
                //StateMachine.PushState("rest");
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

        private void goToTavern(int dt)
        {
            bool isAtTavern = base.goToTavern(dt);
            if (isAtTavern)
            {
                StateMachine.PopState();

                if (body.Money - Config.DrinkInTavernCost < 0)
                {
                    StateMachine.PushState("goHome");
                    Log.Add("citizens:Human " + body.Name + " havent money for drinking");
                    return;
                }

                body.Money -= Config.DrinkInTavernCost;
                StateMachine.PushState("tavernDrink");
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

        public override void Update(int dt)
        {
            if (body.Energy <= 0 && body.IsAlive)
            {
                Log.Add("citizens:Human " + body.Name + " died during: " + StateMachine.GetCurrentState());

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
            }
        }
    }
}
