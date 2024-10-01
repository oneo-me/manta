using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using R3;

namespace AirKit.Reactive.Controls;

[SuppressMessage("Design", "CA1001:具有可释放字段的类型应该是可释放的")]
public class LightView : ContentControl
{
    CompositeDisposable? _disposables;

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        _disposables?.Dispose();
        _disposables = new();

        if (DataContext is not ViewModel viewModel)
            return;

        if (Design.IsDesignMode)
            viewModel.InnerView = this;

        viewModel.OnInnerViewActivated(_disposables);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        _disposables?.Dispose();
        _disposables = null;
    }
}
