using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class ThievesGuild : Building, IResidence, IWorkshop
    {
        private List<Human> workers;

        public List<Human> Workers
        {
            get { return workers; }
        }

        private List<Human> residents;
        public List<Human> Residents
        {
            get { return residents; }
        }

        public ThievesGuild(int x, int y, int width, int height, string type) : base(x, y, width, height, type)
        {
            residents = new List<Human>();
            workers = new List<Human>();
        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }

        public void AddWorker(Human h)
        {
            workers.Add(h);
            h.WorkBuilding = this;
        }

        public void RemoveWorker(Human h)
        {
            workers.Remove(h);
        }

        public void RemoveResident(Human h)
        {
            residents.Remove(h);
        }

        public void AddResident(Human h)
        {
            residents.Add(h);
            h.Home = this;
        }

        public bool IsFree()
        {
            return true;
        }

        public bool HavePlace()
        {
            return true;
        }
    }
}
