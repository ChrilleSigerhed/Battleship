using System;
using System.Windows.Input;

namespace Sup20_12.ViewModels.Base
{
    public class RelayPropertyCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action<string> MyAction { get; set; }
        public RelayPropertyCommand(Action<string> action)
        {
            MyAction = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MyAction.Invoke(parameter as string);
        }
    }
}
