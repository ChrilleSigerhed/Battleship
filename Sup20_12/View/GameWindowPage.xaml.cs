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
        public ImageBrush bitmap { get; set; }

        public GameWindowPage(Player player)
        {
            Player = player;
            InitializeComponent();
            gameWindowViewModel = new GameWindowViewModel(Player, rectangleUI);
            DataContext = gameWindowViewModel;
        }
        private void Target_Drop(object sender, DragEventArgs e)
        {
            bitmap = GetImageSingleBoat();
            Button button = (Button)sender;
            button.Background = Brushes.Transparent;
            button.Background = bitmap;
            if (button.Background == bitmap) // Genom att kontrollera bakgrunden på vad det är vi droppar så kan vi köra olika metoder för olika båtar... om vi väljer att implementera det
            {
                gameWindowViewModel.PlayerPlaceShips(button.CommandParameter.ToString());
            }
        }
        private ImageBrush GetImageSingleBoat()
        {
            ImageBrush bitmap = new ImageBrush();
            bitmap.Stretch = Stretch.Uniform;
            bitmap.ImageSource = BitmapFrame.Create(new Uri(@"pack://Application:,,,/Assets/Images/destroyerImg.png", UriKind.Absolute));
            return bitmap;
        }

        private void rectangleUI_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
