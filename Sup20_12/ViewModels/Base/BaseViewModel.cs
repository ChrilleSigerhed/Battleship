using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Sup20_12.ViewModels.Base
{
   public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
