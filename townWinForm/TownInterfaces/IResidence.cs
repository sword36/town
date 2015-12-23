using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TownInterfaces
{
    public interface IResidence : IBuilding
    {
        //Rectangle Position { get; set; }
        void RemoveResident(ICitizen h);

        void AddResident(ICitizen h);

        PointF Room
        {
            get;
        }

        List<ICitizen> Residents
        {
            get;
        }

        bool HavePlace();
    }
}
