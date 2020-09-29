using System.Windows.Controls;

namespace Sup20_12.View
{
    /// <summary>
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            InitializeComponent();
            DataContext = new MainMenuViewModel();
        }
    }
}
