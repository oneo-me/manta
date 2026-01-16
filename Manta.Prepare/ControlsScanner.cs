using System.Text;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Remote;
using Avalonia.Controls.Shapes;
using Avalonia.Dialogs;
using BindingFlags = System.Reflection.BindingFlags;
using Path = System.IO.Path;

namespace Manta.Prepare;

sealed class ControlImplInfo(string group, string name)
{
    public string Group { get; } = group;
    public string Name { get; } = name;
}

sealed class ControlInfo
{
    public Type Type { get; }
    public string TypeName { get; }
    public ControlImplInfo Impl { get; }

    public ControlInfo(Type type)
    {
        Type = type;
        TypeName = type.Name;
        Impl = TypeName switch
        {
            // Exclude controls that do not require support
            nameof(AboutAvaloniaDialog)           => new(string.Empty, string.Empty),
            nameof(WindowBase)                    => new(string.Empty, string.Empty),
            nameof(Control)                       => new(string.Empty, string.Empty),
            nameof(ContentPresenter)              => new(string.Empty, string.Empty),
            nameof(ScrollContentPresenter)        => new(string.Empty, string.Empty),
            nameof(ItemsPresenter)                => new(string.Empty, string.Empty),
            nameof(Decorator)                     => new(string.Empty, string.Empty),
            nameof(Viewbox)                       => new(string.Empty, string.Empty),
            nameof(Canvas)                        => new(string.Empty, string.Empty),
            nameof(VisualLayerManager)            => new(string.Empty, string.Empty),
            nameof(OverlayLayer)                  => new(string.Empty, string.Empty),
            nameof(ChromeOverlayLayer)            => new(string.Empty, string.Empty),
            nameof(LightDismissOverlayLayer)      => new(string.Empty, string.Empty),
            nameof(ExperimentalAcrylicBorder)     => new(string.Empty, string.Empty),
            nameof(TemplatedControl)              => new(string.Empty, string.Empty),
            nameof(LayoutTransformControl)        => new(string.Empty, string.Empty),
            nameof(ContentControl)                => new(string.Empty, string.Empty),
            nameof(SelectingItemsControl)         => new(string.Empty, string.Empty),
            nameof(HeaderedContentControl)        => new(string.Empty, string.Empty),
            nameof(HeaderedItemsControl)          => new(string.Empty, string.Empty),
            nameof(HeaderedSelectingItemsControl) => new(string.Empty, string.Empty),
            nameof(Popup)                         => new(string.Empty, string.Empty),
            nameof(NativeControlHost)             => new(string.Empty, string.Empty),
            nameof(RemoteWidget)                  => new(string.Empty, string.Empty),
            nameof(Sector)                        => new(string.Empty, string.Empty),
            nameof(TextPresenter)                 => new(string.Empty, string.Empty),
            nameof(TextSelectorLayer)             => new(string.Empty, string.Empty),
            nameof(TickBar)                       => new(string.Empty, string.Empty),
            nameof(Track)                         => new(string.Empty, string.Empty),
            nameof(Image)                         => new(string.Empty, string.Empty),
            nameof(NativeMenuBar)                 => new(string.Empty, string.Empty),

            // Exclude touch screen controls
            nameof(RefreshContainer)    => new(string.Empty, string.Empty),
            nameof(RefreshVisualizer)   => new(string.Empty, string.Empty),
            nameof(TextSelectionHandle) => new(string.Empty, string.Empty),

            // Exclude layout controls
            nameof(DockPanel)                 => new(string.Empty, string.Empty),
            nameof(Grid)                      => new(string.Empty, string.Empty),
            nameof(RelativePanel)             => new(string.Empty, string.Empty),
            nameof(ReversibleStackPanel)      => new(string.Empty, string.Empty),
            nameof(StackPanel)                => new(string.Empty, string.Empty),
            nameof(UniformGrid)               => new(string.Empty, string.Empty),
            nameof(WrapPanel)                 => new(string.Empty, string.Empty),
            nameof(VirtualizingStackPanel)    => new(string.Empty, string.Empty),
            nameof(VirtualizingCarouselPanel) => new(string.Empty, string.Empty),
            nameof(DateTimePickerPanel)       => new(string.Empty, string.Empty),

            // Exclude shape controls
            nameof(Avalonia.Controls.Shapes.Path) => new(string.Empty, string.Empty),
            nameof(Arc)                           => new(string.Empty, string.Empty),
            nameof(Ellipse)                       => new(string.Empty, string.Empty),
            nameof(Line)                          => new(string.Empty, string.Empty),
            nameof(Polygon)                       => new(string.Empty, string.Empty),
            nameof(Polyline)                      => new(string.Empty, string.Empty),
            nameof(Rectangle)                     => new(string.Empty, string.Empty),

            // Rename specific controls
            nameof(ContextMenu)         => new($"M{nameof(Menu)}", $"M{nameof(ContextMenu)}"),
            nameof(MenuItem)            => new($"M{nameof(Menu)}", $"M{nameof(MenuItem)}"),
            nameof(MenuFlyoutPresenter) => new($"M{nameof(Flyout)}", $"M{nameof(MenuFlyoutPresenter)}"),
            nameof(FlyoutPresenter)     => new($"M{nameof(Flyout)}", $"M{nameof(FlyoutPresenter)}"),
            nameof(UserControl)         => new("MView", "MView"),
            nameof(TreeView)            => new("MTree", "MTree"),
            nameof(TreeViewItem)        => new("MTree", "MTreeItem"),
            nameof(ItemsControl)        => new("MItems", "MItems"),
            nameof(TabItem)             => new($"M{nameof(TabControl)}", $"M{nameof(TabItem)}"),
            nameof(TabStripItem)        => new($"M{nameof(TabStrip)}", $"M{nameof(TabStripItem)}"),
            nameof(ListBoxItem)         => new($"M{nameof(ListBox)}", $"M{nameof(ListBoxItem)}"),
            nameof(ComboBoxItem)        => new($"M{nameof(ComboBox)}", $"M{nameof(ComboBoxItem)}"),
            nameof(ScrollBar)           => new($"M{nameof(ScrollViewer)}", $"M{nameof(ScrollBar)}"),
            nameof(CaptionButtons)      => new($"M{nameof(TitleBar)}", "MTitleBarButtons"),
            nameof(CalendarButton)      => new($"M{nameof(Calendar)}", $"M{nameof(CalendarButton)}"),
            nameof(CalendarDayButton)   => new($"M{nameof(Calendar)}", $"M{nameof(CalendarDayButton)}"),
            nameof(CalendarItem)        => new($"M{nameof(Calendar)}", $"M{nameof(CalendarItem)}"),
            nameof(OverlayPopupHost)    => new($"M{nameof(PopupRoot)}", $"M{nameof(OverlayPopupHost)}"),

            // Return to default implementation
            _ => new($"M{TypeName}", $"M{TypeName}")
        };
    }
}

