using Sup20_12.View;
using Sup20_12.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
            DbConnection.InitializeDbPooling();
            ChoosePlayerPageCommand = new RelayCommand(GoToChoosePlayer);
            HighscorePageCommand = new RelayCommand(GoToHighscorePage);
            ExitGameCommand = new RelayCommand(ExitGame);
        }
        public void GoToChoosePlayer()
        {
            MyWin.frame.Content = new ChoosePlayerPage();
        }
        public void GoToHighscorePage()
        {
            MyWin.frame.Content = new HighScorePage();
        }
        public void ExitGame()
        {
            MessageBoxResult result = MessageBox.Show("Vill du verkligen avsluta?", "Avsluta", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    MyWin.Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }    
            
        }
    }
}
