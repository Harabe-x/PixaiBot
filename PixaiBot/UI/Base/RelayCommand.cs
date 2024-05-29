using System;
using System.Windows.Input;

namespace PixaiBot.UI.Base;

public class RelayCommand : ICommand
{
    private readonly Func<object, bool> _canExecute;

    private readonly Action<object> _execute;

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return _canExecute?.Invoke(parameter) ?? true;
    }

    public void Execute(object parameter)
    {
        _execute?.Invoke(parameter);
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}