using System.Reflection;
using Avalonia.Collections;
using Manta.DevTools.Models;
using Manta.DevTools.Models.Items;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views;

[View<PropertyListView>]
sealed class PropertyListView_Model : ViewModel, IViewHostTarget<object?>
{
    public static PropertyListView_Model Design => new();

    public AvaloniaList<ContextItem> Contexts { get; } = [];
    public AvaloniaList<PropertyItemBase> Properties { get; } = [];

    public bool HasContexts => Contexts.Count > 0;

    object? _lastData;

    public void SetData(object? data)
    {
        if (_lastData == data)
            return;

        _lastData = data;

        Contexts.Clear();
        Properties.Clear();

        if (_lastData is null)
            return;

        if (_lastData is TreeNode treeNode)
            LoadFromTreeNode(treeNode);
        else
            LoadFromObject(_lastData);

        OnPropertyChanged(nameof(HasContexts));
    }

    void LoadFromTreeNode(TreeNode node)
    {
        // Collect contexts from hierarchy (bottom-up, lower overrides upper)
        var contextDict = new Dictionary<string, ContextItem>();
        var current = node;

        while (current is not null)
        {
            foreach (var (key, value) in current.ViewModel.InnerGetContextItems())
            {
                // Skip if already exists (lower level takes precedence)
                if (contextDict.ContainsKey(key))
                    continue;

                contextDict[key] = new ContextItem(key, value, current.ViewModel.GetType(), current.ViewModel);
            }

            current = current.Parent;
        }

        // Sort contexts by key and add to list
        var sortedContexts = contextDict.Values
            .OrderBy(c => c.Key, StringComparer.Ordinal)
            .ToList();
        Contexts.AddRange(sortedContexts);

        // Load properties from ViewModel
        LoadProperties(node.ViewModel);
    }

    void LoadFromObject(object obj)
    {
        LoadProperties(obj);
    }

    void LoadProperties(object obj)
    {
        var type = obj.GetType();
        var propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        var propertyItems = new List<PropertyItemBase>();

        foreach (var prop in propertyInfos)
        {
            // Skip IViewContext properties
            if (typeof(IViewContext).IsAssignableFrom(prop.PropertyType))
                continue;

            // Skip indexers
            if (prop.GetIndexParameters().Length > 0)
                continue;

            try
            {
                propertyItems.Add(PropertyItemBase.Create(obj, prop));
            }
            catch
            {
                // Skip properties that throw on access
            }
        }

        // Sort by name
        propertyItems.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
        Properties.AddRange(propertyItems);
    }
}
