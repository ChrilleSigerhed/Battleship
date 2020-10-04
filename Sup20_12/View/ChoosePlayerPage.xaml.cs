using Sup20_12.ViewModels;
using System.Windows.Controls;


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
            DataContext = new ChoosePlayerViewModel();
        }
    }
}
