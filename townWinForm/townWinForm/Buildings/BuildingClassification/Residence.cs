using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm
{
    public class Residence : Building, IResidence
    {
        public Residence(int x, int y, int width, int height, string type) : base(x, y, width, height, type)
        {
            residents = new List<Human>();
        }

        protected List<Human> residents;
        public virtual List<Human> Residents
        {
            get { return residents; }
        }

        public virtual void RemoveResident(Human h)
        {
            residents.Remove(h);
        }

        public virtual void AddResident(Human h)
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
