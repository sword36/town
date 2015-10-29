using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm
{
    public abstract class BehaviourModel : IUpdatable
    {
        public abstract void Update(int dt);
    }
}
