using Sup20_12.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12.Model
{
    public class BattleShip : Ships
    {

        public BattleShip(int longitude, int latitude, bool placeHorizontally)
        {
            ShipType = "BattleShip";
            HitsTaken = 0;
            if (placeHorizontally == true)
            {
                Latitude = new int[] { latitude };
                Longitude = new int[] { longitude, longitude - 1 };
            }
            else
            {
                Latitude = new int[] { latitude, latitude - 1 };
                Longitude = new int[] { longitude };
            }
        }
    }
}
