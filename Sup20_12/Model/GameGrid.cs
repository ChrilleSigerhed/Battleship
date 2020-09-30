using Sup20_12.ViewModels.Base;
using System.Windows.Media;
using Sup20_12.Interface;

namespace Sup20_12.ViewModels
{
   public class GameGrid : BaseViewModel, IGameGrid
    {
        public int Longitude { get; set; }
        public int Latitude { get; set; }
        public string HitOrMiss { get; set; }
        public ImageBrush backgroundImage { get; set; } = new ImageBrush();
        public bool IsClicked { get; set; } = false;

        public GameGrid(int longitude, int latitude, string hitormiss)
        {
            Longitude = longitude;
            Latitude = latitude;
            HitOrMiss = hitormiss;
        }
        public override string ToString()
        {
            return HitOrMiss;
        }

    }
}