sealed class ControlsScanner(Options options)
{
    public void Prepare()
    {
        var assemblys = new[] { typeof(Button).Assembly };
        var controlTypes = assemblys
            .Distinct()
            .SelectMany(x => x.GetTypes().Where(t => t is { IsPublic: true, IsAbstract: false } && typeof(Control).IsAssignableFrom(t)))
            .Select(x => new ControlInfo(x))
            .Where(x => string.IsNullOrWhiteSpace(x.Impl.Group) is false)
            .GroupBy(x => x.Impl.Group)
            .OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
            .ToList();

        // Clear directory
        if (Directory.Exists(options.SupportedControlsDir))
            Directory.Delete(options.SupportedControlsDir, true);
        Directory.CreateDirectory(options.SupportedControlsDir);
        if (Directory.Exists(options.UnSupportedControlsDir))
            Directory.Delete(options.UnSupportedControlsDir, true);
        Directory.CreateDirectory(options.UnSupportedControlsDir);

        // Create xaml files for controls
        foreach (var grouping in controlTypes)
        {
            var isBase = File.Exists(Path.Combine(options.BaseControlsDir, $"{grouping.Key[1..]}.axaml"));
            if (isBase)
                continue;

            var isDesign = File.Exists(Path.Combine(options.DesignControlsDir, $"{grouping.Key[1..]}.axaml"));
            if (isDesign)
                continue;

            var isAdvanced = File.Exists(Path.Combine(options.ControlsDir, $"{grouping.Key}.axaml"));
            if (isAdvanced)
            {
                CreateXamlFile(options.SupportedControlsDir, grouping, info => $"Please use the '{info.Impl.Name}'");
                continue;
            }

            var isNotXaml = File.Exists(Path.Combine(options.ControlsDir, $"{grouping.Key}.cs"));
            if (isNotXaml)
                continue;

            CreateXamlFile(options.UnSupportedControlsDir, grouping, info => $"The control '{info.TypeName}' is not supported");
        }
    }

