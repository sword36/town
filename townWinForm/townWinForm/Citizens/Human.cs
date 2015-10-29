using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class Human : IDrawable, IUpdatable
    {
        public Human()
        {

        }

        public void Draw(Graphics g)
        {

        }

        public void Update(int dt)
        {
            Behaviour.Update(dt);
        }

        public BehaviourModel Behaviour;
    }
}
