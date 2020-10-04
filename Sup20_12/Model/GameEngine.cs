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
            if (ShipsPlaced > 0)
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
                    return false;
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
                    return false;
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
                    while (ComputerShipsList[i].Latitude.Contains(latitude) && ComputerShipsList[i].Longitude.Contains(longitude) ||
                        ComputerShipsList[i].Latitude.Contains(latitude) && ComputerShipsList[i].Longitude.Contains(longitude +1))
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
                    while (CheckIfShipCanBePlaced(longitude,latitude, ComputerShipsList) == true)
                    {
                        longitude = random.Next(1, 6);
                        latitude = random.Next(0, 7);
                    }
                    ComputerShipsList.Add(new Submarine(longitude, latitude));
                }
            }
        }
        private bool CheckIfShipCanBePlaced(int longitude, int latitude, List<Ships> ships)
        {
            if(         (ships[0].Latitude.Contains(latitude) && ships[0].Longitude.Contains(longitude)) ||
                        (ships[1].Latitude.Contains(latitude) && ships[1].Longitude.Contains(longitude)) ||
                        (ships[0].Longitude.Contains(longitude + 1) && ships[0].Latitude.Contains(latitude)) ||
                        (ships[0].Longitude.Contains(longitude - 1) && ships[0].Latitude.Contains(latitude)) ||
                        (ships[1].Longitude.Contains(longitude + 1) && ships[1].Latitude.Contains(latitude)) ||
                        (ships[1].Longitude.Contains(longitude - 1) && ships[1].Latitude.Contains(latitude)))
            {
                return true;
            }
            return false;
        }
        public void RandomFillPlayerShips()
        {
            Random random = new Random();
            int longitude;
            int latitude;

            longitude = random.Next(0, 7);
            latitude = random.Next(0, 7);
            PlayerShipsList.Add(new Destroyer(longitude, latitude));
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    longitude = random.Next(0, 6);
                    latitude = random.Next(0, 7);
                    while (PlayerShipsList[i].Latitude.Contains(latitude) && PlayerShipsList[i].Longitude.Contains(longitude) ||
                        PlayerShipsList[i].Latitude.Contains(latitude) && PlayerShipsList[i].Longitude.Contains(longitude+1))
                    {
                        longitude = random.Next(0, 6);
                        latitude = random.Next(0, 7);
                    }
                    PlayerShipsList.Add(new BattleShip(longitude, latitude));
                }
                else
                {
                    longitude = random.Next(1, 6);
                    latitude = random.Next(0, 7);
                    while (CheckIfShipCanBePlaced(longitude, latitude, PlayerShipsList) == true)
                    {
                        longitude = random.Next(1, 6);
                        latitude = random.Next(0, 7);
                    }
                    PlayerShipsList.Add(new Submarine(longitude, latitude));
                }
            }
        }
        public int[] GetLongitudesForRandomShip()
        {
            RandomFillPlayerShips();
            int[] longitudeShip = new int[3];
            for (int i = 0; i < PlayerShipsList.Count; i++)
            {
               
                if (i == 2)
                {
                    longitudeShip[i] = PlayerShipsList[i].Longitude[1];
                }
                else
                {
                    longitudeShip[i] = PlayerShipsList[i].Longitude[0];
                }
            }
            return longitudeShip;
        }
        public int[] GetLatitudesForRandomShip()
        {
            int[] latitudeShip = new int[3];
            for (int i = 0; i < PlayerShipsList.Count; i++)
            {
                latitudeShip[i] = PlayerShipsList[i].Latitude[0];
            }
            return latitudeShip;
        }


        private void CreatePlayerGrid() 
        { 
            for (int i = 0; i < gridSize; i++) 
            { for (int j = 0; j < gridSize; j++) 
                { GameGrid square = new GameGrid(i, j, ""); 
                    PlayerButtonsInGame.Add(square); 
                } 
            } 
        }

        private void CreateComputerGrid()
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
                        ComputerShipsList.RemoveAt(i);
                    else if (ComputerShipsList[i].HitsTaken == 2 && ComputerShipsList[i].ShipType == "BattleShip")
                        ComputerShipsList.RemoveAt(i);
                    else if (ComputerShipsList[i].HitsTaken == 1 && ComputerShipsList[i].ShipType == "Destroyer")
                        ComputerShipsList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool PlayerCheckCloseOrNot(int longitude, int latitude)
        {
            bool result = true;
            foreach (var ship in ComputerShipsList)
            {
                result = IsCloseToAnotherShip(longitude, latitude, ship);
            }
            return result;
        }

        public bool ComputerCheckCloseOrNot(int longitude, int latitude)
        {
            bool result = true;
            foreach (var ship in PlayerShipsList)
            {
                result = IsCloseToAnotherShip(longitude, latitude, ship);
            }
            return result;
        }

        private bool IsCloseToAnotherShip(int longitude, int latitude, Ships ship)
        {
            if (ship.Longitude.Contains(longitude + 1) || ship.Longitude.Contains(longitude - 1) || ship.Longitude.Contains(longitude))
            {
                if (ship.Latitude.Contains(latitude + 1) || ship.Latitude.Contains(latitude - 1) || ship.Latitude.Contains(latitude))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ComputerCheckHitOrMiss(int longitude, int latitude)
        {
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

        public bool ComputerCheckIfShipStillFloating(int longitude, int latitude)
        {
            foreach (var c in PlayerShipsList)
            {
                if (c.Longitude.Contains(longitude) && c.Latitude.Contains(latitude) && c.HitsTaken != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckIfAPlayerShipHasBeenHit()
        {
            foreach (var c in PlayerShipsList)
            {
                if(c.HitsTaken != 0)
                {
                    return true;
                }
            }
            return false;
        }
        public int[] GetCoordinatesOfPlayerShipAlreadyHit()
        {
            int[] TileShot;
            foreach (var c in PlayerShipsList)
            {
                if(c.HitsTaken != 0)
                {
                    foreach (var x in PlayerButtonsInGame)
                    {
                        if (c.Longitude.Contains(x.Longitude) && c.Latitude.Contains(x.Latitude) && x.HitOrMiss == "Träff!")
                        {
                            TileShot = new int[] { x.Longitude, x.Latitude };
                            return TileShot;
                        }
                    } 
                }
            }
            return null;
        }

        public int[] ComputerShootToSinkShip(int longitude, int latitude)
        {
            Random rand = new Random();
            int randomNumber;
            int newLongitude = 100;
            int newLatitude = 100;
            int[] newShot;
            bool GridHasBeenShot = true;

            while (newLongitude < 0 || newLongitude > 6 || newLatitude < 0 || newLatitude > 6 || GridHasBeenShot == true)

            {
                randomNumber = rand.Next(0, 4);

                if (randomNumber == 0)
                {
                    newLongitude = longitude + 1;
                    newLatitude = latitude;
                }
                else if (randomNumber == 1)
                {
                    newLongitude = longitude - 1;
                    newLatitude = latitude ;
                }else if (randomNumber == 2)
                {
                    newLongitude = longitude - 2;
                    newLatitude = latitude;
                } else if (randomNumber == 3)
                {
                    newLongitude = longitude + 2;
                    newLatitude = latitude;
                }
              
                if (HasGridBeenShot(PlayerButtonsInGame, newLongitude, newLatitude) == false)
                {
                    GridHasBeenShot = false;
                }
            }
            newShot = new int[] { newLongitude, newLatitude };
            return newShot;
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

        public bool PlayerHasWon()
        {
            if(ComputerShipsList.Count == 0)
                return true;
            else
                return false;
        }
        public bool PlayerHasLost()
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
