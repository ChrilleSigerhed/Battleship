﻿using Sup20_12.View;
using Sup20_12.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        public MainWindow win = (MainWindow)Application.Current.MainWindow;
        public SingleBoatUC SingleBoat { get; set; }
        public GameEngine gameEngine { get; set; } = new GameEngine();
        public Player MyPlayer { get; set; }
        public bool PlayerTurn { get; set; } = false;
        #endregion
        public GameWindowViewModel(Player myPlayer, SingleBoatUC boat)
        {
            MyPlayer = myPlayer;
            SingleBoat = boat;
            ComputerButtonsInGame = gameEngine.ComputerButtonsInGame;
            PlayerButtonsInGame = gameEngine.PlayerButtonsInGame;
            PlaceShip = new RelayPropertyCommand(PlayerPlaceShips);
            CheckIfShip = new RelayPropertyCommand(PlayerCheckHitOrMiss);
            GoToMainPageCommand = new RelayCommand(AskIfExitCurrentRound);
            PlaceRandomBoats = new RelayCommand(RandomPlacePlayerShips);

            ShowPlayerNickname = myPlayer.Nickname;
            ShowNumberOfMoves = gameEngine.NumberOfMoves;
        }
      
        public void PlayerPlaceShips(string button)
        {
            int buttonToNumber = int.Parse(button);
            if (PlayerHasShipsLeftToPlace(buttonToNumber))
            {
                SingleBoat.PlacedBoats--;
                Ships--;
                if (Ships == noMoreShipsToUse)
                {
                    ChangePlayerTurn();
                    MessageBox.Show("Nu kan spelet börja, du spelar på den högra spelplanen.");
                }
            }
            else
                MessageBox.Show("Det går inte att placera skepp där.");
        }

        private bool PlayerHasShipsLeftToPlace(int buttonToNumber)
        {
            bool result = false;
            if (gameEngine.FillPlayerShips(PlayerButtonsInGame[buttonToNumber].Longitude, PlayerButtonsInGame[buttonToNumber].Latitude))
                result = true;
            return result;
        }
        public void RandomPlacePlayerShips()
        {
            if(Ships == noMoreShipsToUse) 
            {
                MessageBox.Show("Du har redan slumpat fram dina skepp");
            }
            else
            {
                int[] PlacedShips = gameEngine.RandomFillPlayerShips();
                foreach (var button in PlayerButtonsInGame)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (button.Longitude == PlacedShips[i] && button.Latitude == PlacedShips[i + 1])
                        {
                            ChangePlayerGridToSingleBoat(button);

                        }
                        i += 1;
                    }
                }
                SingleBoat.PlacedBoats = 0;
                Ships = 0;
                ChangePlayerTurn();
                MessageBox.Show("Nu kan spelet börja, du spelar på den högra spelplanen.");
            }
        }

        public void PlayerCheckHitOrMiss(string button)
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

                    if (gameEngine.HasWon())
                    {
                        Highscore myHighscore = gameEngine.AddNewHighscore(true, MyPlayer.Id);
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
                    win.frame.Content = new GameWindowPage(MyPlayer);
                    break;
                case MessageBoxResult.No:
                    win.frame.Content = new MainMenuPage();
                    break;
            }
        }
        private string DidPlayerMakeTheHighscoreString(Highscore myHighscore)
        {
            string returnString = "";
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
            if (gameEngine.PlayerCheckCloseOrNot(ComputerButtonsInGame[buttonToNumber].Longitude, ComputerButtonsInGame[buttonToNumber].Latitude) == true)
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
        private void UpdateNumberOfMovesOnGameboard()
        {
            ShowNumberOfMoves = gameEngine.NumberOfMoves;
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
            if (gameEngine.PlayerCheckHitOrMiss(ComputerButtonsInGame[buttonToNumber].Longitude, ComputerButtonsInGame[buttonToNumber].Latitude))
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

        public void ComputerHitOrMiss()
        {
            
            int[] shoot = gameEngine.ComputerRandomShotFired();

            if(gameEngine.ComputerCheckHitOrMiss(shoot[0], shoot[1]))
            {
                foreach (var c in PlayerButtonsInGame)
                {
                    if(c.Longitude == shoot[0] && c.Latitude == shoot[1])
                    {
                        c.HitOrMiss = "Träff!";
                        ChangeGridSquareToExplosionImage(c);                        
                        c.IsClicked = true;
                    }
                }
                if(gameEngine.HasLost())
                {
                    gameEngine.AddNewHighscore(false, MyPlayer.Id);
                    ShowLosingDialogueBox();
                }
                PlayerTurn = true;
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
        public void ChangePlayerGridToSingleBoat(GameGrid grid)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                BitmapFrame image = BitmapFrame.Create(new Uri(@"pack://Application:,,,/Assets/Images/destroyerImg.png", UriKind.Absolute));
                grid.backgroundImage.ImageSource = image;
                grid.backgroundImage.Stretch = Stretch.Uniform;
            });
        }
        public void ChangeGridSquareToExplosionImage(GameGrid grid)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                BitmapFrame image = BitmapFrame.Create(new Uri(@"Pack://application:,,,/Assets/Images/explosion.png", UriKind.Absolute));
                grid.backgroundImage.ImageSource = image;
                grid.backgroundImage.Stretch = Stretch.Uniform;
            });
        }

        public void ChangeGridSquareToSplashSonarImage(GameGrid grid)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                BitmapFrame image = BitmapFrame.Create(new Uri(@"Pack://application:,,,/Assets/Images/splashSonar.png", UriKind.Absolute));
                grid.backgroundImage.ImageSource = image;
                grid.backgroundImage.Stretch = Stretch.Uniform;
            });
        }

        public void ChangeToSplashImage(GameGrid grid)
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
                MessageBoxResult result = MessageBox.Show($"Ops {MyPlayer.Nickname}, du förlorade... mot en dator... vill du försöka igen?", "Avsluta", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            win.frame.Content = new GameWindowPage(MyPlayer);
                        });
                        break;
                    case MessageBoxResult.No:
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            GoToMainPage();
                        });
                        break;
                }
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
            win.frame.Content = new MainMenuPage();
        }
    }
}
