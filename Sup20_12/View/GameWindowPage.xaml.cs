using Sup20_12.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            gameWindowViewModel = new GameWindowViewModel(rectangleUI, rectangleBS, rectangleSub);
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
            if(gameWindowViewModel.Ships == 2)
            {
                rectangleUI.Visibility = Visibility.Hidden;
            }
            else if(gameWindowViewModel.Ships == 1)
            {
                rectangleBS.Visibility = Visibility.Hidden;
            }
            else if(gameWindowViewModel.Ships == 0)
            {
                rectangleSub.Visibility = Visibility.Hidden;
            }
            BtnSlump.IsEnabled = false;
            BtnSlump.Background = Brushes.Gray;
            BtnSlump.Content = "";    
        }
        private void RemoveBoatsAfterRandomPlacedShips(object sender, RoutedEventArgs e)
        {
            rectangleUI.Visibility = Visibility.Hidden;
            rectangleBS.Visibility = Visibility.Hidden;
            rectangleSub.Visibility = Visibility.Hidden;
        }
    }
}
