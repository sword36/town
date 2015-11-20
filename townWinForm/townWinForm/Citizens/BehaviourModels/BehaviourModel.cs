﻿using System;
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
            if (body.Energy <= 0)
            {
                body.Position = (body.Home as Building).Room;
            }
        }

        //increase energy and happiness
        protected virtual void rest(int dt)
        {
            float dEnergy = Config.EnergyForRest * dt;
            if (body.Energy + dEnergy < Config.MaxEnergy)
            {
                body.Energy += dEnergy;
            } else
            {
                body.Energy = Config.MaxEnergy;
            }

            float dHappy = Config.HappyForRest * dt;
            if (body.Happiness + dHappy <= Config.MaxHappiness)
            {
                body.Happiness += dHappy;
            } else
            {
                body.Happiness = Config.MaxHappiness;
            }
        }

        protected virtual bool goToTavern(int dt)
        {
            if (!isGoing)
            {
                isGoing = true;
                var path = body.Town.FindPath(new Point((int)body.Position.X, (int)body.Position.Y), body.FavoriteTavern);
                body.Move(path, dt);

                Log.Add("citizens:Human" + body.Id + " go to tavern");
            }
            else
            {
                bool isAtTavern = body.MoveAlongThePath(dt);
                if (isAtTavern)
                {
                    isGoing = false;
                    Log.Add("citizens:Human" + body.Id + " came to tavern");
                }
                return isAtTavern;
            }

            float dEnergy = Config.EnergyMoveCost * dt;
            //move
            if (body.Energy - dEnergy > 0)
            {
                body.Energy -= dEnergy;
                body.Move(body.Home.Position, dt);
            }
            else
            {
                body.Energy = 0;
            }
            return false;
        }

        protected virtual void tavernDrink(int dt)
        {
            float dEnergy = Config.EnergyForDrink * dt;
            if (body.Energy - dEnergy > 0)
            {
                body.Energy -= dEnergy;
            }
            else
            {
                body.Energy = 0;
            }

            float dHappy = Config.HappyForDrink * dt;
            if (body.Happiness + dHappy > Config.MaxHappiness)
            {
                body.Happiness = Config.MaxHappiness;

            }
            else
            {
                body.Happiness -= dHappy;
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

                    Log.Add("citizens:Human" + body.Id + " eat: " + dHappy);
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

                Log.Add("citizens:Human" + body.Id + " can't eat: " + " no food");
            }
        }

        int timeToAlive = Config.DyingTime;
        protected virtual string dying(int dt)
        {
            if (body.IsAlive)
            {
                body.IsAlive = false;
            }
            timeToAlive -= dt;
            if (timeToAlive < 0)
            {
                body.IsAlive = true;
                body.Position = (body.Home as Building).Room;
                body.Happiness = Config.HappyAfterDeathe;
            }
            
            if (!body.IsAlive)
            {
                return "dying";
            } else
            {
                return "sleep";
            }
        }

        protected virtual bool goHome(int dt)
        {
            if (!isGoing)
            {
                isGoing = true;
                var path = body.Town.FindPath(new Point((int)body.Position.X, (int)body.Position.Y), body.Home);
                body.Move(path, dt);

                Log.Add("citizens:Human" + body.Id + " go home");
            }
            else
            {
                bool isAtHome = body.MoveAlongThePath(dt);
                if (isAtHome)
                {
                    isGoing = false;
                    Log.Add("citizens:Human" + body.Id + " came home");
                }
                return isAtHome;
            }

            float dEnergy = Config.EnergyMoveCost * dt;
            //move
            if (body.Energy - dEnergy > 0)
            {
                body.Energy -= dEnergy;
                body.Move(body.Home.Position, dt);
            } else
            {
                body.Energy = 0;
            }
            return false;
        }

        protected virtual bool goToWork(int dt)
        {
            //if didn't go before, we should find the path
            if (!isGoing)
            {
                isGoing = true;
                var path = body.Town.FindPath(new Point((int)body.Position.X, (int)body.Position.Y), body.WorkBuilding);
                body.Move(path, dt);

                Log.Add("citizens:Human" + body.Id + " go to work");
            }
            //if path exist already, go along the path
            else
            {
                bool isAtWork = body.MoveAlongThePath(dt);
                if (isAtWork)
                {
                    isGoing = false;
                    Log.Add("citizens:Human" + body.Id + " came at work");
                }
                return isAtWork;
            }

            float dEnergy = Config.EnergyMoveCost * dt;
            if (body.Energy - dEnergy > 0)
            {
                body.Energy -= dEnergy;
                body.Move(body.WorkBuilding.Position, dt);
            } else
            {
                body.Energy = 0;
            }
            return false;
        }

        //decrease energy, and if energy in low level then decrease happiness
        protected virtual void work(int dt)
        {
            float dEnergy = WorkCost * dt;
            if (body.Energy - dEnergy > 0)
            {
                body.Energy -= dEnergy;
            } else
            {
                body.Energy = 0;
            }

            if (body.Energy < Config.EnergyLowerBoundToUnhappy)
            {
                float dHappy = Config.UnhappyForWork * dt;
                if (body.Happiness - dHappy > 0)
                {
                    body.Happiness -= dHappy;
                } else
                {
                    body.Happiness = Config.MaxHappiness;
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
            } else
            {
                body.Energy = Config.MaxEnergy;
            }

            float dHappy = Config.EnergyForSleep * dt;
            if (body.Happiness + dHappy <= Config.MaxHappiness)
            {
                body.Happiness += dHappy;
            } else
            {
                body.Happiness += Config.MaxHappiness;
            }

            Log.Add("citizens:Human" + body.Id + " sleep");
        }
    }
}
