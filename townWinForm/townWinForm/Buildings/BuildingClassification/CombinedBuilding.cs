using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownInterfaces;

namespace townWinForm
{
    public class CombinedBuilding : Building, IWorkshop, IResidence
    {
        public CombinedBuilding(int x, int y, int width, int height, string type) : base(x, y, width, height, type)
        {
            residents = new List<ICitizen>();
            workers = new List<ICitizen>();
        }

        protected List<ICitizen> residents;

        public virtual List<ICitizen> Residents
        {
            get { return residents; }
        }

        protected List<ICitizen> workers;

        public virtual int Count
        {
            get { return Workers.Count; }
        }

        public virtual List<ICitizen> Workers
        {
            get { return workers; }
        }

        public virtual void RemoveResident(ICitizen h)
        {
            residents.Remove(h);
        }

        public virtual void AddResident(ICitizen h)
        {
            residents.Add(h);
            h.Home = this;

        }

        public virtual void AddWorker(ICitizen h)
        {
            workers.Add(h);
            h.WorkBuilding = this;
        }

        public virtual void RemoveWorker(ICitizen h)
        {
            workers.Remove(h);
        }

        public virtual bool IsFree()
        {
            return true;
        }

        public virtual bool HavePlace()
        {
            return true;
        }
    }
}
