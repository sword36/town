using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class Farm : Building, IWorkshop
    {

        private List<Human> workers;

        public int Count
        {
            get { return Workers.Count; }
        }
        public List<Human> Workers
        {
            get { return workers; }
        }
        public Farm(int x, int y, int width, int height, string type) : base(x, y, width, height, type)
        {
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

        public bool IsFree()
        {
            return workers.Count < Config.MaxWorkers;
        }

        public static bool operator <(Farm w1, Farm w2)
        {
            return w1.Count < w2.Count;
        }

        public static bool operator >(Farm w1, Farm w2)
        {
            return w1.Count > w2.Count;
        }
    }
}
