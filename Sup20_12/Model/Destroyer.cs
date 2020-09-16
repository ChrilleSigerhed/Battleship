using Sup20_12.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12.Model
{
    public class Destroyer : Ships
    {
        public Destroyer(int longitude, int latitude)
        {
            ShipType = "Destroyer";
            HitsTaken = 0;
            Longitude = new int[] { longitude };
            Latitude = new int[] { latitude };
        }
    }
}
