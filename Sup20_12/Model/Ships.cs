﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12.ViewModels
{
    public class Ships
    {
        public string ShipType { get; set; }
        public int[] Longitude { get; set; }
        public int[] Latitude { get; set; }
        public int HitsTaken { get; set; }
    }
}
