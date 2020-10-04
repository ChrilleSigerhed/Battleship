namespace Sup20_12.Interface
{
    internal interface IShips
    {
        public string ShipType { get; set; }
        public int HitsTaken { get; set; }
        public int[] Longitude { get; set; }
        public int[] Latitude { get; set; }
    }
}
