using Sup20_12.View;
using Sup20_12.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Sup20_12.ViewModels
{
    public class ChoosePlayerViewModel : BaseViewModel
    {
        #region Properties
        public int SelectedId { get; set; }
        public string PlayerNickname { get; set; }
        public ICommand AddNewPlayerCommand { get; set; }
        public ICommand StartGameWithSelectedPlayerCommand { get; set; }
        public ICommand GoToMainPageCommand { get; set; }
        public ObservableCollection<Player> ListOfPlayersInListBox { get; set; }
        #endregion

        public ChoosePlayerViewModel()
        {
            StartGameWithSelectedPlayerCommand = new RelayCommand(SelectedPlayerForGame);
            AddNewPlayerCommand = new RelayCommand(AddPlayer);
            GoToMainPageCommand = new RelayCommand(GoToMainPage);
            GetPlayersFromDb();
            HighlightLastPlayerFromPreviousSessionInList();
        }

        public void AddPlayer()
        {
            if (PlayerNickname == null || NicknameContainBlankSpaces(PlayerNickname))
                MessageBox.Show("Du har inte skrivit något nickname eller valt ett felaktigt namn (blanksteg är inte tillåtna). Välj från listan eller skriv in ett i rutan.");
            else if (!DbConnection.IsPlayerNicknameUniqueInDb(PlayerNickname))
            {
                MessageBox.Show("Detta nickname finns redan. Om det är ditt kan du klicka på -Starta Spelet- direkt för att spela med detta nickname eller skriv in ett unikt nickname i rutan och klicka - Lägg till spelare");
                HighlightSelectedPlayer(PlayerNickname);
            }
            else
            {
                Player myTempPlayer = new Player(PlayerNickname);
                ClearTextBox();
                myTempPlayer = DbConnection.AddNewPlayerToDb(myTempPlayer);
                GetPlayersFromDb();
                HighlightSelectedPlayer(myTempPlayer.Nickname);
            }
        }

        private bool NicknameContainBlankSpaces(string PlayerNickname)
        {
            if (PlayerNickname.Contains(" "))
                return true;
            else
                return false;
        }
        private void ClearTextBox()
        {
            PlayerNickname = "";
        }

        public void GetPlayersFromDb()
        {
           ListOfPlayersInListBox = DbConnection.Players;
        }
        private void SelectedPlayerForGame()
        {
            if (IsThereAnActivePlayer())
            {
                MyWin.frame.Content = new GameWindowPage(MyPlayer);
                InitialGameInstructions();
            }
        }
        private void InitialGameInstructions()
        {
            MessageBox.Show("Börja spelet genom att dra 3 stycken skepp från hamnen till 3 olika rutor på den vänstra spelplanen.");
        }

        private void GoToMainPage()
        {
            MyWin.frame.Content = new MainMenuPage();
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

        private void SetActivePlayer(Player myPlayer)
        {
            MyPlayer = myPlayer;
        }

        private void HighlightSelectedPlayer(string PlayerNickname)
        {
            for (int i = 0; i < ListOfPlayersInListBox.Count; i++)
            {
                if (ListOfPlayersInListBox[i].Nickname.ToUpper() == PlayerNickname.ToUpper())
                {
                    SelectedId = ListOfPlayersInListBox[i].Id;
                    SetActivePlayer(ListOfPlayersInListBox[i]);
                }
            }
        }

        private void HighlightLastPlayerFromPreviousSessionInList()
        {
            if (MyPlayer != null)
                HighlightSelectedPlayer(MyPlayer.Nickname);
            else
                FindLastPlayerInHighscoreList();

        }

        private void FindLastPlayerInHighscoreList()
        {
            foreach (Player myPlayer in ListOfPlayersInListBox)
            {
                if (myPlayer.LastPlayer == true)
                {
                    MyPlayer = myPlayer;
                    HighlightSelectedPlayer(MyPlayer.Nickname);
                }
            }
        }

    }
}
