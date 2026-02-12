using Manta.Reactive.Views;

namespace Manta.DevTools.Models.PropertyItems;

abstract class ItemBase(string name, Type propertyType)
{
    public string Name { get; } = name;
    public Type PropertyType { get; } = propertyType;
    public virtual double Height => 36d;
    public virtual double Width => double.NaN;
    public abstract ViewModel CreateView();
}
