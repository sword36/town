using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public interface IResidence
    {
        Rectangle Position { get; set; }
        void RemoveResident(Human h);

        void AddResident(Human h);

        List<Human> Residents
        {
            get;
        }

        bool HavePlace();
    }
}
