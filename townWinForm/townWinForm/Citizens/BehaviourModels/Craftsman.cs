using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm.BehaviourModels
{
    public class Craftsman : BehaviourModel
    {
        public StackFSM StateMachine;
        private Human body;

        public Craftsman(Human h)
        {
            body = h;
            StateMachine = new StackFSM();
        }

        public override void Update(int dt)
        {
            
        }
    }
}
