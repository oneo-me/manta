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
        if (options.AutoShowByDefault)
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

    public void ShowOrHideDevTools()
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
                var height = _view.Height > 0 ? _view.Height : Options.DefaultHeight;
                _windowContent.RowDefinitions[1].Height = new(height, GridUnitType.Pixel);
                _view.Height = double.NaN;
            }

            return;
        }

        _isCreated = true;
        _windowContent.RowDefinitions =
        [
            new(GridLength.Star),
            new(Options.DefaultHeight, GridUnitType.Pixel) { MaxHeight = _window.Bounds.Height * 0.8 }
        ];

        _window.SizeChanged += (_, e) =>
        {
            _windowContent.RowDefinitions[1].MaxHeight = e.NewSize.Height * 0.8;
        };

        var gridSplitter = new GridSplitter
        {
            Margin = default,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top,
            [!Layoutable.HeightProperty] = new Binding(nameof(Main_Model.ToolBarHeight), BindingMode.OneWay) { Source = _mainModel }
        };
        Grid.SetRow(gridSplitter, 1);
        _windowContent.Children.Add(gridSplitter);

        Grid.SetRow(_view, 1);
        _windowContent.Children.Add(_view);

        _view[!Visual.IsVisibleProperty] = new Binding(nameof(Main_Model.IsVisible)) { Source = _mainModel };
        gridSplitter[!Visual.IsVisibleProperty] = new Binding(nameof(Main_Model.IsVisible)) { Source = _mainModel };
    }
}
