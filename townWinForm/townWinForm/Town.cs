using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class Town
    {
        public List<Human> Citizens;

        public Town()
        {
            Citizens = new List<Human>();
        }
    }
}
