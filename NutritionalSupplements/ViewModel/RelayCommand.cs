using System;
using System.Windows.Input;

namespace NutritionalSupplements.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Func<object, bool>? _canExecute;
        private readonly Action<object> _onExecute;

        public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
        {
            this._canExecute = canExecute;
            _onExecute = execute ?? throw new ArgumentNullException(execute?.ToString());
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute.Invoke(parameter);

        public void Execute(object? parameter) => _onExecute.Invoke(parameter);
    }
}
