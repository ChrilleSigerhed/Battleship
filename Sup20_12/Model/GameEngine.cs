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

        public bool FillPlayerDestroyer(int longitude, int latitude)
        {
            if (ShipsPlaced > 0)
            {
                
                for (int i = 0; i < PlayerShipsList.Count; i++)
                {
                    if (PlayerShipsList[i].Longitude.Contains(longitude) && PlayerShipsList[i].Latitude.Contains(latitude))
                    {
                        return false;
                    }
                }
                ShipsPlaced--;
                PlayerShipsList.Add(new Destroyer(longitude, latitude));
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool FillPlayerBattleShip(int longitude, int latitude)
        {
            if (longitude != 0)
            {
                if (ShipsPlaced > 0)
                {
                    
                    BattleShip battleship = new BattleShip(longitude, latitude, true);
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
            if (longitude != 0 && longitude != 4)
            {
                if (ShipsPlaced > 0)
                {
                    
                    SubmarineShip submarine = new SubmarineShip(longitude, latitude, true);
                    foreach (var c in PlayerShipsList)
                    {
                        foreach (var x in submarine.Longitude)
                        {
                            if(c.Longitude.Contains(x) && c.Latitude.Contains(latitude))
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
            Random random = new Random() ;
            int longitude;
            int latitude;
            longitude = random.Next(0, 4);
            latitude = random.Next(0, 4);
            ComputerShipsList.Add(new Destroyer(longitude, latitude));
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                longitude = random.Next(1, 4);
                latitude = random.Next(0, 4);
                while (ComputerShipsList[i].Latitude.Contains(longitude) && ComputerShipsList[i].Longitude.Contains(latitude))
                {
                    longitude = random.Next(1, 4);
                    latitude = random.Next(0, 4);
                }
                    ComputerShipsList.Add(new BattleShip(longitude, latitude, true));
                }
                else
                {
                    longitude = random.Next(1, 3);
                    latitude = random.Next(0, 4);
                    while (ComputerShipsList[i].Latitude.Contains(longitude) && ComputerShipsList[i].Longitude.Contains(latitude))
                    {
                        longitude = random.Next(1, 3);
                        latitude = random.Next(0, 4);
                    }
                    ComputerShipsList.Add(new SubmarineShip(longitude, latitude, true));
                }
            }

        }

        public bool IsColliding(Ships ship)
        {
            for (int i = 0; i < ComputerShipsList.Count; i++)
            {
                for (int j = 0; j < ComputerShipsList[i].Longitude.Length; j++)
                {
                    for (int y = 0; y < ship.Longitude.Length; y++)
                    {
                        if(ComputerShipsList[i].Longitude[j] == ship.Longitude[y])
                        {
                            for (int a = 0; a < ComputerShipsList[i].Latitude.Length; a++)
                            {
                                for (int c = 0; c < ship.Latitude.Length; c++)
                                {
                                    if(ComputerShipsList[i].Latitude[a] == ship.Latitude[c])
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
            //foreach (var c in ComputerShipsList)
            //{
            //    if(c.Longitude.Contains(longitude) && c.Latitude.Contains(latitude))
            //    {
            //        ComputerShipsList.Remove(c); 
            //        return true;
            //    }
            //}
            for (int i = 0; i < ComputerShipsList.Count; i++)
            {
                if (ComputerShipsList[i].Longitude.Contains(longitude) && ComputerShipsList[i].Latitude.Contains(latitude))
                {
                    ComputerShipsList[i].HitsTaken++;
                    if (ComputerShipsList[i].HitsTaken == 3 && ComputerShipsList[i].ShipType == "SubmarineShip")
                    {
                        ComputerShipsList.RemoveAt(i);
                    }
                    else if (ComputerShipsList[i].HitsTaken == 2 && ComputerShipsList[i].ShipType == "BattleShip")
                    {
                        ComputerShipsList.RemoveAt(i);
                    } else if (ComputerShipsList[i].HitsTaken == 1 && ComputerShipsList[i].ShipType == "Destroyer")
                    {
                        ComputerShipsList.RemoveAt(i);
                    }
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
                    if (PlayerShipsList[i].HitsTaken == 3 && PlayerShipsList[i].ShipType == "SubmarineShip")
                    {
                    PlayerShipsList.RemoveAt(i);
                    } else if (PlayerShipsList[i].HitsTaken == 2 && PlayerShipsList[i].ShipType == "BattleShip")
                    {
                        PlayerShipsList.RemoveAt(i);
                    } else if (PlayerShipsList[i].HitsTaken == 1 && PlayerShipsList[i].ShipType == "Destroyer")
                    {
                        PlayerShipsList.RemoveAt(i);
                    }
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
