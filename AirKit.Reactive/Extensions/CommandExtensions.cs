using System.Linq.Expressions;
using System.Windows.Input;
using R3;

namespace AirKit.Reactive.Extensions;

public static class CommandExtensions
{
    public static IDisposable InvokeCommand<T, TSource>(this Observable<T> observable, TSource source, Expression<Func<TSource, ICommand>> commandSelector)
    {
        var command = commandSelector.Compile().Invoke(source);

        return observable.Subscribe(x =>
        {
            if (command.CanExecute(x))
                command.Execute(x);
        });
    }

    public static IDisposable InvokeCommand<T>(this Observable<T> observable, ICommand command)
    {
        return observable.Subscribe(x =>
        {
            if (command.CanExecute(x))
                command.Execute(x);
        });
    }
}
