using Manta.Reactive.Views;

namespace Manta.DevTools.Models.PropertyItems;

sealed class ContextItem : ItemBase
{
    public PropertyItemBase PropertyItem { get; }
    public override double Width { get; }
    public override double Height { get; }

    public ContextItem(string name, object value, Type valueType) : base(name, valueType)
    {
        PropertyItem = PropertyItemBase.Create(name, value, ".", valueType, true, () => value);
        Width = PropertyItem.Width;
        Height = PropertyItem.Height;
    }

    public override ViewModel CreateView()
    {
        return PropertyItem.CreateView();
    }
}
