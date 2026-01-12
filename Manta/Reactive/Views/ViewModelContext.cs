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

        public bool GetContext<T>(MView view, string key, out T? context) where T : class
        {
            viewModel.Context ??= new();

            if (viewModel.Context.TryGetValue(key, out var value) && value is T typedValue)
            {
                context = typedValue;
                return true;
            }

            while (true)
            {
                var parent = view.FindAncestorOfType<MView>();
                if (parent is null)
                    break;

                if (parent.DataContext is ViewModel { Context: not null } parentViewModel)
                    return parentViewModel.GetContext(parent, key, out context);
            }

            context = null;
            return false;
        }

        public Dictionary<string, object?> GetContexts(MView view)
        {
            var contexts = new Dictionary<string, object?>();

            if (viewModel.Context is not null)
                foreach (var kvp in viewModel.Context)
                    contexts[kvp.Key] = kvp.Value;

            var parentView = view.FindAncestorOfType<MView>();
            if (parentView?.DataContext is ViewModel parentViewModel)
            {
                var parentContexts = parentViewModel.GetContexts(parentView);
                foreach (var kvp in parentContexts)
                    if (contexts.ContainsKey(kvp.Key) is false)
                        contexts[kvp.Key] = kvp.Value;
            }

            return contexts;
        }
    }

    public static bool GetContext<T>(Visual visual, string key, out T? context) where T : class
    {
        var view = visual.FindAncestorOfType<MView>();
        if (view?.DataContext is ViewModel viewModel)
            return viewModel.GetContext(view, key, out context);

        context = null;
        return false;
    }

    public static Dictionary<string, object?> GetContexts(Visual visual)
    {
        var view = visual.FindAncestorOfType<MView>();
        if (view?.DataContext is ViewModel viewModel)
            return viewModel.GetContexts(view);

        return new();
    }
}
