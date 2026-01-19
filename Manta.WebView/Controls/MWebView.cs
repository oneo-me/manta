using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Manta.Native.Platform;

namespace Manta.WebView.Controls;

[TemplatePart(nameof(PART_NativeHost), typeof(Border))]
public class MWebView : TemplatedControl, IDisposable
{
    public static readonly StyledProperty<string> UrlProperty = AvaloniaProperty.Register<MWebView, string>(
        nameof(Url), string.Empty);

    public string Url
    {
        get => GetValue(UrlProperty);
        set => SetValue(UrlProperty, value);
    }

    public static readonly StyledProperty<string> HtmlContentProperty = AvaloniaProperty.Register<MWebView, string>(
        nameof(HtmlContent), string.Empty);

    public string HtmlContent
    {
        get => GetValue(HtmlContentProperty);
        set => SetValue(HtmlContentProperty, value);
    }

    readonly MantaWebViewHost? _nativeHost;

    public MWebView()
    {
        if (Design.IsDesignMode)
            return;

        _nativeHost = new();
    }

    Border? PART_NativeHost;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_NativeHost = e.NameScope.Get<Border>(nameof(PART_NativeHost));

        if (_nativeHost is not null)
            PART_NativeHost.Child = _nativeHost;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == UrlProperty)
            OnUrlChanged(change.GetNewValue<string>());
        else if (change.Property == HtmlContentProperty)
            OnHtmlContentChanged(change.GetNewValue<string>());
    }

    void OnUrlChanged(string url)
    {
        _nativeHost?.LoadUrl(url);
    }

    void OnHtmlContentChanged(string html)
    {
        _nativeHost?.LoadHtmlString(html);
    }

    public void Dispose()
    {
        _nativeHost?.Dispose();
    }
}
