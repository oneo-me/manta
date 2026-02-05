using System.Collections;
using Avalonia.Collections;
using Manta.Controls;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views;

[View<CollectionPropertyDialog>]
sealed class CollectionPropertyDialog_Model : DialogModel
{
    public string PropertyName { get; set; } = string.Empty;
    public IEnumerable? PropertyValue { get; set; }

    public AvaloniaList<CollectionItemWrapper> Items { get; } = [];

    object? _selectedItem;
    public object? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem == value)
                return;
            _selectedItem = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedValue));
        }
    }

    public object? SelectedValue => (_selectedItem as CollectionItemWrapper)?.Value;

    public string Header
    {
        get
        {
            var count = 0;
            if (PropertyValue is ICollection collection)
                count = collection.Count;
            return $"{PropertyName} [{count}]";
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        LoadItems();
    }

    void LoadItems()
    {
        Items.Clear();

        if (PropertyValue is null)
            return;

        var index = 0;
        foreach (var item in PropertyValue)
        {
            Items.Add(new CollectionItemWrapper(index, item));
            index++;
        }

        if (Items.Count > 0)
            SelectedItem = Items[0];
    }

    public override void OnWindowInitialized(MWindow window)
    {
        base.OnWindowInitialized(window);

        window.Width = 700;
        window.Height = 500;
    }
}

sealed class CollectionItemWrapper(int index, object? value)
{
    public int Index { get; } = index;
    public object? Value { get; } = value;

    public string DisplayText => Value?.ToString() ?? "null";
    public string IndexText => $"[{Index}]";
    public bool IsSimpleType => Value is null || IsSimple(Value.GetType());

    static bool IsSimple(Type type)
    {
        var t = Nullable.GetUnderlyingType(type) ?? type;
        return t.IsPrimitive ||
               t == typeof(string) ||
               t == typeof(decimal) ||
               t == typeof(DateTime) ||
               t == typeof(DateTimeOffset) ||
               t == typeof(TimeSpan) ||
               t == typeof(Guid) ||
               t.IsEnum;
    }
}
