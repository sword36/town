using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public abstract class Building : IDrawable
    {
        public abstract void Draw(Graphics g);
        public RectangleF Position { get; set; }
    }
}
