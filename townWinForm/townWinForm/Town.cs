using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class Town : IDrawable
    {
        private float dx = 0;
        private float dy = 0;

        public List<Human> Citizens;

        public Town()
        {
            Citizens = new List<Human>();
        }

        public void Update(int dt)
        {

        }

        public void Draw(Graphics g)
        {

        }

        private void UpdateD(float dx, float dy)
        {

        }
    }
}
