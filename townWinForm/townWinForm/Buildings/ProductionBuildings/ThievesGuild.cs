using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class ThievesGuild : ProductionBuilding
    {
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
    }
}
