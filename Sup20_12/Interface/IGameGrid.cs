using System.Windows.Media;

namespace Sup20_12.Interface
{
    interface IGameGrid
    {
        public int Longitude { get; set; }
        public int Latitude { get; set; }
        public string HitOrMiss { get; set; }
        public ImageBrush backgroundImage { get; set; }
        public bool IsClicked { get; set; }
    }
}
