using Sup20_12.Model;
using Sup20_12.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
        private int gridSize = 7;
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
                PlayerShipsList.Add(new Destroyer(longitude, latitude));
                ShipsPlaced--;
                return true;
            }
            else
                return false;
        }
        public bool FillPlayerBattleShip(int longitude, int latitude)
        {
            if (longitude != 6)
            {
                if (ShipsPlaced > 0)
                {

                    BattleShip battleship = new BattleShip(longitude, latitude);
                    foreach (var c in PlayerShipsList)
                    {
                        foreach (var x in battleship.Longitude)
                        {
                            if (c.Longitude.Contains(x) && c.Latitude.Contains(latitude))
                            {
                                return false;
                            }
                        }
                    }
                    ShipsPlaced--;
                    PlayerShipsList.Add(battleship);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public bool FillPlayerSubmarineShip(int longitude, int latitude)
        {
            if (longitude != 0 && longitude != 6)
            {
                if (ShipsPlaced > 0)
                {

                    Submarine submarine = new Submarine(longitude, latitude);
                    foreach (var c in PlayerShipsList)
                    {
                        foreach (var x in submarine.Longitude)
                        {
                            if (c.Longitude.Contains(x) && c.Latitude.Contains(latitude))
                            {
                                return false;
                            }
                        }
                    }
                    ShipsPlaced--;
                    PlayerShipsList.Add(submarine);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public void FillComputerShips()
        {
            Random random = new Random();
            int longitude;
            int latitude;

            longitude = random.Next(0, 7);
            latitude = random.Next(0, 7);
            ComputerShipsList.Add(new Destroyer(longitude, latitude));
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    longitude = random.Next(0, 6);
                    latitude = random.Next(0, 7);
                    while (ComputerShipsList[i].Latitude.Contains(latitude) && ComputerShipsList[i].Longitude.Contains(longitude))
                    {
                        longitude = random.Next(0, 6);
                        latitude = random.Next(0, 7);
                    }
                    ComputerShipsList.Add(new BattleShip(longitude, latitude));
                }
                else
                {
                    longitude = random.Next(1, 6);
                    latitude = random.Next(0, 7);
                    while (ComputerShipsList[i].Latitude.Contains(latitude) && ComputerShipsList[i].Longitude.Contains(longitude))
                    {
                        longitude = random.Next(1, 6);
                        latitude = random.Next(0, 7);
                    }
                    ComputerShipsList.Add(new Submarine(longitude, latitude));
                }
            }
        }
        public int[] RandomFillPlayerShips()
        {
            Random random = new Random();
            int counter = 0;
            int longitude;
            int latitude;

            int[] buttonsLongitudeLatitude = new int[6 - PlayerShipsList.Count * 2];
            for (int i = PlayerShipsList.Count; i < 3; i++)
            {
                longitude = random.Next(0, gridSize);
                latitude = random.Next(0, gridSize);
                Submarine submarine = new Submarine(longitude, latitude);
                while (IsCollidingPlayer(submarine) == true)
                {
                    longitude = random.Next(0, gridSize);
                    latitude = random.Next(0, gridSize);
                    submarine = new Submarine(longitude, latitude);
                }
                buttonsLongitudeLatitude[counter] = longitude;
                buttonsLongitudeLatitude[counter + 1] = latitude;
                counter += 2;
                PlayerShipsList.Add(submarine);
            }
            return buttonsLongitudeLatitude;
        }
        public bool IsCollidingPlayer(Ships ship)
        {
            for (int i = 0; i < PlayerShipsList.Count; i++)
            {
                for (int j = 0; j < PlayerShipsList[i].Longitude.Length; j++)
                {
                    for (int y = 0; y < ship.Longitude.Length; y++)
                    {
                        if (PlayerShipsList[i].Longitude[j] == ship.Longitude[y])
                        {
                            for (int a = 0; a < PlayerShipsList[i].Latitude.Length; a++)
                            {
                                for (int c = 0; c < ship.Latitude.Length; c++)
                                {
                                    if (PlayerShipsList[i].Latitude[a] == ship.Latitude[c])
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
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                   GameGrid square = new GameGrid(i,j,""); 
                   PlayerButtonsInGame.Add(square);
                }
            }
        }
        public void CreateComputerGrid()
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    GameGrid square = new GameGrid(i, j, "");
                    ComputerButtonsInGame.Add(square);
                }
            }
        }
        public bool PlayerCheckHitOrMiss(int longitude, int latitude)
        {
            NumberOfMoves++;

            for (int i = 0; i < ComputerShipsList.Count; i++)
            {
                if (ComputerShipsList[i].Longitude.Contains(longitude) && ComputerShipsList[i].Latitude.Contains(latitude))
                {
                    ComputerShipsList[i].HitsTaken++;
                    if (ComputerShipsList[i].HitsTaken == 3 && ComputerShipsList[i].ShipType == "Submarine")
                    {
                        ComputerShipsList.RemoveAt(i);
                    }
                    else if (ComputerShipsList[i].HitsTaken == 2 && ComputerShipsList[i].ShipType == "BattleShip")
                    {
                        ComputerShipsList.RemoveAt(i);
                    }
                    else if (ComputerShipsList[i].HitsTaken == 1 && ComputerShipsList[i].ShipType == "Destroyer")
                    {
                        ComputerShipsList.RemoveAt(i);
                    }
                    return true;
                }

            }
            return false;

        }
        public bool PlayerCheckCloseOrNot(int longitude, int latitude)
        {
            foreach (var ship in ComputerShipsList)
            {
                if(ship.Longitude.Contains(longitude + 1) || ship.Longitude.Contains(longitude - 1) || ship.Longitude.Contains(longitude))
                {
                    if(ship.Latitude.Contains(latitude + 1) || ship.Latitude.Contains(latitude - 1) || ship.Latitude.Contains(latitude))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ComputerCheckCloseOrNot(int longitude, int latitude)
        {
            foreach (var ship in PlayerShipsList)
            {
                if (ship.Longitude.Contains(longitude + 1) || ship.Longitude.Contains(longitude - 1) || ship.Longitude.Contains(longitude))
                {
                    if (ship.Latitude.Contains(latitude + 1) || ship.Latitude.Contains(latitude - 1) || ship.Latitude.Contains(latitude))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool ComputerCheckHitOrMiss(int longitude , int latitude)
        {
            //for (int i = 0; i < PlayerShipsList.Count; i++)
            //{
            //    if (PlayerShipsList[i].Longitude.Contains(longitude) && PlayerShipsList[i].Latitude.Contains(latitude))
            //    {
            //        PlayerShipsList.RemoveAt(i);
            //        return true;
            //    }
            //}
            //return false;
            for (int i = 0; i < PlayerShipsList.Count; i++)
            {
                if (PlayerShipsList[i].Longitude.Contains(longitude) && PlayerShipsList[i].Latitude.Contains(latitude))
                {
                    PlayerShipsList[i].HitsTaken++;
                    if (PlayerShipsList[i].HitsTaken == 3 && PlayerShipsList[i].ShipType == "Submarine")
                    {
                        PlayerShipsList.RemoveAt(i);
                    }
                    else if (PlayerShipsList[i].HitsTaken == 2 && PlayerShipsList[i].ShipType == "BattleShip")
                    {
                        PlayerShipsList.RemoveAt(i);
                    }
                    else if (PlayerShipsList[i].HitsTaken == 1 && PlayerShipsList[i].ShipType == "Destroyer")
                    {
                        PlayerShipsList.RemoveAt(i);
                    }
                    return true;
                }

            }
            return false;
        }

        public int[] ComputerShotCloseToSplashSonar(int longitude, int latitude)
        {
            Random rand = new Random();
            int randomNumber;
            int newLongitude = 100;
            int newLatitude = 100;
            bool GridHasBeenShot = true;

            int counter = 0;
            int[] newShot;


            while (newLongitude < 0 || newLongitude > 6 || newLatitude < 0 || newLatitude > 6 || GridHasBeenShot == true)

            {
                    randomNumber = rand.Next(0, (gridSize+1));

                    if (randomNumber == 0)
                    {
                        newLongitude = longitude + 1;
                        newLatitude = latitude + 1;
                    }
                    else if (randomNumber == 1)
                    {
                        newLongitude = longitude;
                        newLatitude = latitude + 1;
                    }
                    else if (randomNumber == 2)
                    {
                        newLongitude = longitude - 1;
                        newLatitude = latitude + 1;
                    }
                    else if (randomNumber == 3)
                    {
                        newLongitude = longitude - 1;
                        newLatitude = latitude;
                    }
                    else if (randomNumber == 4)
                    {
                        newLongitude = longitude - 1;
                        newLatitude = latitude - 1;
                    }
                    else if (randomNumber == 5)
                    {
                        newLongitude = longitude;
                        newLatitude = latitude - 1;
                    }
                    else if (randomNumber == 6)
                    {
                        newLongitude = longitude + 1;
                        newLatitude = latitude - 1;
                    }
                    else if (randomNumber == 7)
                    {
                        newLongitude = longitude + 1;
                        newLatitude = latitude;
                    }
                if (HasGridBeenShot(PlayerButtonsInGame, newLongitude, newLatitude) == false)
                {
                    GridHasBeenShot = false;
                }

                counter++;
                if(counter == 8)
                {
                    newShot =  ComputerRandomShotFired();
                    return newShot;
                }


            }
            newShot = new int[] { newLongitude, newLatitude };
            return newShot;
        }

        public int[] ComputerRandomShotFired()
        {
            Random random = new Random();
            int longitude = random.Next(0, gridSize);
            int latitude = random.Next(0, gridSize);

            while (HasGridBeenShot(PlayerButtonsInGame, longitude, latitude) == true)
            {
                longitude = random.Next(0, gridSize);
                latitude = random.Next(0, gridSize);
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
                else if (latitude > (gridSize-1) || latitude < 0 || longitude < 0 || longitude > (gridSize-1))
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
