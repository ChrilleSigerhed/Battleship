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
        
        public GameWindowPage(Player player)
        {
            Player = player;
            InitializeComponent();
            DataContext = new GameWindowViewModel(Player);
        }
        private void Target_Drop(object sender, DragEventArgs e)
        {
            Style colorBrush = (Style)e.Data.GetData(typeof(Style));
            Target.Style = colorBrush;
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button r = (Button)sender;
            DataObject dataObject = new DataObject(r.Style);
            DragDrop.DoDragDrop(r, dataObject, DragDropEffects.Move);
        }
    }
}
