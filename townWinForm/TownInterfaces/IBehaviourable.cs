using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownInterfaces
{
    public interface IBehaviourable
    {
        void rest(int dt);
        void work(int dt);
    }
}
