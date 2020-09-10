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
        public GameWindowPage()
        {
            InitializeComponent();
            DataContext = new GameWindowViewModel();
        }
    }
}
