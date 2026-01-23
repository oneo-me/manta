using System.Runtime.CompilerServices;
using System.Windows.Input;
using Manta.Reactive.Commands;

namespace Manta.Reactive.Views;

public abstract class CommandModel : Model
{
    Dictionary<string, ICommand>? _commands;

    T GetCommandCore<T>(Func<T> commandFactory, string? name) where T : ICommand
    {
        ArgumentNullException.ThrowIfNull(name);

        _commands ??= new();

        if (_commands.TryGetValue(name, out var command))
            return (T)command;

        command = commandFactory();
        _commands[name] = command;

        return (T)command;
    }

    protected CommandWarper GetCommand(Action action, Func<bool>? canAction = null, [CallerMemberName] string? name = null)
    {
        return GetCommandCore(() => new CommandWarper(action, canAction), name);
    }

    protected CommandWarper<T> GetCommand<T>(Action<T> action, Func<T, bool>? canAction = null, [CallerMemberName] string? name = null)
    {
        return GetCommandCore(() => new CommandWarper<T>(action, canAction), name);
    }

    protected CommandWarper GetCommand(Func<Task> action, Func<bool>? canAction = null, [CallerMemberName] string? name = null)
    {
        return GetCommandCore(() => new CommandWarper(action, canAction), name);
    }

    protected CommandWarper<T> GetCommand<T>(Func<T, Task> action, Func<T, bool>? canAction = null, [CallerMemberName] string? name = null)
    {
        return GetCommandCore(() => new CommandWarper<T>(action, canAction), name);
    }
}
