using Sup20_12.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sup20_12.ViewModels
{
    public class GameEngine : BaseViewModel
    {
        public Dictionary<string, bool> PlayerShips { get; set; } = new Dictionary<string, bool>();

        public Dictionary<string, bool> ComputerShips { get; set; } = new Dictionary<string, bool>();
        public ObservableCollection<GameGrid> PlayerButtonsInGame { get; set; } = new ObservableCollection<GameGrid>();

        public int PlayerShipsToPlace { get; set; } = 3;

        public GameEngine()
        {
            
        }

        public bool FillPlayerShips(string button)
        {
            if(PlayerShipsToPlace != 0 && !PlayerShips.ContainsKey(button))
            {
                PlayerShips.Add(button, true);
                PlayerShipsToPlace--;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CheckHitOrMiss(string parameter, Dictionary<string, bool> ButtonsAndShips)
        {
        }

        public void ComputerRandomShotFired(ObservableCollection<GameGrid> PlayerButtonsInGame)
        {
            Random random = new Random();
            int numberFromRandom = random.Next(0, 24);

            while (PlayerButtonsInGame[numberFromRandom].HitOrMiss != "")
            {
                numberFromRandom = random.Next(0, 24);
            }
            CheckHitOrMiss(numberFromRandom.ToString(), PlayerShips);
        }

        public bool HasWon()
        {
            if(ComputerShips.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasLost()
        {
            if (PlayerShips.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
