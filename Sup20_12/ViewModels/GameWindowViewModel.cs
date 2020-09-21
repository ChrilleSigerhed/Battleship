using Sup20_12.View;
using Sup20_12.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sup20_12.ViewModels
{
   public class GameWindowViewModel : BaseViewModel
    {
        #region Properties
        public ICommand PlaceShip { get; set; }
        public ICommand CheckIfShip { get; set; }
        public ICommand GoToMainPageCommand { get; set; }
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
        public Player Player { get; set; }
        public bool PlayerTurn { get; set; } = false;
        #endregion
        public GameWindowViewModel(Player player, SingleBoatUC boat)
        {
            Player = player;
            SingleBoat = boat;
            ComputerButtonsInGame = gameEngine.ComputerButtonsInGame;
            PlayerButtonsInGame = gameEngine.PlayerButtonsInGame;
            PlaceShip = new RelayPropertyCommand(PlayerPlaceShips);
            CheckIfShip = new RelayPropertyCommand(PlayerCheckHitOrMiss);
            GoToMainPageCommand = new RelayCommand(AskIfExitCurrentRound);
            ShowPlayerNickname = player.Nickname;
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
                    MessageBox.Show("Nu kan spelet börja, du spelar på den högra skärmen.");
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

        public void PlayerCheckHitOrMiss(string button)
        {
            
            if (PlayerTurn == true)
            {
                
                int buttonToNumber = int.Parse(button);
                if (HasBeenShotAtAlready(buttonToNumber))
                    MessageBox.Show("Du har redan skjutit där!");
                else if(HitComputerShip(buttonToNumber))
                {
                    AddHitOnComputerBoard(buttonToNumber);
                    ChangePlayerTurn();
                    
                    Task.Delay(500).ContinueWith(t => ComputerHitOrMiss());
                    if (gameEngine.HasWon())
                    {
                        gameEngine.AddNewHighscore(true, Player.Id);
                        ShowWinDialogueBox();
                    }
                }
                else
                {
                    AddCloseOrMissOnComputerBoard(buttonToNumber);
                    PlayerTurn = false;
                    Task.Delay(500).ContinueWith(t => ComputerHitOrMiss());
                }
            }
        }

        private void ShowWinDialogueBox()
        {
            MessageBoxResult result = MessageBox.Show($"Grattis {Player.Nickname} du vann, vill du spela igen?", "Avsluta", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    win.frame.Content = new GameWindowPage(Player);
                    break;
                case MessageBoxResult.No:
                    win.frame.Content = new MainMenuPage();
                    break;
            }
        }

        private void AddCloseOrMissOnComputerBoard(int buttonToNumber)
        {
            UpdateNumberOfMovesOnGameboard();
            PlayerShotsFired.Add(buttonToNumber);
            if (gameEngine.PlayerCheckCloseOrNot(ComputerButtonsInGame[buttonToNumber].Longitude, ComputerButtonsInGame[buttonToNumber].Latitude) == true)
                ComputerButtonsInGame[buttonToNumber].HitOrMiss = "Nära";
            else
                ComputerButtonsInGame[buttonToNumber].HitOrMiss = "Miss";
        }

        private void UpdateNumberOfMovesOnGameboard()
        {
            ShowNumberOfMoves = gameEngine.NumberOfMoves;
        }

        private void AddHitOnComputerBoard(int buttonToNumber)
        {
            UpdateNumberOfMovesOnGameboard();
            ComputerButtonsInGame[buttonToNumber].HitOrMiss = "Träff";
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
                        c.IsClicked = true;
                    }
                }
                if(gameEngine.HasLost())
                {
                    gameEngine.AddNewHighscore(false, Player.Id);
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
                        c.IsClicked = true;
                    }
                }
                PlayerTurn = true;
            }
        }

        private void ShowLosingDialogueBox()
        {
            MessageBoxResult result = MessageBox.Show($"Ops {Player.Nickname}, du förlorade... mot en dator... vill du försöka igen?", "Avsluta", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        win.frame.Content = new GameWindowPage(Player);
                    });
                    break;
                case MessageBoxResult.No:
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        win.frame.Content = new MainMenuPage();
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
