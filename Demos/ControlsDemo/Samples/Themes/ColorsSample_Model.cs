using System.Reactive.Disposables;
using Avalonia.Controls;
using ControlsDemo.Langs;
using Axe.Reactive;
using ControlsDemo.Models;

namespace ControlsDemo.Samples.Themes;

[Sample(SampleGroupType.Themes, LangKey.SampleName_Colors)]
[View<ColorsSample>]
sealed class ColorsSample_Model : ViewModel
{
    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        var colorsSample = this.GetView<ColorsSample>();

        Observable
            .FromEventHandler<ScrollChangedEventArgs>(x => colorsSample.ScrollViewer.ScrollChanged += x, x => colorsSample.ScrollViewer.ScrollChanged -= x)
            .Subscribe(_ =>
            {
                var b = colorsSample.ScrollViewer.Offset.Y > 32;
                colorsSample.BackgroundBlur.Opacity = b ? 1 : 0;
            }).DisposeWith(disposables);
    }
}
