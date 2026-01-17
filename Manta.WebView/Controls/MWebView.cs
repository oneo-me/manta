using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Manta.WebView.Platform.macOS;

namespace Manta.WebView.Controls;

[TemplatePart(nameof(PART_NativeHost), typeof(Border))]
public class MWebView : TemplatedControl
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

    readonly MacOSNativeControlHost? _nativeHost;

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
        if (_nativeHost is null)
            return;

        if (string.IsNullOrEmpty(url))
            return;

        _nativeHost.WebView.LoadUrl(url);
    }

    void OnHtmlContentChanged(string html)
    {
        if (_nativeHost is null)
            return;

        if (string.IsNullOrEmpty(html))
            return;

        _nativeHost.WebView.LoadHtmlString(html);
    }

    public void Reload()
    {
        if (_nativeHost is null)
            return;

        _nativeHost.WebView.Reload();
    }

    public void GoBack()
    {
        if (_nativeHost is null)
            return;

        _nativeHost.WebView.GoBack();
    }

    public void GoForward()
    {
        if (_nativeHost is null)
            return;

        _nativeHost.WebView.GoForward();
    }

    public void ExecuteJavaScript(string script, Action<string?>? callback = null)
    {
        if (_nativeHost is null)
            return;

        _nativeHost.WebView.ExecuteJavaScript(script, callback);
    }
}
