using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12.ViewModels
{
    class GameEngineViewModel : IGameEngine
    {
        public Dictionary<SquaresInGrid, Ships> computerGrid { get; set; }
        public Dictionary<SquaresInGrid, Ships> playerGrid { get; set; }
        public Player player { get; set; }
        public GameEngineViewModel()
        {
            
        }
        public void StartTheGameWithSelectedPlayer(Player player)
        {
            this.player = player;
        }
        public void ComputerGrid(Dictionary<SquaresInGrid, Ships> computerGrid)
        {
            this.computerGrid = computerGrid;
        }
        public void PlayerGrid(Dictionary<SquaresInGrid, Ships> playerGrid)
        {
            this.playerGrid = playerGrid;
        }

    }
}
