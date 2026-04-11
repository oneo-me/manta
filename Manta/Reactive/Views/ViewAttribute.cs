using System.Diagnostics.CodeAnalysis;
using Manta.Controls;

namespace Manta.Reactive.Views;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
[SuppressMessage("ReSharper", "UnusedTypeParameter")]
public sealed class ViewAttribute<T> : Attribute where T : MView, new();
