using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public interface IWorkshop
    {
        Rectangle Position { get; set; }
        List<Human> Workers
        {
            get;
        }

        void AddWorker(Human h);

        void RemoveWorker(Human h);

        bool IsFree();
    }
}
