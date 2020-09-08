using Sup20_12.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Sup20_12.ViewModels
{
    public class ChoosePlayerViewModel : BaseViewModel
    {
        public string Nickname { get; set; } = "Nickname";
        public Player Player{ get; set; }
        public string PlayerName { get; set; }
        public ICommand AddNewPlayerCommand { get; set; }
        public ICommand StartGameWithSelectedPlayerCommand { get; set; }
        public List<Player> Players { get; set; }
        private GameEngineViewModel NewGame { get; set; }

        public ChoosePlayerViewModel()
        {
            StartGameWithSelectedPlayerCommand = new RelayCommand(SelectedPlayerForGame);
            AddNewPlayerCommand = new RelayCommand(AddPlayer);
            GetPlayersFromDb();
        }
        public void AddPlayer()
        {
            Player = new Player(this.PlayerName);
            ClearTextBox();
            DbConnection.AddNewPlayerToDb(Player);
            UpdatePlayerList();
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
           Players = (List<Player>)DbConnection.GetPlayers();
        }
        public void SelectedPlayerForGame()
        {
            NewGame = new GameEngineViewModel();
            NewGame.StartTheGameWithSelectedPlayer(this.Player);
        }
    }
}
