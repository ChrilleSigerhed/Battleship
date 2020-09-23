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
        public int ShipsPlaced { get; set; } = 3;
        #endregion
        public GameEngine()
        {
            CreatePlayerGrid();
            CreateComputerGrid();
            FillComputerShips();
        }
        
        public bool FillPlayerShips(int longitude, int latitude)
        {
            if(ShipsPlaced > 0)
            {
                for (int i = 0; i < PlayerShipsList.Count; i++)
                {
                    if (PlayerShipsList[i].Longitude.Contains(longitude) && PlayerShipsList[i].Latitude.Contains(latitude))
                        return false;
                }
                PlayerShipsList.Add(new Submarine(longitude, latitude));
                ShipsPlaced--;
                return true;
            }
            else
                return false;
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
                Submarine submarine = new Submarine(longitude, latitude);
                while (IsColliding(submarine) == true)
                {
                    longitude = random.Next(0, 5);
                    latitude = random.Next(0, 5);
                    submarine = new Submarine(longitude, latitude);
                }
                ComputerShipsList.Add(submarine);
            }
        }
        public int[] RandomFillPlayerShips()
        {
            int[] buttonsLongitudeLatitude = new int[6];
            int counter = 2;
            Random random = new Random();
            int longitude;
            int latitude;
            longitude = random.Next(0, 5);
            latitude = random.Next(0, 5);
            buttonsLongitudeLatitude[0] = longitude;
            buttonsLongitudeLatitude[1] = latitude;
            
            PlayerShipsList.Add(new Submarine(longitude, latitude));

            for (int i = 0; i < 2; i++)
            {
                longitude = random.Next(0, 5);
                latitude = random.Next(0, 5);
                Submarine submarine = new Submarine(longitude, latitude);
                while (IsColliding(submarine) == true)
                {
                    longitude = random.Next(0, 5);
                    latitude = random.Next(0, 5);
                    submarine = new Submarine(longitude, latitude);
                }
                buttonsLongitudeLatitude[counter] = longitude;
                buttonsLongitudeLatitude[counter+1] = latitude;
                counter += 2;
                PlayerShipsList.Add(submarine);
            }
            return buttonsLongitudeLatitude;
        }
        public bool IsColliding(Ships ship)
        {
            for (int i = 0; i < ComputerShipsList.Count; i++)
            {
                for (int j = 0; j < ComputerShipsList[i].Longitude.Length; j++)
                {
                    for (int y = 0; y < ship.Longitude.Length; y++)
                    {
                        if (ComputerShipsList[i].Longitude[j] == ship.Longitude[y])
                        {
                            for (int a = 0; a < ComputerShipsList[i].Latitude.Length; a++)
                            {
                                for (int c = 0; c < ship.Latitude.Length; c++)
                                {
                                    if (ComputerShipsList[i].Latitude[a] == ship.Latitude[c])
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
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
                if(c.Longitude.Contains(longitude) && c.Latitude.Contains(latitude))
                {
                    ComputerShipsList.Remove(c); 
                    return true;
                }
            }
            return false;
           
        }
        public bool PlayerCheckCloseOrNot(int longitude, int latitude)
        {
            foreach (var ship in ComputerShipsList)
            {
                if(ship.Longitude.Contains(longitude +1) || ship.Longitude.Contains(longitude-1) || ship.Longitude.Contains(longitude))
                {
                    if(ship.Latitude.Contains(latitude+1) || ship.Latitude.Contains(latitude-1) || ship.Latitude.Contains(latitude))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool ComputerCheckHitOrMiss(int longitude , int latitude)
        {
            for (int i = 0; i < PlayerShipsList.Count; i++)
            {
                if (PlayerShipsList[i].Longitude.Contains(longitude) && PlayerShipsList[i].Latitude.Contains(latitude))
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
                    return true;
            }
            return false;
        }

        public bool HasWon()
        {
            if(ComputerShipsList.Count == 0)
                return true;
            else
                return false;
        }
        public bool HasLost()
        {
            if (PlayerShipsList.Count == 0)
                return true;
            else
                return false;
        }

        public Highscore AddNewHighscore(bool hasWon, int playerId)
        {
            Highscore myHighscore;
            NewHighscore = new Highscore(hasWon, NumberOfMoves, playerId);
            return myHighscore = DbConnection.AddOneHighscoreToDb(NewHighscore);
        }

    }
}
