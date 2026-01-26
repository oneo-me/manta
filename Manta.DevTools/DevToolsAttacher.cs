using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Manta.Controls;
using Manta.DevTools.Views;
using Manta.Reactive.Views;

namespace Manta.DevTools;

sealed class DevToolsAttacher
{
    readonly MWindow _window;
    readonly MView _view;
    readonly Grid _windowContent;
    readonly Main_Model _mainModel;

    public DevToolsOptions Options { get; }
    public Window SourceWindow => _window;

    public DevToolsAttacher(DevToolsOptions options, MWindow window, Grid windowContent)
    {
        Options = options;
        _window = window;
        _windowContent = windowContent;
        _mainModel = new(this);
        _view = _mainModel.CreateView();

        window.KeyUp += WindowOnKeyUp;

        if (options.AutoShowForMainWindow && window.IsMainWindow)
            window.Opened += WindowOnOpened;

        switch (window.Variant)
        {
            case MWindowVariant.Default:
            case MWindowVariant.TrayWindow:
            case MWindowVariant.DialogWindow:
            default:
                if (options.AutoShowForOtherWindows)
                    window.Opened += WindowOnOpened;
                break;
        }
    }

    bool _isAutoOpened;

    void WindowOnOpened(object? sender, EventArgs e)
    {
        if (_isAutoOpened)
            return;

        _isAutoOpened = true;
        ShowOrHideDevTools();
    }

    void WindowOnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.I && (e.KeyModifiers.HasFlag(KeyModifiers.Control | KeyModifiers.Shift) || e.KeyModifiers.HasFlag(KeyModifiers.Meta | KeyModifiers.Shift)))
            ShowOrHideDevTools();
    }

    bool _isCreated;

    const double DefWidth = 320d;

    public void ShowOrHideDevTools()
    {
        if (_isCreated)
        {
            var isVisible = _mainModel.IsVisible;
            var column = _windowContent.ColumnDefinitions[1];
            if (isVisible)
            {
                _mainModel.IsVisible = false;
                _view.Width = column.ActualWidth;
                column.MinWidth = 0;
                column.Width = new(0, GridUnitType.Pixel);
            }
            else
            {
                _mainModel.IsVisible = true;
                var width = _view.Width > 0 ? _view.Width : DefWidth;
                column.MinWidth = DefWidth;
                column.Width = new(width, GridUnitType.Pixel);
                _view.Width = double.NaN;
            }

            return;
        }

        _isCreated = true;
        _windowContent.ColumnDefinitions =
        [
            new(GridLength.Star),
            new(DefWidth, GridUnitType.Pixel) { MaxWidth = _window.Bounds.Width * 0.4, MinWidth = DefWidth }
        ];

        _window.SizeChanged += (_, e) =>
        {
            _windowContent.ColumnDefinitions[1].MaxWidth = e.NewSize.Width * 0.4;
        };

        Grid.SetColumn(_view, 1);
        _windowContent.Children.Add(_view);

        var gridSplitter = new GridSplitter
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch
        };
        Grid.SetColumn(gridSplitter, 1);
        _windowContent.Children.Add(gridSplitter);

        _view[!Visual.IsVisibleProperty] = new Binding(nameof(Main_Model.IsVisible)) { Source = _mainModel };
        gridSplitter[!Visual.IsVisibleProperty] = new Binding(nameof(Main_Model.IsVisible)) { Source = _mainModel };
    }
}
