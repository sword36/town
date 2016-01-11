using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownInterfaces;

namespace products
{
    public abstract class Thing : IThing
    {
        public float Price { get; set; }
        public float Weight { get; set; }
    }
}
