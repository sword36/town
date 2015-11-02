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

        public virtual void Update(int dt) { }

        //increase energy and happiness
        protected virtual void rest(int dt)
        {
            float dEnergy = Config.EnergyForRest * dt;
            if (body.Energy + dEnergy <= Config.MaxEnergy)
            {
                body.Energy += dEnergy;
            }

            float dHappy = Config.HappyForRest * dt;
            if (body.Happiness + dHappy <= Config.MaxHappiness)
            {
                body.Happiness += dHappy;
            }
        }

        protected virtual void goHome(int dt)
        {
            float dEnergy = Config.EnergyMoveCost * dt;
            //move
            if (body.Energy - dEnergy >= 0)
            {
                body.Energy -= dEnergy;
                body.Move(body.Home.Position, dt);
            }
        }

        protected virtual void goToWork(int dt)
        {
            float dEnergy = Config.EnergyMoveCost * dt;
            //move
            if (body.Energy - dEnergy >= 0)
            {
                body.Energy -= dEnergy;
                body.Move(body.WorkBuilding.Position, dt);
            }
        }

        //decrease energy, and if energy in low level then decrease happiness
        protected virtual void work(int dt)
        {
            float dEnergy = WorkCost * dt;
            if (body.Energy - dEnergy >= 0)
            {
                body.Energy -= dEnergy;
            } 
            if (body.Energy < Config.EnergyLowerBoundToUnhappy)
            {
                float dHappy = Config.UnhappyForWork * dt;
                if (body.Happiness - dHappy >= 0)
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

            float dHappy = Config.EnergyForSleep * dt;
            if (body.Happiness + dHappy <= Config.MaxHappiness)
            {
                body.Happiness += dHappy;
            }
        }
    }
}
