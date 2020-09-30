using Sup20_12.Interface;

namespace Sup20_12.ViewModels
{
    public class Ships : IShips
    {
        public string ShipType { get; set; }
        public int HitsTaken { get; set; }
        public int[] Longitude { get; set; }
        public int[] Latitude { get; set; }
    }
}
