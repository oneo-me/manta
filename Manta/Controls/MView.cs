using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Manta.Reactive.Views;

namespace Manta.Controls;

public class MView : ContentControl
{
    CompositeDisposable? _disposables;

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        _disposables?.Dispose();
        _disposables = new();

        if (DataContext is not ViewModel viewModel)
            return;

        if (Design.IsDesignMode && viewModel.InnerView is null)
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
