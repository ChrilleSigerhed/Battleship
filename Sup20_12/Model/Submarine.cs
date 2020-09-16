using Sup20_12.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12.Model
{
    class Submarine : Ships
    {
        public string ShipType { get; set; }

        public Submarine(int longitude, int latitude)
        {
            ShipType = "Submarine";
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
