using System.Windows.Input;
using Manta.Reactive.Views;

namespace Manta.Reactive.Commands;

public class CommandWarper : Model, ICommand
{
    readonly Action<object?>? _syncExecute;
    readonly Func<object?, Task>? _asyncExecute;
    readonly Func<object?, bool>? _canExecute;

    public event EventHandler? CanExecuteChanged;

    public CommandWarper(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _syncExecute = execute;
        _canExecute = canExecute;
    }

    public CommandWarper(Action execute, Func<bool>? canExecute = null) : this(_ => execute(), canExecute is null ? null : _ => canExecute())
    {
    }

    public bool IsExecuting
    {
        get;
        private set => SetValue(ref field, value);
    }

    public CommandWarper(Func<object?, Task> execute, Func<object?, bool>? canExecute = null)
    {
        _asyncExecute = execute;
        _canExecute = canExecute;
    }

    public CommandWarper(Func<Task> execute, Func<bool>? canExecute = null) : this(_ => execute(), canExecute is null ? null : _ => canExecute())
    {
    }

    public void OnChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool CanExecute(object? parameter)
    {
        if (IsExecuting)
            return false;

        if (_canExecute is null)
            return true;

        return _canExecute(parameter);
    }

    public async void Execute(object? parameter)
    {
        if (_asyncExecute is not null)
        {
            IsExecuting = true;
            OnChanged();
            try
            {
                await _asyncExecute(parameter);
            }
            finally
            {
                IsExecuting = false;
                OnChanged();
            }
        }
        else
        {
            _syncExecute?.Invoke(parameter);
        }
    }
}
