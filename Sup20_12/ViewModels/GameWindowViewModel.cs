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
        public int Ships { get; set; } = 3;
        public ObservableCollection<GameGrid> PlayerButtonsInGame { get; set; } = new ObservableCollection<GameGrid>();
        public ObservableCollection<GameGrid> ComputerButtonsInGame { get; set; } = new ObservableCollection<GameGrid>();
        public Dictionary<string, bool> PlayerShips { get; set; } = new Dictionary<string, bool>();
        public Dictionary<string, bool> ComputerShips { get; set; } = new Dictionary<string, bool>();

        public MainWindow win = (MainWindow)Application.Current.MainWindow;
        public GameEngine gameEngine { get; set; } = new GameEngine();
        public Player Player { get; set; }
        #endregion
        public GameWindowViewModel(Player player)
        {
            Player = player;
            ComputerButtonsInGame = gameEngine.ComputerButtonsInGame;
            PlayerButtonsInGame = gameEngine.PlayerButtonsInGame;
            ComputerShips = gameEngine.ComputerShips;
            PlaceShip = new RelayPropertyCommand(PlayerPlaceShips);
            CheckIfShip = new RelayPropertyCommand(PlayerCheckHitOrMiss);
        }

        public void PlayerPlaceShips(string button)
        {
            if (gameEngine.FillPlayerShips(button) == true)
            {
                Ships--;
                int buttonToNumber = int.Parse(button);


                PlayerButtonsInGame[buttonToNumber].HitOrMiss = "Skepp";

                if (Ships == 0)
                {
                    MessageBox.Show("Nu kan spelet börja, du spelar på den högra skärmen");
                }
            }
            else
            {
                MessageBox.Show("Du har redan placerat ett skepp där");
            }
        }

        public void PlayerCheckHitOrMiss(string button)
        {
            int buttonToNumber = int.Parse(button);
            if(ComputerButtonsInGame[buttonToNumber].HitOrMiss != "")
            {
                MessageBox.Show("Du har redan skjutit där!");
            }

            else if(gameEngine.PlayerCheckHitOrMiss(button, ComputerShips) == true)
            {
                ComputerButtonsInGame[buttonToNumber].HitOrMiss = "Träff";
                if(gameEngine.HasWon() == true)
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
                Task.Delay(500).ContinueWith(t => ComputerHitOrMiss());
            }
        }
        public void ComputerHitOrMiss()
        {
            int shoot = gameEngine.ComputerRandomShotFired(PlayerButtonsInGame);
            if(gameEngine.ComputerCheckHitOrMiss(shoot.ToString(), PlayerShips) == true)
            {
                PlayerButtonsInGame[shoot].HitOrMiss = "Träff";
                if(gameEngine.HasLost() == true)
                {
                    
                    gameEngine.AddNewHighscoreLost(Player.Id);
                    MessageBox.Show("Du förlorade");
                }
            }
            else
            {
                PlayerButtonsInGame[shoot].HitOrMiss = "Miss";
            }
        }
      
    }
}
