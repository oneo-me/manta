using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Manta.Extensions;
using Manta.Native;

namespace Manta.Controls;

[TemplatePart(nameof(PART_Decorations), typeof(Panel))]
[TemplatePart(nameof(PART_WindowContent), typeof(Grid))]
public sealed partial class MWindow : Window
{
    protected override Type StyleKeyOverride { get; } = typeof(MWindow);

    public static readonly StyledProperty<bool> IsMainWindowProperty = AvaloniaProperty.Register<MWindow, bool>(
        nameof(IsMainWindow));

    public bool IsMainWindow
    {
        get => GetValue(IsMainWindowProperty);
        internal set => SetValue(IsMainWindowProperty, value);
    }

    public static readonly StyledProperty<bool> AllowFullScreenProperty = AvaloniaProperty.Register<MWindow, bool>(
        nameof(AllowFullScreen));

    public bool AllowFullScreen
    {
        get => GetValue(AllowFullScreenProperty);
        set => SetValue(AllowFullScreenProperty, value);
    }

    public static readonly StyledProperty<bool> CloseWhenDeactivatedProperty = AvaloniaProperty.Register<MWindow, bool>(
        nameof(CloseWhenDeactivated));

    public bool CloseWhenDeactivated
    {
        get => GetValue(CloseWhenDeactivatedProperty);
        set => SetValue(CloseWhenDeactivatedProperty, value);
    }

    public MWindow()
    {
        Deactivated += OnDeactivated;

        if (OperatingSystem.IsMacOS() is false)
            return;

        KeyUp += OnKeyUp;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        MantaWindow.ConfigureWindow(this);
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        this.FocusActivate();
    }

    void OnDeactivated(object? sender, EventArgs e)
    {
        if (CloseWhenDeactivated)
            Close();
    }

    void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.W && e.KeyModifiers.HasFlag(KeyModifiers.Meta))
            Close();
    }

    Panel? PART_Decorations;
    Grid? PART_WindowContent;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (Design.IsDesignMode)
            return;

        PART_WindowContent = e.NameScope.Get<Grid>(nameof(PART_WindowContent));
        PrepareAttach?.Invoke(this, PART_WindowContent);

        if (OperatingSystem.IsLinux() is false)
            return;

        PART_Decorations = e.NameScope.Get<Panel>(nameof(PART_Decorations));

        const double size = 8d;

        var background = new SolidColorBrush(new Color(0, 0, 0, 0));

        PART_Decorations.Children.Add(new Border
        {
            Cursor = new(StandardCursorType.TopLeftCorner),
            Tag = WindowEdge.NorthWest,
            Background = background,
            Width = size,
            Height = size,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        });
        PART_Decorations.Children.Add(new Border
        {
            Cursor = new(StandardCursorType.TopSide),
            Tag = WindowEdge.North,
            Background = background,
            Height = size,
            Margin = new(size, 0, size, 0),
            VerticalAlignment = VerticalAlignment.Top
        });
        PART_Decorations.Children.Add(new Border
        {
            Cursor = new(StandardCursorType.TopRightCorner),
            Tag = WindowEdge.NorthEast,
            Background = background,
            Width = size,
            Height = size,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top
        });
        PART_Decorations.Children.Add(new Border
        {
            Cursor = new(StandardCursorType.LeftSide),
            Tag = WindowEdge.West,
            Background = background,
            Width = size,
            Margin = new(0, size, 0, size),
            HorizontalAlignment = HorizontalAlignment.Left
        });
        PART_Decorations.Children.Add(new Border
        {
            Cursor = new(StandardCursorType.RightSide),
            Tag = WindowEdge.East,
            Background = background,
            Width = size,
            Margin = new(0, size, 0, size),
            HorizontalAlignment = HorizontalAlignment.Right
        });
        PART_Decorations.Children.Add(new Border
        {
            Cursor = new(StandardCursorType.BottomLeftCorner),
            Tag = WindowEdge.SouthWest,
            Background = background,
            Width = size,
            Height = size,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom
        });
        PART_Decorations.Children.Add(new Border
        {
            Cursor = new(StandardCursorType.BottomSide),
            Tag = WindowEdge.South,
            Background = background,
            Height = size,
            Margin = new(size, 0, size, 0),
            VerticalAlignment = VerticalAlignment.Bottom
        });
        PART_Decorations.Children.Add(new Border
        {
            Cursor = new(StandardCursorType.BottomRightCorner),
            Tag = WindowEdge.SouthEast,
            Background = background,
            Width = size,
            Height = size,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Bottom
        });

        foreach (var border in PART_Decorations.Children)
        {
            if (border.Tag is not WindowEdge)
                continue;

            border.PointerPressed += OnWindowBorderPointerPressed;
        }
    }

    void OnWindowBorderPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border { Tag: WindowEdge edge } && CanResize)
            BeginResizeDrag(edge, e);
    }
}
