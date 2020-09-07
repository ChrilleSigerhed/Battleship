using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sup20_12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Player myPlayer = new Player("SirMrNoName");
            myPlayer = DbConnection.AddNewPlayerToDb(myPlayer);

            Highscore myHighscore = new Highscore(true, 99, myPlayer.Id);
            myHighscore = DbConnection.AddOneHighscoreToDb(myHighscore);

        }
    }
}
