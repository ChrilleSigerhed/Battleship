using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Sup20_12.ViewModels
{
    internal interface IGameEngine
    {
        public List<Ships> PlayerShipsList { get; set; }
        public List<Ships> ComputerShipsList { get; set; }
        public ObservableCollection<GameGrid> PlayerButtonsInGame { get; set; }
        public ObservableCollection<GameGrid> ComputerButtonsInGame { get; set; }
        public Highscore NewHighscore { get; set; }
        public int NumberOfMoves { get; set; }
        public int ShipsPlaced { get; set; }
    }
}