    static void CreateXamlFile(string targetDir, IGrouping<string, ControlInfo> grouping, Func<ControlInfo, string> formatMessage)
    {
        foreach (var info in grouping)
        {
            var file = Path.Combine(targetDir, $"{info.TypeName}.axaml");

            using var writer = new StreamWriter(file, false);
            writer.WriteLine("<ResourceDictionary");
            writer.WriteLine("    xmlns=\"https://github.com/avaloniaui\"");
            writer.WriteLine("    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
            writer.WriteLine("    x:ClassModifier=\"internal\">");

            var sb = new StringBuilder();

            if (info.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Any(x => x.GetParameters().Length == 0))
                sb.AppendLine($"            <{info.TypeName} />");

            if (sb.Length == 0)
            {
                writer.WriteLine("    <!-- No need to preview controls -->");
            }
            else
            {
                writer.WriteLine("    <Design.PreviewWith>");
                writer.WriteLine("        <MPanel Mode=\"Vertical\" Spacing=\"12\" Margin=\"12\">");
                writer.Write(sb.ToString());
                writer.WriteLine("        </MPanel>");
                writer.WriteLine("    </Design.PreviewWith>");
            }

            if (typeof(TemplatedControl).IsAssignableFrom(info.Type))
            {
                writer.WriteLine($"    <ControlTheme x:Key=\"{{x:Type {info.TypeName}}}\" TargetType=\"{info.TypeName}\">");
                writer.WriteLine("        <Setter Property=\"Template\">");
                writer.WriteLine("            <ControlTemplate>");

                var templateParts = GetTemplateParts(info.Type).Where(x => x.IsRequired).ToList();

                if (templateParts.Count > 0)
                {
                    writer.WriteLine("                <Panel>");
                    writer.WriteLine($"                    <SelectableTextBlock Foreground=\"{{MBrush Destructive}}\" Text=\"{formatMessage(info)}\" />");

                    foreach (var templatePart in templateParts)
                        writer.WriteLine($"                    <{templatePart.Type.Name} Name=\"{templatePart.Name}\" IsVisible=\"False\" />");

                    writer.WriteLine("                </Panel>");
                }
                else
                {
                    writer.WriteLine($"                <SelectableTextBlock Foreground=\"{{MBrush Destructive}}\" Text=\"{formatMessage(info)}\" />");
                }

                writer.WriteLine("            </ControlTemplate>");
                writer.WriteLine("        </Setter>");
                writer.WriteLine("    </ControlTheme>");
            }
            else
            {
                var hasText = info.Type.GetProperty("Text") is not null;

                if (hasText is false)
                    writer.WriteLine($"    <!-- {formatMessage(info)} -->");
                writer.WriteLine($"    <ControlTheme x:Key=\"{{x:Type {info.TypeName}}}\" TargetType=\"{info.TypeName}\">");
                writer.WriteLine("        <Setter Property=\"Foreground\" Value=\"{MBrush Destructive}\" />");
                if (hasText)
                    writer.WriteLine($"        <Setter Property=\"Text\" Value=\"{formatMessage(info)}\" />");
                writer.WriteLine("    </ControlTheme>");
            }

            writer.WriteLine("</ResourceDictionary>");
        }
    }

    static IEnumerable<TemplatePartAttribute> GetTemplateParts(Type type)
    {
        var attributes = type.GetCustomAttributes(typeof(TemplatePartAttribute), true);
        foreach (var attribute in attributes)
            if (attribute is TemplatePartAttribute templatePartAttribute)
                yield return templatePartAttribute;
    }
}
