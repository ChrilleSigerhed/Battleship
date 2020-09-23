using Sup20_12.View;
using Sup20_12.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;


namespace Sup20_12
{
    public class HighScoreViewModel : BaseViewModel
    {
        public int MyProperty { get; set; }

        public string HighScoreLst { get; set; }
        public string FrequentPlayers { get; set; }

        public ICommand GoToMainPage { get; set; }

        public HighScoreViewModel()
        {
            GetHighScores();
            GetFrequentPlayers();
            GoToMainPage = new RelayCommand(GoToMain);
        }

        public void GetHighScores()
        {
            List<Highscore> myTempHighscoreList;
            myTempHighscoreList = (List<Highscore>)DbConnection.GetThreeWinnersFromHighscore();
            HighScoreLst = ConvertHighscoreListToString(myTempHighscoreList);
        }

        private void GetFrequentPlayers()
        {
            List<Player> myTempPlayerList;
            myTempPlayerList = (List<Player>)DbConnection.GetThreeFrequentPlayersFromHighscore();
            FrequentPlayers = ConvertFrequentPlayerListToString(myTempPlayerList);
        }

        public void GoToMain()
        {
            MyWin.frame.Content = new MainMenuPage();
        }

        private string ConvertFrequentPlayerListToString(List<Player> myTempPlayerList)
        {
            string frequentPlayers = "SPELARE\t\t\tANTAL\n";
            foreach (var myPlayer in myTempPlayerList)
            {
                frequentPlayers += myPlayer.Nickname;
                frequentPlayers += $"{AddTabToHighscoreList(myPlayer.Nickname)}";
                frequentPlayers += myPlayer.NumberOfGamesPlayed;
                frequentPlayers += "\n";
            }
            return frequentPlayers;
        }

        private string ConvertHighscoreListToString(List<Highscore> myTempHighscoreList)
        {
            string highscores = "SPELARE\t\t\tDRAG\n";
            foreach (var myHighscore in myTempHighscoreList)
            {
                highscores += myHighscore.Nickname;
                highscores += $"{AddTabToHighscoreList(myHighscore.Nickname)}";
                highscores += myHighscore.NumberOfMoves;
                highscores += "\n";
            }
            return highscores;
        }

        private string AddTabToHighscoreList(string nickname)
        {
            if (nickname.Length < 10)
                return "\t\t\t";
            else
                return "\t\t";
        }
    }
}
