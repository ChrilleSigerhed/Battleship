using Sup20_12.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Sup20_12.ViewModels
{
   public class GameGrid : BaseViewModel
    {
        public int Id { get; set; }
        public string HitOrMiss { get; set; }

        public GameGrid(int id, string hitormiss)
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
