using Sup20_12.Interface;
using Sup20_12.ViewModels;

namespace Sup20_12.Model
{
    internal class Submarine : Ships, IShips
    {


        public Submarine(int longitude, int latitude)
        {
            ShipType = "Submarine";
            HitsTaken = 0;
            Latitude = new int[] { latitude };
            Longitude = new int[] { longitude - 1, longitude, longitude + 1 };
        }
    }
}
