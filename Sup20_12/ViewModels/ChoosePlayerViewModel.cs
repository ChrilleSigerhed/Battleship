using Sup20_12.View;
using Sup20_12.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Sup20_12.ViewModels
{
    public class ChoosePlayerViewModel : BaseViewModel
    {
        #region Properties
        public Player Player { get; set; }
        public string PlayerName { get; set; }
        public ICommand AddNewPlayerCommand { get; set; }
        public ICommand StartGameWithSelectedPlayerCommand { get; set; }
        public ICommand GoToMainPageCommand { get; set; }
        public List<Player> ListOfPlayersInListBox { get; set; }

        public MainWindow win = (MainWindow)Application.Current.MainWindow;
        #endregion

        public ChoosePlayerViewModel()
        {
            StartGameWithSelectedPlayerCommand = new RelayCommand(SelectedPlayerForGame);
            AddNewPlayerCommand = new RelayCommand(AddPlayer);
            GoToMainPageCommand = new RelayCommand(GoToMainPage);
            GetPlayersFromDb();
        }
        public void AddPlayer()
        {
            if(this.PlayerName != null)
            {
                Player = new Player(this.PlayerName);
                ClearTextBox();
                DbConnection.AddNewPlayerToDb(Player);
                UpdatePlayerList();
            }
        }
        public void UpdatePlayerList()
        {
            GetPlayersFromDb();
        }
        public void ClearTextBox()
        {
            this.PlayerName = "";
        }
        public void GetPlayersFromDb()
        {
           ListOfPlayersInListBox = (List<Player>)DbConnection.GetPlayers();
        }
        public void SelectedPlayerForGame()
        {
            
            this.Player = Player;
            win.frame.Content = new GameWindowPage(Player);
        }
        public void GoToMainPage()
        {
            win.frame.Content = new MainMenuPage();
        }
    }
}
