using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12.Interface
{
    interface IShips
    {
        public string ShipType { get; set; }
        public int HitsTaken { get; set; }
        public int[] Longitude { get; set; }
        public int[] Latitude { get; set; }
    }
}
