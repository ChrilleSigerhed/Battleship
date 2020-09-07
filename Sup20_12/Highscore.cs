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

        public Highscore(int id, bool win, int numberOfMoves, int playerId)
        {
            Id = id;
            Win = win;
            NumberOfMoves = numberOfMoves;
            PlayerId = playerId;
        }
    }
}
