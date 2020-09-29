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
        public int ShowNumberOfMoves { get; set; }
        public string ShowPlayerNickname { get; set; }
        public int Ships { get; set; } = 3;
        public ObservableCollection<GameGrid> PlayerButtonsInGame { get; set; }  = new ObservableCollection<GameGrid>();
        public ObservableCollection<GameGrid> ComputerButtonsInGame { get; set; } = new ObservableCollection<GameGrid>();
        public List<int> PlayerShotsFired { get; set; } = new List<int>();

        private int noMoreShipsToUse = 0;
        public SingleBoatUC SingleBoat { get; set; }
        public GameEngine MyGameEngine { get; set; } = new GameEngine();
        public bool PlayerTurn { get; set; } = false;
        public bool WasCloseToShip { get; set; } = false;
        public int[] CoordinatesCloseToShip { get; set; }
        #endregion
        public GameWindowViewModel(SingleBoatUC boat)
        {
            SingleBoat = boat;
            ComputerButtonsInGame = MyGameEngine.ComputerButtonsInGame;
            PlayerButtonsInGame = MyGameEngine.PlayerButtonsInGame;
            PlaceShip = new RelayPropertyCommand(PlayerPlaceShips);
            CheckIfShip = new RelayPropertyCommand(PlayerCheckHitOrMiss);
            GoToMainPageCommand = new RelayCommand(AskIfExitCurrentRound);
            PlaceRandomBoats = new RelayCommand(RandomPlacePlayerShips);
            ShowPlayerNickname = MyPlayer.Nickname;
            ShowNumberOfMoves = MyGameEngine.NumberOfMoves;
        }
      
        public void PlayerPlaceShips(string button)
        {
            int buttonToNumber = int.Parse(button);
            if (PlayerHasShipsLeftToPlace(buttonToNumber))
            {
                SingleBoat.PlacedBoats--;
                Ships--;
                foreach (var x in PlayerButtonsInGame)
                {
                    if (PlayerButtonsInGame[buttonToNumber].Longitude == x.Longitude && PlayerButtonsInGame[buttonToNumber].Latitude == x.Latitude)
                    {
                        ChangePlayerGridToSingleBoat(x);
                    }
                }

                if (Ships == noMoreShipsToUse)
                {
                    ChangePlayerTurn();
                    MessageBox.Show("Nu kan spelet börja, du spelar på den högra spelplanen.");
                }
            }
            else
                MessageBox.Show("Det går inte att placera skepp där.");
        }
        public void PlayerPlaceSubmarineShip(string button)
        {

            int buttonToNumber = int.Parse(button);
            if (MyGameEngine.FillPlayerSubmarineShip(PlayerButtonsInGame[buttonToNumber].Longitude, PlayerButtonsInGame[buttonToNumber].Latitude) == true)
            {
                Ships--;

                foreach (var x in PlayerButtonsInGame)
                {
                    if (PlayerButtonsInGame[buttonToNumber].Longitude == x.Longitude && PlayerButtonsInGame[buttonToNumber].Latitude == x.Latitude)
                    {
                        ChangePlayerGridToSubmarine(x.Longitude, x.Latitude, x);
                    }
                }
                if (Ships == 0)
                {
                    PlayerTurn = true;
                    MessageBox.Show("Nu kan spelet börja, du spelar på den högra skärmen");
                }
            }
            else
            {
                MessageBox.Show("Du har redan placerat ett skepp där");
            }
        }
        public void PlayerPlaceBattleShip(string button)
        {

            int buttonToNumber = int.Parse(button);
            if (MyGameEngine.FillPlayerBattleShip(PlayerButtonsInGame[buttonToNumber].Longitude, PlayerButtonsInGame[buttonToNumber].Latitude) == true)
            {
                Ships--;

                    foreach (var x in PlayerButtonsInGame)
                    {
                        if (PlayerButtonsInGame[buttonToNumber].Longitude == x.Longitude && PlayerButtonsInGame[buttonToNumber].Latitude == x.Latitude)
                        {
                            ChangePlayerGridToBattleShip(x.Longitude, x.Latitude, x);
                        }
                    }
               
                if (Ships == 0)
                {
                    PlayerTurn = true;
                    MessageBox.Show("Nu kan spelet börja, du spelar på den högra skärmen");
                }
            }
            else
            {
                MessageBox.Show("Du har redan placerat ett skepp där");
            }
        }

        private bool PlayerHasShipsLeftToPlace(int buttonToNumber)
        {
            bool result = false;
            if (MyGameEngine.FillPlayerShips(PlayerButtonsInGame[buttonToNumber].Longitude, PlayerButtonsInGame[buttonToNumber].Latitude))
                result = true;
            return result;
        }
        private void RandomPlacePlayerShips()
        {
            if (Ships != noMoreShipsToUse)
            {
                int[] PlacedShips = MyGameEngine.RandomFillPlayerShips();

                foreach (var button in PlayerButtonsInGame)
                {
                    for (int i = 0; i < PlacedShips.Length - 1; i++)
                    {
                        if (button.Longitude == PlacedShips[i] && button.Latitude == PlacedShips[i + 1])
                        {
                            ChangePlayerGridToSingleBoat(button);
                        }
                        i += 1;
                    }
                }
                Ships = 0;
                ChangePlayerTurn();
                MessageBox.Show("Nu kan spelet börja, du spelar på den högra spelplanen.");
            }
            else
            {
                MessageBox.Show("Du har redan slumpat fram dina skepp");
            }
        }
        private void PlayerCheckHitOrMiss(string button)
        {

            if (PlayerTurn == true)
            {
                int buttonToNumber = int.Parse(button);
                if (HasBeenShotAtAlready(buttonToNumber))
                    MessageBox.Show("Du har redan skjutit där!");
                else if (HitComputerShip(buttonToNumber))
                {
                    AddHitOnComputerBoard(buttonToNumber);
                    ChangePlayerTurn();

                    if (MyGameEngine.HasWon())
                    {
                        Highscore myHighscore = MyGameEngine.AddNewHighscore(true, MyPlayer.Id);
                        ShowWinDialogueBox(myHighscore);
                    }
                    Task.Delay(500).ContinueWith(t => ComputerHitOrMiss());
                }
                else
                {
                    AddCloseOrMissOnComputerBoard(buttonToNumber);
                    PlayerTurn = false;
                    Task.Delay(500).ContinueWith(t => ComputerHitOrMiss());
                }
            }
        }

        private void ShowWinDialogueBox(Highscore myHighscore)
        {
            MessageBoxResult result = MessageBox.Show($"Grattis {MyPlayer.Nickname}, du vann{DidPlayerMakeTheHighscoreString(myHighscore)}", "Avsluta", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    MyWin.frame.Content = new GameWindowPage(MyPlayer);
                    break;
                case MessageBoxResult.No:
                    MyWin.frame.Content = new MainMenuPage();
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
            if(MyGameEngine.ComputerCheckCloseOrNot(longitude, latitude) == true)
            {
                foreach (var c in PlayerButtonsInGame)
                {
                    if(c.Longitude == longitude && c.Latitude == latitude)
                    {
                        ChangeGridSquareToSplashSonarImage(c);
                        
                        WasCloseToShip = true;
                        PlayerTurn = true;
                        c.IsClicked = true;
                    }
                }
            }
        }
        private void ComputerShootAroundSplashSonar()
        {
            int[] shoot = MyGameEngine.ComputerShotCloseToSplashSonar(CoordinatesCloseToShip[0], CoordinatesCloseToShip[1]);

            if (MyGameEngine.ComputerCheckHitOrMiss(shoot[0], shoot[1]))
            {
                foreach (var c in PlayerButtonsInGame)
                {
                    if (c.Longitude == shoot[0] && c.Latitude == shoot[1])
                    {
                        c.HitOrMiss = "Träff!";
                        ChangeGridSquareToExplosionImage(c);
                        c.IsClicked = true;
                        WasCloseToShip = false;
                    }
                }
                if (MyGameEngine.HasLost())
                {
                    MyGameEngine.AddNewHighscore(false, MyPlayer.Id);
                    ShowLosingDialogueBox();
                }
                PlayerTurn = true;
            } else if (MyGameEngine.ComputerCheckCloseOrNot(shoot[0], shoot[1]))
            {
                AddCloseOnPlayerBoard(shoot[0], shoot[1]);
            }
            else
            {
                foreach (var c in PlayerButtonsInGame)
                {
                    if (c.Longitude == shoot[0] && c.Latitude == shoot[1])
                    {
                        c.HitOrMiss = "Miss!";
                        ChangeToSplashImage(c);
                        c.IsClicked = true;
                    }
                }
                PlayerTurn = true;
            }
        }
        private void ComputerHitOrMiss()
        {
            
            int[] shoot = MyGameEngine.ComputerRandomShotFired();

            if (WasCloseToShip == false)
            {
                if (MyGameEngine.ComputerCheckHitOrMiss(shoot[0], shoot[1]))
                {
                    foreach (var c in PlayerButtonsInGame)
                    {
                        if (c.Longitude == shoot[0] && c.Latitude == shoot[1])
                        {
                            c.HitOrMiss = "Träff!";
                            ChangeGridSquareToExplosionImage(c);
                            c.IsClicked = true;
                        }
                    }
                    if (MyGameEngine.HasLost())
                    {
                        MyGameEngine.AddNewHighscore(false, MyPlayer.Id);
                        ShowLosingDialogueBox();
                    }
                    PlayerTurn = true;
                }
                else if (MyGameEngine.ComputerCheckCloseOrNot(shoot[0], shoot[1]))
                {
                    CoordinatesCloseToShip = new int[] { shoot[0], shoot[1] };
                    AddCloseOnPlayerBoard(shoot[0], shoot[1]);
                }
                else
                {
                    foreach (var c in PlayerButtonsInGame)
                    {
                        if (c.Longitude == shoot[0] && c.Latitude == shoot[1])
                        {
                            c.HitOrMiss = "Miss!";
                            ChangeToSplashImage(c);
                            c.IsClicked = true;
                        }
                    }
                    PlayerTurn = true;
                }
            } else if (WasCloseToShip == true)
            {
                ComputerShootAroundSplashSonar();
            }
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
            else { 
                return false;
            }
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
                    MessageBoxResult result = MessageBox.Show($"Ops {MyPlayer.Nickname}, du förlorade... mot en dator... vill du försöka igen?", "Avsluta", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            {
                                MyWin.frame.Content = new GameWindowPage(MyPlayer);
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
            MyWin.frame.Content = new MainMenuPage();
        }
    }
}
