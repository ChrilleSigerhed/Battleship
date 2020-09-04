using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12
{
    class Player : IPlayer
    {
        public string Nickname { get; set; }
        public int Id { get; set; }

        public List<IHighscore> MyHighscoreList { get; set; }

        public Player(string nickname)
        {
            Nickname = nickname;
            //MyHighscoreList = GetHighscoreList(); När GetHighscoreList är implementerad ska denna kodrad in för att ge spelaren dess highscore.
        }

    }
}
