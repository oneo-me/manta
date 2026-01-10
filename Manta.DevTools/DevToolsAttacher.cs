using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Manta.Controls;
using Manta.DevTools.Views;
using Manta.Reactive.Views;
using Manta.Themes.Variables;

namespace Manta.DevTools;

sealed class DevToolsAttacher
{
    readonly DevToolsOptions _options;
    readonly MWindow _window;
    readonly MView _view;
    readonly Grid _windowContent;
    readonly Main_Model _mainModel = new();

    public DevToolsAttacher(DevToolsOptions options, MWindow window, Grid windowContent)
    {
        _options = options;
        _window = window;
        _windowContent = windowContent;
        _view = _mainModel.CreateView();

        window.KeyUp += WindowOnKeyUp;
        if (options.AutoShowDevToolsOnStart)
            window.Opened += WindowOnOpened;
    }

    void WindowOnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.I && (e.KeyModifiers.HasFlag(KeyModifiers.Control | KeyModifiers.Shift) || e.KeyModifiers.HasFlag(KeyModifiers.Meta | KeyModifiers.Shift)))
            ShowOrHideDevTools();
    }

    void WindowOnOpened(object? sender, EventArgs e)
    {
        ShowOrHideDevTools();
    }

    bool _isCreated;

    void ShowOrHideDevTools()
    {
        if (_isCreated)
        {
            var isVisible = _mainModel.IsVisible;
            if (isVisible)
            {
                _mainModel.IsVisible = false;
                _view.Height = _windowContent.RowDefinitions[1].Height.Value;
                _windowContent.RowDefinitions[1].Height = new(0, GridUnitType.Pixel);
            }
            else
            {
                _mainModel.IsVisible = true;
                var height = _view.Height > 0 ? _view.Height : _options.DevToolsDefaultHeight;
                _windowContent.RowDefinitions[1].Height = new(height, GridUnitType.Pixel);
                _view.Height = double.NaN;
            }

            return;
        }

        _isCreated = true;
        _windowContent.RowDefinitions =
        [
            new(GridLength.Star),
            new(_options.DevToolsDefaultHeight, GridUnitType.Pixel)
        ];

        Grid.SetRow(_view, 1);
        _windowContent.Children.Add(_view);

        var gridSplitter = new GridSplitter
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top
        };
        Grid.SetRow(gridSplitter, 1);
        _windowContent.Children.Add(gridSplitter);

        _view[!TemplatedControl.BorderBrushProperty] = new MBrush(MBrushKey.Border).GetResourceBinding(_view);
        _view[!TemplatedControl.BorderThicknessProperty] = new MThickness(MSizeVariant.Px) { Filter = "Top" }.GetResourceBinding(_view);

        _view[!Visual.IsVisibleProperty] = new Binding(nameof(Main_Model.IsVisible)) { Source = _mainModel };
        gridSplitter[!Visual.IsVisibleProperty] = new Binding(nameof(Main_Model.IsVisible)) { Source = _mainModel };
    }
}
