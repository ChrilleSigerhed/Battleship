using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Sup20_12.ViewModels
{
    interface IGameEngine
    {
        public Dictionary<SquaresInGrid,Ships> computerGrid { get; set; }
        public Dictionary<SquaresInGrid, Ships> playerGrid { get; set; }
        public Player player { get; set; }
    }
}
