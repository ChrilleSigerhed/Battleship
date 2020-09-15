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
        #region Properties
        public Dictionary<string, bool> PlayerShips { get; set; } = new Dictionary<string, bool>();
        public Dictionary<string, bool> ComputerShips { get; set; } = new Dictionary<string, bool>();
        public ObservableCollection<GameGrid> PlayerButtonsInGame { get; set; } = new ObservableCollection<GameGrid>();
        public ObservableCollection<GameGrid> ComputerButtonsInGame { get; set; } = new ObservableCollection<GameGrid>();
        public Highscore NewHighscore { get; set; }
        public int NumberOfMoves { get; set; } = 0;
        public int PlayerShipsToPlace { get; set; } = 3;
        #endregion
        public GameEngine()
        {
            CreatePlayerGrid();
            CreateComputerGrid();
            FillComputerShips();
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
        public void FillComputerShips()
        {
            Random random;
            int numberFromRandom;
            for (int i = 0; i < 3; i++)
            {
                random = new Random();
                numberFromRandom = random.Next(0, 24);
                while (ComputerShips.ContainsKey(numberFromRandom.ToString()))
                {
                    numberFromRandom = random.Next(0, 24);
                }
                ComputerShips.Add(numberFromRandom.ToString(), true);
            }

        }
        public void CreatePlayerGrid()
        {
            for (int i = 0; i < 25; i++)
            {
                GameGrid square = new GameGrid(i, "");
                PlayerButtonsInGame.Add(square);
            }
        }
        public void CreateComputerGrid()
        {
            for (int i = 0; i < 25; i++)
            {
                GameGrid square = new GameGrid(i, "");
                ComputerButtonsInGame.Add(square);
            }
        }
        public bool PlayerCheckHitOrMiss(string button, Dictionary<string, bool> ButtonsAndShips)
        {
            NumberOfMoves++;
            if (ComputerShips.ContainsKey(button.ToString()))
            {
                ComputerShips.Remove(button);
                return true;

            }
            else
            {
                return false;
            }
        }
        public bool ComputerCheckHitOrMiss(string button, Dictionary<string, bool> ButtonsAndShips)
        {
            if (PlayerShips.ContainsKey(button.ToString()))
            {
                PlayerShips.Remove(button);
                return true;

            }
            else
            {
                return false;
            }
        }
        public int ComputerRandomShotFired(ObservableCollection<GameGrid> PlayerButtonsInGame)
        {
            Random random = new Random();
            int numberFromRandom = random.Next(0, 3);

            while (PlayerButtonsInGame[numberFromRandom].HitOrMiss == "Träff" || PlayerButtonsInGame[numberFromRandom].HitOrMiss == "Miss") // Ful lösning
            {
                numberFromRandom = random.Next(0, 3);
            }
            return numberFromRandom;
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
        public void AddNewHighscoreWin(int playerId)
        {
            NewHighscore = new Highscore(true, NumberOfMoves, playerId );
            DbConnection.AddOneHighscoreToDb(NewHighscore);
        }
        public void AddNewHighscoreLost(int playerId)
        {
            NewHighscore = new Highscore(false, NumberOfMoves, playerId);
            DbConnection.AddOneHighscoreToDb(NewHighscore);
        }

    }
}
