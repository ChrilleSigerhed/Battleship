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

        public List<Highscore> HighScoreLst { get; set; }

        public ICommand GoToMainPage { get; set; }

        public HighScoreViewModel()
        {
            GetHighScores();
            GoToMainPage = new RelayCommand(GoToMain);
        }

        public List<Highscore> GetHighScores()
        {
            HighScoreLst = (List<Highscore>)DbConnection.GetAllHighscores();
            return HighScoreLst;
        }

        public void GoToMain()
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            win.frame.Content = new MainMenuPage();
        }

    }
}
