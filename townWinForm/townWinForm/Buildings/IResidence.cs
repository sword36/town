using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm
{
    public interface IResidence
    {
        void RemoveResident(Human h);

        void AddResident(Human h);

        List<Human> Residents
        {
            get;
        }
    }
}
