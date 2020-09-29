using Sup20_12.ViewModels;

namespace Sup20_12.Model
{
   public class BattleShip : Ships
    {
        public BattleShip(int longitude, int latitude)
        {
            ShipType = "BattleShip";
            HitsTaken = 0;
            Latitude = new int[] { latitude };
            Longitude = new int[] { longitude, longitude + 1 };
        }
    }
}
