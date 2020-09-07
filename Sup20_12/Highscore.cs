using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12
{
    class Highscore : IHighscore
    {
        public int Id { get; set; }
        public bool Win { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfMoves { get; set; }
        public int PlayerId { get; set; }

        public Highscore(bool win, int numberOfMoves, int playerId)
        {
            Win = win;
            NumberOfMoves = numberOfMoves;
            PlayerId = playerId;
        }
    }
}
