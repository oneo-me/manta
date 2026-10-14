using System.Windows.Input;

namespace MantaComponents.ViewModels;

/// <summary>
/// Minimal <see cref="ICommand"/> implementation, so the sample does not depend on Manta commands
/// or any other MVVM package.
/// </summary>
sealed class RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null) : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return canExecute?.Invoke(parameter) ?? true;
    }

    public void Execute(object? parameter)
    {
        execute(parameter);
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
