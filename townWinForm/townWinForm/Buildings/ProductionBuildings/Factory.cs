﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public class Factory : ProductionBuilding
    {
        public Factory(int x, int y, int width, int height) : base(x, y, width, height)
        {

        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }

    }
}
