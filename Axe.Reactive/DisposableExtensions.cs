using System.Reactive.Disposables;

namespace Axe.Reactive;

public static class DisposableExtensions
{
    public static T DisposeWith<T>(this T disposable, CompositeDisposable compositeDisposable) where T : IDisposable
    {
        compositeDisposable.Add(disposable);
        return disposable;
    }

    public static void DisposeIfDisposable(this object obj)
    {
        if (obj is IDisposable disposable)
            disposable.Dispose();
    }

    public static void DisposeWithIfDisposable(this object obj, CompositeDisposable compositeDisposable)
    {
        if (obj is IDisposable disposable)
            disposable.DisposeWith(compositeDisposable);
    }
}
