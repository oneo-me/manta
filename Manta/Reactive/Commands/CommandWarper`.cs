namespace Manta.Reactive.Commands;

public sealed class CommandWarper<T> : CommandWarper
{
    public CommandWarper(Action<T> execute, Func<T, bool>? canExecute = null) : base(x =>
    {
        if (x is T value) execute(value);
    }, canExecute is null ? null : x => x is T value && canExecute(value))
    {
    }

    public CommandWarper(Action execute, Func<bool>? canExecute = null) : base(execute, canExecute)
    {
    }

    public CommandWarper(Func<T, Task> execute, Func<T, bool>? canExecute = null) : base(async x =>
    {
        if (x is T value) await execute(value);
    }, canExecute is null ? null : x => x is T value && canExecute(value))
    {
    }

    public CommandWarper(Func<Task> execute, Func<bool>? canExecute = null) : base(execute, canExecute)
    {
    }
}
