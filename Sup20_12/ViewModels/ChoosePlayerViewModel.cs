using Sup20_12.View;
using Sup20_12.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Sup20_12.ViewModels
{
    public class ChoosePlayerViewModel : BaseViewModel
    {
        #region Properties
        public Player MyPlayer { get; set; }
        public string PlayerNickname { get; set; }
        public ICommand AddNewPlayerCommand { get; set; }
        public ICommand StartGameWithSelectedPlayerCommand { get; set; }
        public ICommand GoToMainPageCommand { get; set; }
        public ObservableCollection<Player> ListOfPlayersInListBox { get; set; }

        public MainWindow win = (MainWindow)Application.Current.MainWindow;
        #endregion

        public ChoosePlayerViewModel()
        {
            StartGameWithSelectedPlayerCommand = new RelayCommand(SelectedPlayerForGame);
            AddNewPlayerCommand = new RelayCommand(AddPlayer);
            GoToMainPageCommand = new RelayCommand(GoToMainPage);
            GetPlayersFromDb(MyPlayer);
        }


        public void AddPlayer()
        {
            if (this.PlayerNickname == null)
                MessageBox.Show("Du har inte valt något nickname. Välj från listan eller skriv in ett nytt i rutan.");
            else if (!DbConnection.IsPlayerNicknameUniqueInDb(this.PlayerNickname))
                MessageBox.Show("Nickname finns redan. Om det är ditt kan du välja det från listan nedan, annars skriv in ett unikt nickname i rutan och klicka - Lägg till spelare");
            else
            {
                Player myTempPlayer = new Player(PlayerNickname);
                ClearTextBox();
                myTempPlayer = DbConnection.AddNewPlayerToDb(myTempPlayer);
                GetPlayersFromDb(myTempPlayer);
            }
        }

        public void ClearTextBox()
        {
            this.PlayerNickname = "";
        }
        public void GetPlayersFromDb(Player myTempPlayer)
        {
           ListOfPlayersInListBox = DbConnection.Players;
           
        }
        public void SelectedPlayerForGame()
        {
            if (IsThereAnActivePlayer())
            {
                win.frame.Content = new GameWindowPage(MyPlayer);
                InitialGameInstructions();
            }
        }
        public void InitialGameInstructions()
        {
            MessageBox.Show("Börja spelet genom att dra 3 stycken skepp från hamnen till 3 olika rutor på den vänstra spelplanen!");
        }
        public void GoToMainPage()
        {
            win.frame.Content = new MainMenuPage();
        }

        private bool IsThereAnActivePlayer()
        {
            if (MyPlayer == null)
            {
                MessageBox.Show("Du har inte valt någon spelare. Välj en i listan eller skriv in ett nytt nickname.");
                return false;
            }
            else
                return true;
        }
    }
}
