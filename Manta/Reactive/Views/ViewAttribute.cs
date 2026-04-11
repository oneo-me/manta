using Manta.Controls;

namespace Manta.Reactive.Views;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ViewAttribute<TView> : Attribute where TView : MView, new();
