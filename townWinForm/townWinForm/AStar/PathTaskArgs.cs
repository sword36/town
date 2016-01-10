using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class PathTaskArgs
    {
        public int[,] matrix { get; }
        public Point start { get; }
        public Point finish { get; }

        public PathTaskArgs(int[,] m, Point s, Point f)
        {
            matrix = m;
            start = s;
            finish = f;
        }
    }
}
