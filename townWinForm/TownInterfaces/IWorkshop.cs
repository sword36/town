using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TownInterfaces
{
    public interface IWorkshop : IBuilding
    {
        //Rectangle Position { get; set; }
        List<ICitizen> Workers
        {
            get;
        }

        int Count { get; }

        void AddWorker(ICitizen h);

        void RemoveWorker(ICitizen h);

        List<ICitizen> PeopleIn { get; set; }

        bool IsFree();
    }
}
