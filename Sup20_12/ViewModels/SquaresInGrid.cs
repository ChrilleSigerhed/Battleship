using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12.ViewModels
{
   public class SquaresInGrid
    {
        public int Id { get; set; }
        public string HitOrMiss { get; set; }

        public SquaresInGrid(int id, string hitormiss)
        {
            this.Id = id;
            this.HitOrMiss = hitormiss;
        }
        public override string ToString()
        {
            return HitOrMiss;
        }

    }
    
}
