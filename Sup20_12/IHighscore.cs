using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12
{
    interface IHighscore
    {
        int Id { get; set; }
        public bool Win { get; set; }
        DateTime Date { get; set; }
        public int NumberOfMoves { get; set; }
        public int PlayerId { get; set; }
    }
}
