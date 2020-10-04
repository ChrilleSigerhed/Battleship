using Sup20_12.View;
using Sup20_12.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sup20_12.ViewModels
{
   public class GameWindowViewModel : BaseViewModel
    {

        #region Properties
        public ICommand PlaceShip { get; set; }
        public ICommand CheckIfShip { get; set; }
        public ICommand GoToMainPageCommand { get; set; }
        public ICommand PlaceRandomBoats { get; set; }
        public int ShowNumberOfMoves { get; private set; }
        public string ShowPlayerNickname { get; private set; }
        public int Ships { get; private set; } = 3;
        public ObservableCollection<GameGrid> PlayerButtonsInGame { get; set; }  = new ObservableCollection<GameGrid>();
        public ObservableCollection<GameGrid> ComputerButtonsInGame { get; set; } = new ObservableCollection<GameGrid>();
        public List<int> PlayerShotsFired { get; set; } = new List<int>();

        private int noMoreShipsToUse = 0;
        private SingleBoatUC Destroyer { get; set; }
        private BattleShipUC BattleShip { get; set; }
        private SubmarineUC Submarine { get; set; }
        private GameEngine MyGameEngine { get; set; } = new GameEngine();
        private bool PlayerTurn { get; set; } = false;
        private bool WasCloseToShip { get; set; } = false;
        private bool ComputerHitShip { get; set; } = false;
        private int[] CoordinatesCloseToShip { get; set; }
        private int [] CoordinatesHitShip { get; set; }
        private int delayTime = 1000;
        #endregion
        public GameWindowViewModel(SingleBoatUC destroyer, BattleShipUC battleship, SubmarineUC submarine)
        {
            Destroyer = destroyer;
            BattleShip = battleship;
            Submarine = submarine;
            ComputerButtonsInGame = MyGameEngine.ComputerButtonsInGame;
            PlayerButtonsInGame = MyGameEngine.PlayerButtonsInGame;
            PlaceShip = new RelayPropertyCommand(PlayerPlaceShips);
            CheckIfShip = new RelayPropertyCommand(PlayerCheckHitOrMiss);
            GoToMainPageCommand = new RelayCommand(AskIfExitCurrentRound);
            PlaceRandomBoats = new RelayCommand(RandomPlacePlayerShips);
            ShowPlayerNickname = Global.MyPlayer.Nickname;
            ShowNumberOfMoves = MyGameEngine.NumberOfMoves;
        }
      
        public void PlayerPlaceShips(string button)
        {
            int buttonToNumber = int.Parse(button);
            if (PlayerHasShipsLeftToPlace(buttonToNumber))
            {
                SubtractDestroyerToPlace(buttonToNumber);
                PlayerHasNoMoreShipsToPlace();
            }
            else
                MessageBoxYouCantPlaceShipsThere();
        }

        private void MessageBoxYouCantPlaceShipsThere()
        {
            MessageBox.Show("Det går inte att placera ett skepp där.");
        }

        private void MessageBoxYouCanNowPlay()
        {
            if (Ships == 0)
                MessageBox.Show("Nu kan spelet börja, du spelar på den högra skärmen");
        }

        private void SubtractDestroyerToPlace(int buttonToNumber)
        {
            Destroyer.PlacedBoats--;
            Ships--;
            foreach (var myGameButton in PlayerButtonsInGame)
            {
                PlayerPlaceDestroyer(myGameButton, buttonToNumber);
            }
        }
        
        private void PlayerPlaceDestroyer(GameGrid myGameButton, int buttonToNumber)
        {
            if (PlayerButtonsInGame[buttonToNumber].Longitude == myGameButton.Longitude && PlayerButtonsInGame[buttonToNumber].Latitude == myGameButton.Latitude)
                ChangePlayerGridToSingleBoat(myGameButton);
        }

        private void PlayerHasNoMoreShipsToPlace()
        {
            if (Ships == noMoreShipsToUse)
            {
                ChangePlayerTurn();
                MessageBoxYouCanNowPlay();
            }
        }

        private bool PlayerHasShipsLeftToPlace(int buttonToNumber)
        {
            bool result = false;
            if (MyGameEngine.FillPlayerShips(PlayerButtonsInGame[buttonToNumber].Longitude, PlayerButtonsInGame[buttonToNumber].Latitude))
                result = true;
            return result;
        }

        public void PlayerPlaceSubmarineShip(string button)
        {
            int buttonToNumber = int.Parse(button);
            if (MyGameEngine.FillPlayerSubmarineShip(PlayerButtonsInGame[buttonToNumber].Longitude, PlayerButtonsInGame[buttonToNumber].Latitude) == true)
            {
                SubtractSubmarineToPlace(buttonToNumber);
                PlayerHasNoMoreShipsToPlace();
            }
            else
                MessageBoxYouCantPlaceShipsThere();
        }

        private void SubtractSubmarineToPlace(int buttonToNumber)
        {
            Ships--;
            Submarine.PlacedBoats--;
            foreach (var myGameButton in PlayerButtonsInGame)
            {
                ShouldShipBeSubmarine(myGameButton, buttonToNumber);
            }
        }

        private void ShouldShipBeSubmarine(GameGrid myGameButton, int buttonToNumber)
        {
            if (PlayerButtonsInGame[buttonToNumber].Longitude == myGameButton.Longitude && PlayerButtonsInGame[buttonToNumber].Latitude == myGameButton.Latitude)
                ChangePlayerGridToSubmarine(myGameButton.Longitude, myGameButton.Latitude, myGameButton);
        }

        public void PlayerPlaceBattleShip(string button)
        {
            int buttonToNumber = int.Parse(button);
            if (MyGameEngine.FillPlayerBattleShip(PlayerButtonsInGame[buttonToNumber].Longitude, PlayerButtonsInGame[buttonToNumber].Latitude) == true)
            {
                SubtractBattleShipToPlace(buttonToNumber);
                MessageBoxYouCanNowPlay();
            }
            else
                MessageBoxYouCantPlaceShipsThere();
        }

        private void SubtractBattleShipToPlace(int buttonToNumber)
        {
            Ships--;
            BattleShip.PlacedBoats--;
            foreach (var myGameButton in PlayerButtonsInGame)
            {
                ShouldShipBeBattleShip(myGameButton, buttonToNumber);
            }
        }

        private void ShouldShipBeBattleShip(GameGrid myGameButton, int buttonToNumber)
        {
            if (PlayerButtonsInGame[buttonToNumber].Longitude == myGameButton.Longitude && PlayerButtonsInGame[buttonToNumber].Latitude == myGameButton.Latitude)
                ChangePlayerGridToBattleShip(myGameButton.Longitude, myGameButton.Latitude, myGameButton);
        }

        //-------------------------------------------------------------------------------------------------
        private void RandomPlacePlayerShips()
        {
            if (Ships == 3)
            {
                int[] longitudeShips = MyGameEngine.GetLongitudesForRandomShip();
                int[] latitudeShips = MyGameEngine.GetLatitudesForRandomShip();
                foreach (var myGameButton in PlayerButtonsInGame)
                {
                    ChangeGridToAShip(myGameButton, latitudeShips, longitudeShips);
                }
                Ships = 0;
                PlayerHasNoMoreShipsToPlace();
            }
        }

        private void ChangeGridToAShip(GameGrid myGameButton, int[] latitudeShips, int[] longitudeShips)
        {
            if (myGameButton.Longitude == longitudeShips[0] && myGameButton.Latitude == latitudeShips[0])
                ChangePlayerGridToSingleBoat(myGameButton);
            else if (myGameButton.Longitude == longitudeShips[1] && myGameButton.Latitude == latitudeShips[1])
                ChangePlayerGridToBattleShip(myGameButton.Longitude, myGameButton.Latitude, myGameButton);
            else if (myGameButton.Longitude == longitudeShips[2] && myGameButton.Latitude == latitudeShips[2])
                ChangePlayerGridToSubmarine(myGameButton.Longitude, myGameButton.Latitude, myGameButton);
        }

        private void PlayerCheckHitOrMiss(string button)
        {
            int buttonToNumber = int.Parse(button);
            if (HasBeenShotAtAlready(buttonToNumber) && PlayerTurn)
                MessageBox.Show("Du har redan skjutit där!");
            else if (HitComputerShip(buttonToNumber) && PlayerTurn)
            {
                AddHitOnComputerBoard(buttonToNumber);
                ChangePlayerTurn();
                CheckIfPlayerHasWon();
            }
            else if (PlayerTurn)
            {
                AddCloseOrMissOnComputerBoard(buttonToNumber);
                ChangePlayerTurn();
                DelayBeforeComputerMakesAMove();
            }
        }

        private void CheckIfPlayerHasWon()
        {
            if (MyGameEngine.PlayerHasWon())
                PlayerHasWon();
            else
                DelayBeforeComputerMakesAMove();
        }

        private void PlayerHasWon()
        {
            Highscore myHighscore = MyGameEngine.AddNewHighscore(true, Global.MyPlayer.Id);
            ShowWinDialogueBox(myHighscore);
        }

        private void DelayBeforeComputerMakesAMove()
        {
            Task.Delay(delayTime).ContinueWith(t => ComputerHitOrMiss());
        }

        private void ShowWinDialogueBox(Highscore myHighscore)
        {
            MessageBoxResult result = MessageBox.Show($"Grattis {Global.MyPlayer.Nickname}, du vann{DidPlayerMakeTheHighscoreString(myHighscore)}", "Avsluta", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Global.MyWin.frame.Content = new GameWindowPage();
                    break;
                case MessageBoxResult.No:
                    Global.MyWin.frame.Content = new MainMenuPage();
                    break;
            }
        }
        private string DidPlayerMakeTheHighscoreString(Highscore myHighscore)
        {
            string returnString;
            IEnumerable<Highscore> myHighscoreList = DbConnection.GetThreeWinnersFromHighscore();
            if (IsPlayersHighscoreIdOnTheList(myHighscoreList, myHighscore))
                returnString = " och tog dig dessutom in på highscorelistan! Vill du spela igen?";
            else
                returnString = "! Vill du spela igen?";
            return returnString;
        }

        private bool IsPlayersHighscoreIdOnTheList(IEnumerable<Highscore> myHighscoreList, Highscore myHighscore)
        {
            bool result = false;
            foreach (Highscore highscore in myHighscoreList)
            {
                if (myHighscore.Id == highscore.Id)
                    result = true;
            }
            return result;
        }

        private void AddCloseOrMissOnComputerBoard(int buttonToNumber)
        {
            UpdateNumberOfMovesOnGameboard();
            PlayerShotsFired.Add(buttonToNumber);

            if (MyGameEngine.PlayerCheckCloseOrNot(ComputerButtonsInGame[buttonToNumber].Longitude, ComputerButtonsInGame[buttonToNumber].Latitude) == true)
            {
                ComputerButtonsInGame[buttonToNumber].HitOrMiss = "Nära";
                ChangeGridSquareToSplashSonarImage(ComputerButtonsInGame[buttonToNumber]);
            }
            else
            {
                ComputerButtonsInGame[buttonToNumber].HitOrMiss = "Miss";
                ChangeToSplashImage(ComputerButtonsInGame[buttonToNumber]);
            }
        }

        private void AddCloseOnPlayerBoard(int longitude, int latitude)
        {
            if(MyGameEngine.ComputerCheckCloseOrNot(longitude, latitude))
                CheckGameGridForCloseHit(longitude, latitude);
        }

        private void CheckGameGridForCloseHit(int longitude, int latitude)
        {
            foreach (var myPlayerButton in PlayerButtonsInGame)
            {
                ChangeToSonarSplashImageIfCloseToPlayerShip(myPlayerButton, longitude, latitude);
            }
        }

        private void ChangeToSonarSplashImageIfCloseToPlayerShip(GameGrid myPlayerButton, int longitude, int latitude)
        {
            if (myPlayerButton.Longitude == longitude && myPlayerButton.Latitude == latitude)
            {
                ChangeGridSquareToSplashSonarImage(myPlayerButton);
                WasCloseToShip = true;
                ChangePlayerTurn();
                myPlayerButton.IsClicked = true;
            }
        }

        private void ComputerShootAroundSplashSonar()
        {
            int[] shoot = MyGameEngine.ComputerShotCloseToSplashSonar(CoordinatesCloseToShip[0], CoordinatesCloseToShip[1]);

            if (MyGameEngine.ComputerCheckHitOrMiss(shoot[0], shoot[1]))
            {
                CheckIfComputerHit(shoot);
                CheckIfPlayerLost();
                ChangePlayerTurn();
            } 
            else if (MyGameEngine.ComputerCheckCloseOrNot(shoot[0], shoot[1]))
                AddCloseOnPlayerBoard(shoot[0], shoot[1]);
            else
            {
                CheckIfComputerMiss(shoot);
                ChangePlayerTurn();
            }
        }

        private void CheckIfComputerMiss(int[] shoot)
        {
            foreach (var myPlayerButton in PlayerButtonsInGame)
            {
                if (myPlayerButton.Longitude == shoot[0] && myPlayerButton.Latitude == shoot[1])
                {
                    myPlayerButton.HitOrMiss = "Miss!";
                    ChangeToSplashImage(myPlayerButton);
                    myPlayerButton.IsClicked = true;
                }
            }
        }

        private void CheckIfComputerHit(int[] shoot)
        {
            foreach (var myPlayerButton in PlayerButtonsInGame)
            {
                if (myPlayerButton.Longitude == shoot[0] && myPlayerButton.Latitude == shoot[1])
                    ComputerHitPlayerShip(shoot, myPlayerButton);
            }
        }
        private void CheckIfPlayerLost()
        {
            if (MyGameEngine.PlayerHasLost())
            {
                MyGameEngine.AddNewHighscore(false, Global.MyPlayer.Id);
                ShowLosingDialogueBox();
            }
        }

        private void ComputerHitPlayerShip(int[] shoot, GameGrid myPlayerButton)
        {
            myPlayerButton.HitOrMiss = "Träff!";
            CoordinatesHitShip = new int[] { shoot[0], shoot[1] };
            ChangeGridSquareToExplosionImage(myPlayerButton);
            myPlayerButton.IsClicked = true;
            WasCloseToShip = false;
            ComputerHitShip = true;
        }

        
        public void ComputerShootToSinkShip(int[] shoot)
        {
            if (MyGameEngine.ComputerCheckIfShipStillFloating(shoot[0], shoot[1]))
                CheckComputerTurn(shoot);
            else if(MyGameEngine.ComputerCheckIfShipStillFloating(shoot[0], shoot[1]) == false)
            {
                ComputerHitShip = false;
                ComputerHitOrMiss();
            }
        }

        private void CheckComputerTurn(int[] shoot)
        {
            int[] newShot = MyGameEngine.ComputerShootToSinkShip(shoot[0], shoot[1]);

            if (MyGameEngine.ComputerCheckHitOrMiss(newShot[0], newShot[1]))
            {
                CheckShootCloseToShipHit(newShot, shoot);
                CheckIfPlayerLost();
                ChangePlayerTurn();
            }
            else if (MyGameEngine.ComputerCheckCloseOrNot(newShot[0], newShot[1]))
                AddCloseOnPlayerBoard(newShot[0], newShot[1]);
            else
            {
                CheckIfComputerMiss(newShot);
                ChangePlayerTurn();
            }
        }

        private void CheckShootCloseToShipHit(int[] newShot, int[] shoot)
        {
            foreach (var myPlayerButton in PlayerButtonsInGame)
            {
                if (myPlayerButton.Longitude == newShot[0] && myPlayerButton.Latitude == newShot[1])
                {
                    ComputerHitPlayerShip(newShot, myPlayerButton);
                }
            }
        }
        
        public void ShootCloseToAShipAlreadyHit()
        {
            int[] shot = MyGameEngine.GetCoordinatesOfPlayerShipAlreadyHit();

            if (MyGameEngine.ComputerCheckIfShipStillFloating(shot[0], shot[1]) == true)
            {
                CheckComputerTurn(shot);
            }
            else if (MyGameEngine.ComputerCheckIfShipStillFloating(shot[0], shot[1]) == false)
            {
                ComputerHitShip = false;
                ComputerHitOrMiss();
            }
        }

        //------------------------------------------------------------
        public void ComputerHitOrMiss()
        {
            int[] shoot = MyGameEngine.ComputerRandomShotFired();

            if (WasCloseToShip == false && ComputerHitShip == false && MyGameEngine.CheckIfAPlayerShipHasBeenHit() == false)
            {
                CheckComputerTurn(shoot);
                CheckIfPlayerLost();
                ChangePlayerTurn();
                
                if (MyGameEngine.ComputerCheckCloseOrNot(shoot[0], shoot[1]))
                {
                    CoordinatesCloseToShip = new int[] { shoot[0], shoot[1] };
                    AddCloseOnPlayerBoard(shoot[0], shoot[1]);
                }
                else
                {
                    CheckIfComputerMiss(shoot);
                    ChangePlayerTurn();
                }
            } 
            else if (WasCloseToShip == true && ComputerHitShip == false && MyGameEngine.CheckIfAPlayerShipHasBeenHit() == false)
                ComputerShootAroundSplashSonar();
            else if (ComputerHitShip == true && MyGameEngine.CheckIfAPlayerShipHasBeenHit() == false)
                ComputerShootToSinkShip(CoordinatesHitShip);
            else if (MyGameEngine.CheckIfAPlayerShipHasBeenHit() == true)
                ShootCloseToAShipAlreadyHit();
        }
        private void UpdateNumberOfMovesOnGameboard()
        {
            ShowNumberOfMoves = MyGameEngine.NumberOfMoves;
        }

        private void AddHitOnComputerBoard(int buttonToNumber)
        {
            UpdateNumberOfMovesOnGameboard();
            ComputerButtonsInGame[buttonToNumber].HitOrMiss = "Träff";
            ChangeGridSquareToExplosionImage(ComputerButtonsInGame[buttonToNumber]);
            PlayerShotsFired.Add(buttonToNumber);
        }
        private void ChangePlayerTurn()
        {
            if (PlayerTurn == true)
                PlayerTurn = false;
            else
                PlayerTurn = true;
        }

        private bool HitComputerShip(int buttonToNumber)
        {
            if (MyGameEngine.PlayerCheckHitOrMiss(ComputerButtonsInGame[buttonToNumber].Longitude, ComputerButtonsInGame[buttonToNumber].Latitude))
                return true;
            else
                return false;
        }

        private bool HasBeenShotAtAlready(int buttonToNumber)
        {
            if (PlayerShotsFired.Contains(buttonToNumber))
                return true;
            else 
                return false;
        }
        public void ChangePlayerGridToSingleBoat(GameGrid grid)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                BitmapFrame image = BitmapFrame.Create(new Uri(@"pack://Application:,,,/Assets/Images/destroyerImg.png", UriKind.Absolute));
                grid.backgroundImage.ImageSource = image;
                grid.backgroundImage.Stretch = Stretch.Uniform;
            });
        }
        private void ChangePlayerGridToBattleShip(int longitude, int latitude, GameGrid grid)
        {
           
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (var c in PlayerButtonsInGame)
                {
                    if (c.Longitude == longitude + 1 && c.Latitude == latitude)
                    {
                        BitmapFrame image = BitmapFrame.Create(new Uri(@"Pack://application:,,,/Assets/Images/boatTwoSternVerticalImg.png", UriKind.Absolute));
                        c.backgroundImage.ImageSource = image;
                        c.backgroundImage.Stretch = Stretch.Uniform;

                    }
                    if (c.Longitude == longitude && c.Latitude == latitude)
                    {
                        BitmapFrame image1 = BitmapFrame.Create(new Uri(@"Pack://application:,,,/Assets/Images/boatTwoBowVerticalImg.png", UriKind.Absolute));
                        c.backgroundImage.ImageSource = image1;
                        c.backgroundImage.Stretch = Stretch.Uniform;
                    }
                }
                
            });
        }
        private void ChangePlayerGridToSubmarine(int longitude, int latitude, GameGrid grid)
        {

            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (var c in PlayerButtonsInGame)
                {
                    if (c.Longitude == longitude - 1 && c.Latitude == latitude)
                    {
                        BitmapFrame image = BitmapFrame.Create(new Uri(@"Pack://application:,,,/Assets/Images/boatTreeBowVerticalImg.png", UriKind.Absolute));
                        c.backgroundImage.ImageSource = image;
                        c.backgroundImage.Stretch = Stretch.Uniform;

                    }
                    if (c.Longitude == longitude + 1  && c.Latitude == latitude)
                    {
                        BitmapFrame image1 = BitmapFrame.Create(new Uri(@"Pack://application:,,,/Assets/Images/boatTreeSternVerticalImg.png", UriKind.Absolute));
                        c.backgroundImage.ImageSource = image1;
                        c.backgroundImage.Stretch = Stretch.Uniform;
                    }
                    if (c.Longitude == longitude && c.Latitude == latitude)
                    {
                        BitmapFrame image1 = BitmapFrame.Create(new Uri(@"Pack://application:,,,/Assets/Images/boatTreeMiddleVerticalImg.png", UriKind.Absolute));
                        c.backgroundImage.ImageSource = image1;
                        c.backgroundImage.Stretch = Stretch.Uniform;
                    }
                }

            });
        }

        private void ChangeGridSquareToExplosionImage(GameGrid grid)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                BitmapFrame image = BitmapFrame.Create(new Uri(@"Pack://application:,,,/Assets/Images/explosion.png", UriKind.Absolute));
                grid.backgroundImage.ImageSource = image;
                grid.backgroundImage.Stretch = Stretch.Uniform;
            });
        }

        private void ChangeGridSquareToSplashSonarImage(GameGrid grid)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                BitmapFrame image = BitmapFrame.Create(new Uri(@"Pack://application:,,,/Assets/Images/splashSonar.png", UriKind.Absolute));
                grid.backgroundImage.ImageSource = image;
                grid.backgroundImage.Stretch = Stretch.Uniform;
            });
        }

        private void ChangeToSplashImage(GameGrid grid)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                BitmapFrame image = BitmapFrame.Create(new Uri(@"Pack://application:,,,/Assets/Images/splash.png", UriKind.Absolute));
                grid.backgroundImage.ImageSource = image;
                grid.backgroundImage.Stretch = Stretch.Uniform;
            });
        }

        private void ShowLosingDialogueBox()
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MessageBoxResult result = MessageBox.Show($"Ops {Global.MyPlayer.Nickname}, du förlorade... mot en dator... vill du försöka igen?", "Avsluta", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            {
                                Global.MyWin.frame.Content = new GameWindowPage();
                            };
                            break;
                        case MessageBoxResult.No:
                            {
                                GoToMainPage();
                            };
                            break;
                    }
                }), null);
            });
        }

        private void AskIfExitCurrentRound()
        {
            MessageBoxResult result = MessageBox.Show("Vill du verkligen avsluta pågående spel?", "Avsluta", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    GoToMainPage();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        private void GoToMainPage()
        {
            Global.MyWin.frame.Content = new MainMenuPage();
        }
    }
}
