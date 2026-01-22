using Avalonia;
using Avalonia.VisualTree;
using Manta.Controls;

namespace Manta.Reactive.Views;

public static class ViewModelContext
{
    extension(ViewModel viewModel)
    {
        public void SetContext<T>(string key, T context) where T : class
        {
            viewModel.Context ??= new();
            viewModel.Context[key] = context;
        }

        public T? GetContext<T>(string key) where T : class
        {
            viewModel.Context ??= new();

            if (viewModel.Context.TryGetValue(key, out var value) && value is T typedValue)
                return typedValue;

            var view = viewModel.InnerView;
            if (view is null)
                throw new InvalidOperationException("The ViewModel is not associated with any view.");

            while (true)
            {
                var parent = view.FindAncestorOfType<MView>();
                if (parent is null)
                    break;

                if (parent.DataContext is ViewModel { Context: not null } parentViewModel)
                    return parentViewModel.GetContext<T>(key);
            }

            return null;
        }

        public Dictionary<string, object?> GetContexts()
        {
            var contexts = new Dictionary<string, object?>();

            if (viewModel.Context is not null)
                foreach (var kvp in viewModel.Context)
                    contexts[kvp.Key] = kvp.Value;

            var view = viewModel.InnerView;
            if (view is null)
                throw new InvalidOperationException("The ViewModel is not associated with any view.");

            var parentView = view.FindAncestorOfType<MView>();
            if (parentView?.DataContext is ViewModel parentViewModel)
            {
                var parentContexts = parentViewModel.GetContexts();
                foreach (var kvp in parentContexts)
                    if (contexts.ContainsKey(kvp.Key) is false)
                        contexts[kvp.Key] = kvp.Value;
            }

            return contexts;
        }
    }

    public static T? GetContext<T>(Visual visual, string key) where T : class
    {
        var view = visual.FindAncestorOfType<MView>();
        if (view?.DataContext is ViewModel viewModel)
            return viewModel.GetContext<T>(key);

        return null;
    }

    public static Dictionary<string, object?> GetContexts(Visual visual)
    {
        var view = visual.FindAncestorOfType<MView>();
        if (view?.DataContext is ViewModel viewModel)
            return viewModel.GetContexts();

        return new();
    }
}
