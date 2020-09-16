using Sup20_12.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12.Model
{
    public class SubmarineShip : Ships
    {

        public SubmarineShip(int longitude, int latitude, bool placeHorizontally)
        {
            ShipType = "SubmarineShip";
            HitsTaken = 0;
            if (placeHorizontally == true)
            {
                Latitude = new int[] { latitude };
                Longitude = new int[] { longitude - 1, longitude, longitude + 1 };
            } else 
            {
                Latitude = new int[] { latitude-1, latitude, latitude+1 };
                Longitude = new int[] { longitude };
            }

        }

        
    }
}
