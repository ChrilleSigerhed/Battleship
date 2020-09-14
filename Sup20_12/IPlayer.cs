using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12
{
    interface IPlayer
    {
        string Nickname { get; set; }
        public int Id { get; set; }
        public int NumberOfGamesPlayed { get; set; }
    }
}
