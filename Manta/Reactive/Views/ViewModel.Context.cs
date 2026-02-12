using System.Runtime.CompilerServices;
using Avalonia.VisualTree;
using Manta.Controls;

namespace Manta.Reactive.Views;

public abstract partial class ViewModel
{
    Dictionary<string, IViewContext>? _contextCache;

    void OnLoadViewContexts()
    {
        if (_contextCache is null)
            return;

        foreach (var viewContext in _contextCache.Values)
        {
            var data = GetContextCore(viewContext.Key);
            viewContext.Source = data.source;
            viewContext.Value = data.value;
        }
    }

    protected ViewContext<T> GetContext<T>(string key, [CallerMemberName] string? name = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(name);

        _contextCache ??= new();

        if (_contextCache.TryGetValue(name, out var context))
            return (ViewContext<T>)context;

        context = new ViewContext<T>(key);
        _contextCache[name] = context;

        return (ViewContext<T>)context;
    }

    Dictionary<string, object>? _context;

    protected void SetContext<T>(string key, T value) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(key);

        _context ??= new();
        _context[key] = value;
    }

    (Type source, object value) GetContextCore(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (_context is not null && _context.TryGetValue(key, out var value))
            return (GetType(), value);

        var parent = FindParent();
        if (parent is not null)
            return parent.GetContextCore(key);

        return default;
    }

    ViewModel? FindParent()
    {
        var innerView = InnerView;
        if (innerView is null)
            throw new InvalidOperationException("The ViewModel is not attached to a View.");

        var parentView = innerView.FindAncestorOfType<MView>();
        if (parentView?.DataContext is ViewModel parentViewModel)
            return parentViewModel;

        return null;
    }

    /// <summary>
    ///     Using DevTools
    /// </summary>
    /// <returns></returns>
    internal IEnumerable<KeyValuePair<string, object>> InnerGetContextItems()
    {
        if (_context is null)
            yield break;

        foreach (var kvp in _context)
            yield return kvp;
    }
}
