using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm
{
    public class Workshop : Building, IWorkshop
    {
        public Workshop(int x, int y, int width, int height, string type) : base(x, y, width, height, type)
        {
            workers = new List<Human>();
        }

        protected List<Human> workers;

        public virtual int Count
        {
            get { return Workers.Count; }
        }

        public virtual List<Human> Workers
        {
            get { return workers; }
        }

        public virtual void AddWorker(Human h)
        {
            workers.Add(h);
            h.WorkBuilding = this;
        }

        public virtual void RemoveWorker(Human h)
        {
            workers.Remove(h);
        }

        public virtual bool IsFree()
        {
            return workers.Count < Config.MaxWorkers;
        }
    }
}
