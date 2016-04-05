using System;
using System.Windows.Input;

namespace MovieSelector.Infrascturcture
{
    public class DelegateParameterCommand<T> : ICommand where T: class 
    {
        private readonly Action<T> _action;

        public DelegateParameterCommand(Action<T> action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            _action(parameter as T);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
