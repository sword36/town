using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingInterfaces
{
    public interface IResidence
    {

        Rectangle Position { get; set; }
        void RemoveResident(Human h);

        void AddResident(Human h);

        PointF Room
        {
            get;
        }

        List<Human> Residents
        {
            get;
        }

        bool HavePlace();
    }
}
