using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Sup20_12
{
   public class Player : IPlayer
    {
        public string Nickname { get; set; }
        public int Id { get; set; }
        public long NumberOfGamesPlayed { get; set; }

        public Player(string nickname)
        {
            Nickname = nickname;

        }
        public override string ToString()
        {
            return Nickname;
        }

    }
}
