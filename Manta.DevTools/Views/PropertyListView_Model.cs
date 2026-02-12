using System.Reflection;
using Avalonia.Collections;
using Manta.DevTools.Models;
using Manta.DevTools.Models.PropertyItems;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views;

[View<PropertyListView>]
sealed class PropertyListView_Model : ViewModel, IViewHostTarget<object?>
{
    public static PropertyListView_Model Design => new();

    public AvaloniaList<ItemBase> Items { get; } = [];
    public ViewTemplate<ItemBase> ItemTemplate { get; } = new(item => item.CreateView());

    object? _lastData;

    public void SetData(object? data)
    {
        if (_lastData == data)
            return;

        _lastData = data;

        Items.Clear();

        if (_lastData is null)
            return;

        if (_lastData is TreeNode treeNode)
            LoadFromTreeNode(treeNode);
        else
            LoadFromObject(_lastData);
    }

    void LoadFromTreeNode(TreeNode node)
    {
        LoadContexts(node);
        LoadProperties(node.ViewModel);
    }

    void LoadFromObject(object obj)
    {
        LoadProperties(obj);
    }

    void LoadContexts(TreeNode node)
    {
        var contextDict = new Dictionary<string, ContextItem>();
        var current = node;

        while (current is not null)
        {
            foreach (var (key, value) in current.ViewModel.InnerGetContextItems())
            {
                if (contextDict.ContainsKey(key))
                    continue;

                contextDict[key] = new(key, value, value.GetType());
            }

            current = current.Parent;
        }

        var sortedContexts = contextDict.Values
            .OrderBy(c => c.Name, StringComparer.Ordinal)
            .ToList();

        if (sortedContexts.Count == 0)
            return;

        Items.Add(new GroupHeaderItem("Contexts"));
        Items.AddRange(sortedContexts);
    }

    void LoadProperties(object obj)
    {
        var items = new List<ItemBase>();

        var type = obj.GetType();
        var propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

        foreach (var prop in propertyInfos)
        {
            if (typeof(IViewContext).IsAssignableFrom(prop.PropertyType))
                continue;

            items.Add(PropertyItemBase.Create(obj, prop));
        }

        items.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));

        if (items.Count == 0)
            return;

        Items.Add(new GroupHeaderItem("Properties"));
        Items.AddRange(items);
    }
}
