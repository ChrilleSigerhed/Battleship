using Sup20_12.View;
using Sup20_12.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sup20_12
{
   public class MainMenuViewModel : BaseViewModel
    {
        public ICommand ChoosePlayerPageCommand { get; set; }
        public ICommand HighscorePageCommand { get; set; }
        public ICommand ExitGameCommand { get; set; }
        public Page SelectedPage { get; set; }
        public MainMenuViewModel()
        {
            ChoosePlayerPageCommand = new RelayCommand(GoToChoosePlayer);
            HighscorePageCommand = new RelayCommand(GoToHighscorePage);
            ExitGameCommand = new RelayCommand(ExitGame);
        }
        public void GoToChoosePlayer()
        {
            var page = new ChoosePlayerPage();
            SelectedPage = page;
            
        }
        public void GoToHighscorePage()
        {
            var page = new HighScorePage();
            SelectedPage = page;
        }
        public void ExitGame()
        {
            // Lägga till funktion som avslutar spelet
        }
    }
}
