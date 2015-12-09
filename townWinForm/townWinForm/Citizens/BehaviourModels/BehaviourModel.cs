using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class BehaviourModel : IUpdatable
    {
        public float WorkCost { get; set; }
        protected Human body;
        public int Level { get; set; }
        protected bool isGoing = false;
        private int lastTryingEat = Config.TryEatInterval;

        public virtual void Update(int dt)
        {
        }



        public virtual string State
        {
            get;
        }

        //increase energy and happiness
        protected virtual void rest(int dt)
        {
            float dEnergy = Config.EnergyForRest * dt;
            if (body.Energy + dEnergy < Config.MaxEnergy)
            {
                body.Energy += dEnergy;
            }
            else
            {
                body.Energy = Config.MaxEnergy;
            }

            float dHappy = Config.HappyForRest * dt;
            if (body.Happiness + dHappy <= Config.MaxHappiness)
            {
                body.Happiness += dHappy;
            }
            else
            {
                body.Happiness = Config.MaxHappiness;
            }
        }

        protected virtual bool goToTavern(int dt)
        {
            float dEnergy = Config.EnergyMoveCost * dt;
            if (body.Energy - dEnergy > -1)
            {
                body.Energy -= dEnergy;
            }
            else
            {
                return false;
            }

            if (!isGoing)
            {
                isGoing = true;
                var path = body.Town.FindPath(new Point((int)body.Position.X, (int)body.Position.Y),
                    body.FavoriteTavern);
                body.Move(path, dt);

                Log.Add("citizens:Human " + body.Name + " go to tavern");
            }
            else
            {
                bool isAtTavern = body.MoveAlongThePath(dt);
                if (isAtTavern)
                {
                    isGoing = false;
                    Log.Add("citizens:Human " + body.Name + " came to tavern");
                }
                return isAtTavern;
            }

            return false;
        }

        protected virtual bool goToMarket(int dt)
        {
            float dEnergy = Config.EnergyMoveCost * dt;
            if (body.Energy - dEnergy > -1)
            {
                body.Energy -= dEnergy;
            }
            else
            {
                return false;
            }

            if (!isGoing)
            {
                isGoing = true;
                var path = body.Town.FindPath(new Point((int)body.Position.X, (int)body.Position.Y),
                    body.Town.GetNearestMarket(body) as Building);
                body.Move(path, dt);

                Log.Add("citizens:Human " + body.Name + " go to market");
            }
            else
            {
                bool isAtMarket = body.MoveAlongThePath(dt);
                if (isAtMarket)
                {
                    isGoing = false;
                    Log.Add("citizens:Human " + body.Name + " came to market");
                }
                return isAtMarket;
            }

            return false;
        }

        protected bool isSelling = false;

        protected virtual bool sell(int dt, out bool goOut)
        {
            goOut = false;
            if (!isSelling)
            {
                isSelling = true;
                body.WaitTime = Config.SellingTime;
            }

            body.WaitTime -= dt;
            if (body.WaitTime > 0)
            {
                return false;
            } else
            {
                body.WaitTime = 0;
            }

            bool isSold = false;
            List<Human> peopleInMarket = (body.Town.GetNearestMarket(body) as Building).PeopleIn;

            ThingType sellingType;
            if (body.Bag.FoodCount > body.Bag.ProductCount)
            {
                sellingType = ThingType.FOOD;
            } else if (body.Bag.ProductCount > body.Bag.FoodCount)
            {
                sellingType = ThingType.PRODUCT;
            } else
            {
                sellingType = ThingType.ANY;
            }

            for (int i = 0; i < peopleInMarket.Count; i++)
            {
                if (peopleInMarket[i].CurrentProf == "trader" && peopleInMarket[i] != body)
                {
                    isSold = body.Sell(peopleInMarket[i], sellingType);
                    if (isSold)
                    {
                        body.Happiness += Config.HappyForSelling;
                        if (body.Happiness > Config.MaxHappiness)
                        {
                            body.Happiness = Config.MaxHappiness;
                        }
                        return true;
                    }
                }
            }

            if (!isSold)
            {
                goOut = true;
            }
            isSelling = false;
            return isSold;
        }

        protected bool isBuying = false;

        protected virtual bool buyFood(int dt, out bool goOut)
        {
            goOut = false;
            if (!isBuying)
            {
                isBuying = true;
                body.WaitTime = Config.SellingTime;
            }

            body.WaitTime -= dt;
            if (body.WaitTime > 0)
            {
                return false;
            }
            else
            {
                body.WaitTime = 0;
            }

            bool isBought = false;
            List<Human> peopleInMarket = (body.Town.GetNearestMarket(body) as Building).PeopleIn;

            for (int i = 0; i < peopleInMarket.Count; i++)
            {
                if (peopleInMarket[i].CurrentProf == "trader" && peopleInMarket[i] != body)
                {
                    isBought = body.Buy(peopleInMarket[i], ThingType.FOOD);
                    if (isBought)
                    {
                        body.Happiness += Config.HappyForSelling;
                        if (body.Happiness > Config.MaxHappiness)
                        {
                            body.Happiness = Config.MaxHappiness;
                        }
                        return true;
                    }
                }
            }

            if (!isBought)
            {
                goOut = true;
            }
            isBought = false;
            return isBought;
        }

        protected virtual void tavernDrink(int dt)
        {
            float dEnergy = Config.EnergyForDrink * dt;
            if (body.Energy - dEnergy < -1)
            {
                body.Energy = 0;
            }
            else
            {
                body.Energy -= dEnergy;
            }

            float dHappy = Config.HappyForDrink * dt;
            if (body.Happiness + dHappy > Config.MaxHappiness)
            {
                body.Happiness = Config.MaxHappiness;
            }
            else
            {
                body.Happiness += dHappy;
            }
        }

        protected virtual void eat(int dt)
        {
            lastTryingEat += dt;
            try
            {
                if (lastTryingEat > Config.TryEatInterval)
                {
                    lastTryingEat = 0;

                    float dHappy = body.Eat();
                    if (body.Happiness + dHappy < Config.MaxHappiness)
                    {
                        body.Happiness += dHappy;
                    }
                    else
                    {
                        body.Happiness = Config.MaxHappiness;
                    }

                    Log.Add("citizens:Human " + body.Name + " eat: " + dHappy);
                }

            }
            catch (NoFoodExeption ex)
            {
                if (body.Happiness - Config.UnhappyForNoFood > 0)
                {
                    body.Happiness -= Config.UnhappyForNoFood;
                }
                else
                {
                    body.Happiness = 0;
                }

                Log.Add("citizens:Human " + body.Name + " can't eat: " + " no food((");
            }
        }

        protected virtual bool dying(int dt)
        {
            body.WaitTime -= dt;
            if (body.WaitTime < 0)
            {
                Log.Add("citizens:Human " + body.Name + " alive");
                body.WaitTime = Config.DyingTime;
                body.IsAlive = true;
                body.Energy = 1;
                body.Position = Util.ConvertIndexToInt(new PointF(body.Home.Position.X + 1, body.Home.Position.Y + 1));
                body.Happiness = Config.HappyAfterDeath;
                return true;
            }
            return false;
        }

        protected virtual bool patrol(int dt)
        {
            body.AddExp(Config.ExpForPatrol);

            float dEnergy = Config.EnergyPatrolCost * dt;
            if (body.Energy - dEnergy > -1)
            {
                body.Energy -= dEnergy;
            }
            else
            {
                return false;
            }

            if (body.Energy < Config.EnergyLowerBoundToUnhappy)
            {
                float dHappy = Config.UnhappyForWork * dt;
                if (body.Happiness - dHappy > 0)
                {
                    body.Happiness -= dHappy;
                }
            }

            if (!isGoing)
            {
                isGoing = true;
                var path = body.Town.FindPath(new Point((int)body.Position.X, (int)body.Position.Y),
                    body.Town.GetRandomStreetPoint());
                body.Move(path, dt);

                //Log.Add("citizens:Human " + body.Name + " patrol streen");
            }
            else
            {
                bool isAtPoint = body.MoveAlongThePath(dt);
                if (isAtPoint)
                {
                    isGoing = false;
                    //Log.Add("citizens:Human " + body.Name + " came home");
                }
                return isAtPoint;
            }

            return false;
        }

        protected virtual bool goHome(int dt)
        {
            float dEnergy = Config.EnergyMoveCost * dt;
            if (body.Energy - dEnergy > -1)
            {
                body.Energy -= dEnergy;
            }
            else
            {
                return false;
            }

            if (!isGoing)
            {
                isGoing = true;
                var path = body.Town.FindPath(new Point((int)body.Position.X, (int)body.Position.Y), body.Home);
                body.Move(path, dt);

                Log.Add("citizens:Human " + body.Name + " go home");
            }
            else
            {
                bool isAtHome = body.MoveAlongThePath(dt);
                if (isAtHome)
                {
                    isGoing = false;
                    Log.Add("citizens:Human " + body.Name + " came home");
                }
                return isAtHome;
            }

            return false;
        }

        protected virtual bool goToWork(int dt)
        {
            float dEnergy = Config.EnergyMoveCost * dt;
            if (body.Energy - dEnergy > -1)
            {
                body.Energy -= dEnergy;
            }
            else
            {
                return false;
            }

            //if didn't go before, we should find the path
            if (!isGoing)
            {
                isGoing = true;
                var path = body.Town.FindPath(new Point((int)body.Position.X, (int)body.Position.Y), body.WorkBuilding);
                body.Move(path, dt);

                Log.Add("citizens:Human " + body.Name + " go to work");
            }
            //if path exist already, go along the path
            else
            {
                bool isAtWork = body.MoveAlongThePath(dt);
                if (isAtWork)
                {
                    isGoing = false;
                    Log.Add("citizens:Human " + body.Name + " came at work");
                }
                return isAtWork;
            }

            return false;
        }

        //decrease energy, and if energy in low level then decrease happiness
        protected virtual void work(int dt)
        {
            if (body.CurrentProf == "trader")
                body.AddExp(Config.ExpForWorking / 2);
            else body.AddExp(Config.ExpForWorking);

            float dEnergy = WorkCost * dt;

            if (body.Energy - dEnergy > 0)
            {
                body.Energy -= dEnergy;
            }
            else
            {
                body.Energy = 0;
            }

            if (body.Energy < Config.EnergyLowerBoundToUnhappy)
            {

                float dHappy = Config.UnhappyForWork * dt;
                if (body.CurrentProf == "trader")
                {
                    dHappy /= 2;
                }

                if (body.Happiness - dHappy > 0)
                {
                    body.Happiness -= dHappy;
                }
            }
        }

        protected virtual void attack(int dt)
        {
            body.Attack();
        }

        //increase energy and happiness
        protected virtual void sleep(int dt)
        {
            float dEnergy = Config.EnergyForSleep * dt;
            if (body.Energy + dEnergy <= Config.MaxEnergy)
            {
                body.Energy += dEnergy;
            }
            else
            {
                body.Energy = Config.MaxEnergy;
            }

            float dHappy = Config.HappyForSleep * dt;
            if (body.Happiness + dHappy <= Config.MaxHappiness)
            {
                body.Happiness += dHappy;
            }
            else
            {
                body.Happiness = Config.MaxHappiness;
            }

            //Log.Add("citizens:Human" + body.Name + " sleep");
        }
    }
}
