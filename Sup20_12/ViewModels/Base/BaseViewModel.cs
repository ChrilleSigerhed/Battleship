using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace Sup20_12.ViewModels.Base
{
   public class BaseViewModel : INotifyPropertyChanged
    {
        public MainWindow MyWin { get; } = (MainWindow)Application.Current.MainWindow;
        public static Player MyPlayer { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
