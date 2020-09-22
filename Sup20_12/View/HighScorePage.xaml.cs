using System.Windows.Controls;

namespace Sup20_12.View
{
    /// <summary>
    /// Interaction logic for HighScorePage.xaml
    /// </summary>
    public partial class HighScorePage : Page
    {
        public HighScorePage()
        {
            InitializeComponent();
            DataContext = new HighScoreViewModel();
        }
    }
}
