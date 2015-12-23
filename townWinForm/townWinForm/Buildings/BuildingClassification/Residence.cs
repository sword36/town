using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownInterfaces;

namespace townWinForm
{
    public class Residence : Building, IResidence
    {
        public Residence(int x, int y, int width, int height, string type) : base(x, y, width, height, type)
        {
            residents = new List<TownInterfaces.ICitizen>();
        }

        protected List<TownInterfaces.ICitizen> residents;
        public virtual List<TownInterfaces.ICitizen> Residents
        {
            get { return residents; }
        }

        public virtual void RemoveResident(TownInterfaces.ICitizen h)
        {
            residents.Remove(h);
        }

        public virtual void AddResident(ICitizen h)
        {
            residents.Add(h);
            h.Home = this;
        }

        public virtual bool HavePlace()
        {
            return residents.Count < Config.MaxResidents;
        }
    }
}
