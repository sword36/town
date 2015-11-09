using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm.Buildings
{
    public abstract class ProductionBuilding : Building
    {
        protected List<Human> Workers;

        public virtual bool AddWorker(Human h)
        {
            if (Workers.Count >= Config.MaxWorkers)
            {
                return false;
            }

            Workers.Add(h);
            h.WorkBuilding = this;
            return true;
        }
    }
}
