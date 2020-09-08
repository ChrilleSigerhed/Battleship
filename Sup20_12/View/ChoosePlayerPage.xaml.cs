using Sup20_12.ViewModels;
using System;
using System.Collections.Generic;
using System.Printing;
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
    /// Interaction logic for ChoosePlayerPage.xaml
    /// </summary>
    public partial class ChoosePlayerPage : Page
    {
       
        public ChoosePlayerPage()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }


        private void btnAddPlayer_Click(object sender, RoutedEventArgs e)
        {
            Player player = new Player(txtNickname.Text);
            DbConnection.AddNewPlayerToDb(player);
        }

    }
}
