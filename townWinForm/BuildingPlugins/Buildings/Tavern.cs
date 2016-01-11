﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownInterfaces;

namespace BuildingPlugins
{
    public class Tavern : Building, IEntertainment, INotIgnore
    {
        public Tavern(int x, int y, int width, int height, string type) : base(x, y, width, height, type)
        {

        }


        public Tavern() { }
    }
}
