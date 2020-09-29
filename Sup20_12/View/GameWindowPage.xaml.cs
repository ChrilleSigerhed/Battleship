using Sup20_12.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sup20_12.View
{
    /// <summary>
    /// Interaction logic for GameWindowPage.xaml
    /// </summary>

    public partial class GameWindowPage : Page
    {
        public Player Player { get; set; }
        public GameWindowViewModel gameWindowViewModel { get; set; }

        public GameWindowPage(Player player)
        {
            Player = player;
            InitializeComponent();
            gameWindowViewModel = new GameWindowViewModel(rectangleUI);
            DataContext = gameWindowViewModel;
        }
        private void Target_Drop(object sender, DragEventArgs e)
        {
            Button button = (Button)sender;
            if (gameWindowViewModel.Ships == 3)
            {
                gameWindowViewModel.PlayerPlaceShips(button.CommandParameter.ToString());
            }
            else if (gameWindowViewModel.Ships == 2)
            {
                gameWindowViewModel.PlayerPlaceBattleShip(button.CommandParameter.ToString());
            }
            else if(gameWindowViewModel.Ships == 1)
            {
                gameWindowViewModel.PlayerPlaceSubmarineShip(button.CommandParameter.ToString());
            }
           
                
        }
        private void rectangleUI_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveBoats(object sender, RoutedEventArgs e)
        {
            rectangleUI.Visibility = Visibility.Hidden;
        }
    }
}
