﻿using Sup20_12.View;
using Sup20_12.ViewModels;
using Sup20_12.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace Sup20_12
{
    public class MainMenuViewModel : BaseViewModel
    {
        #region Properties
        public ICommand ChoosePlayerPageCommand { get; set; }
        public ICommand HighscorePageCommand { get; set; }
        public ICommand ExitGameCommand { get; set; }
        #endregion

        public MainMenuViewModel()
        {
            Global.MyWin = (MainWindow)Application.Current.MainWindow;
            DbConnection.InitializeDbPooling();
            ChoosePlayerPageCommand = new RelayCommand(GoToChoosePlayer);
            HighscorePageCommand = new RelayCommand(GoToHighscorePage);
            ExitGameCommand = new RelayCommand(ExitGame);
        }

        private void GoToChoosePlayer()
        {
            Global.MyWin.frame.Content = new ChoosePlayerPage();
        }

        private void GoToHighscorePage()
        {
            Global.MyWin.frame.Content = new HighScorePage();
        }

        private void ExitGame()
        {
            MessageBoxResult result = MessageBox.Show("Vill du verkligen avsluta?", "Avsluta", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Global.MyWin.Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}
