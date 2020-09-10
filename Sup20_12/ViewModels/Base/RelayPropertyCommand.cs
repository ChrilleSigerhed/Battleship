using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Sup20_12.ViewModels.Base
{
    public class RelayPropertyCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action<string> action { get; set; }
        public RelayPropertyCommand(Action<string> action)
        {
            this.action = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action.Invoke(parameter as string);
        }
    }
}
