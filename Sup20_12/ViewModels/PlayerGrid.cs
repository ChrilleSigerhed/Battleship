using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12.ViewModels
{
    class PlayerGrid : GameEngineViewModel
    {
        public Dictionary<SquaresInGrid, Ships> SquaresAndShips { get; set; } = new Dictionary<SquaresInGrid, Ships>();
        private SquaresInGrid square;
        public PlayerGrid()
        {
            CreatePlayerGrid(square);
            var playerGrid = new GameEngineViewModel();
            playerGrid.PlayerGrid(SquaresAndShips);
        }
        public void CreatePlayerGrid(SquaresInGrid square)
        {
            for (int i = 0; i < 25; i++)
            {
                square = new SquaresInGrid();
                square.SquareId = i;
                SquaresAndShips.Add(square, null);
            }
        }
    }
}
