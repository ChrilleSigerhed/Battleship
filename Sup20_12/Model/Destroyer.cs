using Sup20_12.Interface;
using Sup20_12.ViewModels;

namespace Sup20_12.Model
{
    internal class Destroyer : Ships, IShips
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
