using System.Reactive.Disposables;

namespace LightKit.Reactive.Extensions;

public static class DisposableExtensions
{
    public static T DisposeWith<T>(this T disposable, CompositeDisposable compositeDisposable) where T : IDisposable
    {
        compositeDisposable.Add(disposable);
        return disposable;
    }

    extension(object obj)
    {
        public void DisposeIfDisposable()
        {
            if (obj is IDisposable disposable)
                disposable.Dispose();
        }

        public void DisposeWithIfDisposable(CompositeDisposable compositeDisposable)
        {
            if (obj is IDisposable disposable)
                disposable.DisposeWith(compositeDisposable);
        }
    }
}
