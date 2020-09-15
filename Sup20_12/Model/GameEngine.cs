using Sup20_12.Model;
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
        public List<Ships> PlayerShipsList { get; set; } = new List<Ships>();
        public List<Ships> ComputerShipsList { get; set; } = new List<Ships>();
        public ObservableCollection<GameGrid> PlayerButtonsInGame { get; set; } = new ObservableCollection<GameGrid>();
        public ObservableCollection<GameGrid> ComputerButtonsInGame { get; set; } = new ObservableCollection<GameGrid>();
        public Highscore NewHighscore { get; set; }
        public int NumberOfMoves { get; set; } = 0;
        #endregion
        public GameEngine()
        {
            CreatePlayerGrid();
            CreateComputerGrid();
            FillComputerShips();
        }
        
        public bool FillPlayerShips(int longitude, int latitude)
        {
            if(PlayerShipsList.Count < 3)
            {
                PlayerShipsList.Add(new Submarine(longitude, latitude));
                return true;
            }
            else
            {
                return false;
            }
        }
        public void FillComputerShips()
        {
            Random random = new Random() ;
            int longitude;
            int latitude;
            longitude = random.Next(0, 5);
            latitude = random.Next(0, 5);
            ComputerShipsList.Add(new Submarine(longitude, latitude));
            for (int i = 0; i < 2; i++)
            {
                longitude = random.Next(0, 5);
                latitude = random.Next(0, 5);
                while (ComputerShipsList[i].Latitude == longitude && ComputerShipsList[i].Longitude == latitude)
                {
                    longitude = random.Next(0, 5);
                    latitude = random.Next(0, 5);
                }
                ComputerShipsList.Add(new Submarine(longitude, latitude));
            }

        }
        public void CreatePlayerGrid()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                   GameGrid square = new GameGrid(i,j,""); 
                   PlayerButtonsInGame.Add(square);
                }
            }
        }
        public void CreateComputerGrid()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    GameGrid square = new GameGrid(i, j, "");
                    ComputerButtonsInGame.Add(square);
                }
            }
        }
        public bool PlayerCheckHitOrMiss(int longitude, int latitude)
        {
            NumberOfMoves++;
            foreach (var c in ComputerShipsList)
            {
                if(c.Latitude == longitude && c.Latitude == latitude)
                {
                    ComputerShipsList.Remove(c);
                    return true;
                }
            }
            return false;
           
        }
        public bool ComputerCheckHitOrMiss(int latitude, int longitude)
        {
            for (int i = 0; i < PlayerShipsList.Count; i++)
            {
                if (PlayerShipsList[i].Longitude == longitude && PlayerShipsList[i].Latitude == latitude)
                {
                    PlayerShipsList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public int[] ComputerRandomShotFired()
        {
            Random random = new Random();
            int longitude = random.Next(0, 5);
            int latitude = random.Next(0, 5);

            while (HasGridBeenShot(PlayerButtonsInGame, longitude, latitude) == true)
            {
                longitude = random.Next(0, 5);
                latitude = random.Next(0, 5);
            }
                int[] coordinates = { longitude, latitude };
                return coordinates;
        }

        public bool HasGridBeenShot(ObservableCollection<GameGrid> gameGrid, int longitude, int latitude)
        {
            foreach (var c in gameGrid)
            {
                if (c.Latitude == latitude && c.Longitude == longitude && c.IsClicked == true)
                {
                    return true;
                }
            }
            return false;
        }
        public bool HasWon()
        {
            if(ComputerShipsList.Count == 0)
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
            if (PlayerShipsList.Count == 0)
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
