using Sup20_12.View;
using Sup20_12.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        public List<int> PlayerShootsFired { get; set; } = new List<int>();

        public MainWindow win = (MainWindow)Application.Current.MainWindow;
        public GameEngine gameEngine { get; set; } = new GameEngine();
        public Player Player { get; set; }
        public bool PlayerTurn { get; set; } = false;
        #endregion 
        public GameWindowViewModel(Player player)
        {
            Player = player;
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
            if (gameEngine.FillPlayerShips(PlayerButtonsInGame[buttonToNumber].Latitude, PlayerButtonsInGame[buttonToNumber].Longitude) == true)
            {
                Ships--;
                PlayerButtonsInGame[buttonToNumber].HitOrMiss = "Skepp";
                if(Ships == 0)
                {
                    PlayerTurn = true;
                    MessageBox.Show("Nu kan spelet börja, du spelar på den högra skärmen");
                }
            }
            else
                MessageBox.Show("Du har redan placerat ett skepp där");
        }
        
        public void PlayerCheckHitOrMiss(string button)
        {
            
            if (PlayerTurn == true)
            {
                ShowNumberOfMoves = gameEngine.NumberOfMoves;
                int buttonToNumber = int.Parse(button);
                if (PlayerShootsFired.Contains(buttonToNumber))
                {
                    MessageBox.Show("Du har redan skjutit där!");
                }
                else if(gameEngine.PlayerCheckHitOrMiss(ComputerButtonsInGame[buttonToNumber].Longitude, ComputerButtonsInGame[buttonToNumber].Latitude) == true)
                {
                    ComputerButtonsInGame[buttonToNumber].HitOrMiss = "Träff";
                    PlayerShootsFired.Add(buttonToNumber);
                    PlayerTurn = false;
                    
                    Task.Delay(500).ContinueWith(t => ComputerHitOrMiss());
                    if (gameEngine.HasWon() == true)
                    {
                        gameEngine.AddNewHighscoreWin(Player.Id);
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
                }
                else
                {
                    ComputerButtonsInGame[buttonToNumber].HitOrMiss = "Miss";
                    PlayerTurn = false;
                    Task.Delay(500).ContinueWith(t => ComputerHitOrMiss());
                }
            }
        }
        public void ComputerHitOrMiss()
        {
            int[] shoot = gameEngine.ComputerRandomShotFired();

            if(gameEngine.ComputerCheckHitOrMiss(shoot[0], shoot[1]) == true)
            {
                foreach (var c in PlayerButtonsInGame)
                {
                    if(c.Longitude == shoot[0] && c.Latitude == shoot[1])
                    {
                        c.HitOrMiss = "Träff!";
                        c.IsClicked = true;
                    }
                }
                PlayerTurn = true;
                if(gameEngine.HasLost() == true)
                {
                    gameEngine.AddNewHighscoreLost(Player.Id);
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
