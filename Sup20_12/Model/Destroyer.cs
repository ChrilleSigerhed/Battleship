using Sup20_12.ViewModels;
using Sup20_12.Interface;

namespace Sup20_12.Model
{
    class Destroyer : Ships, IShips
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
