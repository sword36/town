using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class ThievesGuild : ProductionBuilding, IResidence
    {
        private List<Human> residents;
        public List<Human> Residents
        {
            get { return residents; }
        }

        public ThievesGuild(int x, int y, int width, int height) : base(x, y, width, height)
        {

        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }

        public override bool AddWorker(Human h)
        {
            
            if (h.CurrentProf != "thief")
            {
                return false;
            }

            Workers.Add(h);
            h.WorkBuilding = this;
            return true;
        }

        public void RemoveResident(Human h)
        {
            residents.Remove(h);
        }

        public void AddResident(Human h)
        {
            residents.Add(h);
        }
    }
}
