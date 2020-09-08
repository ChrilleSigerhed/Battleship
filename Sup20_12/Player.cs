using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12
{
    class Player : IPlayer
    {
        public string Nickname { get; set; }
        public int Id { get; set; }

        public Player(string nickname)
        {
            Nickname = nickname;

        }

    }
}
