﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownInterfaces
{
    public interface IFood : IThing
    {
        float Energy { get; set; }
    }
}
