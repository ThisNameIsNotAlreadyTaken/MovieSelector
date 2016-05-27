using System;
using System.Windows.Input;

namespace Common.Instrastructure
{
    public class DelegateParametrizedCommand<T> : ICommand where T : class
    {
        private readonly Action<T> _action;

        public DelegateParametrizedCommand(Action<T> action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            T toNesessaryType = parameter as T;
            _action(toNesessaryType);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
