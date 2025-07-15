using System.Windows.Input;

namespace AirKit;

public sealed class CommandWarper : ICommand
{
    readonly Action<object?> _execute;
    readonly Func<object?, bool>? _canExecute;

    public event EventHandler? CanExecuteChanged;

    public CommandWarper(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public CommandWarper(Action execute, Func<bool>? canExecute = null) : this(_ => execute(), canExecute is null ? null : _ => canExecute())
    {
    }

    bool _isExecuting;

    public CommandWarper(Func<object?, Task> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = obj =>
        {
            _isExecuting = true;
            OnChanged();
            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Task.Run(async () =>
            {
                await execute(obj).ConfigureAwait(false);
            }).ContinueWith(_ =>
            {
                _isExecuting = false;
                OnChanged();
            }, taskScheduler);
        };
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
        if (_isExecuting)
            return false;

        if (_canExecute is null)
            return true;

        return _canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }
}
