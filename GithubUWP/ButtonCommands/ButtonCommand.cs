using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GithubUWP.ButtonCommands
{
    public class ButtonCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        Action<object> _action;

        public ButtonCommand(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
