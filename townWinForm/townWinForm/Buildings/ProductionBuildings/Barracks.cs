using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class Barracks : ProductionBuilding, IResidence
    {
        private List<Human> residents;
        public List<Human> Residents
        {
            get { return residents; }
        }

        public Barracks(int x, int y, int width, int height) : base(x, y, width, height)
        {

        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);
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
