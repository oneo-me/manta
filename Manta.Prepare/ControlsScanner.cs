using System.Text;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Remote;
using Avalonia.Controls.Shapes;
using Avalonia.Dialogs;
using Avalonia.Dialogs.Internal;
using BindingFlags = System.Reflection.BindingFlags;
using Path = System.IO.Path;

namespace Manta.Prepare;

sealed class ControlsScanner(Options options)
{
    public void Prepare()
    {
        var assemblys = new[] { typeof(Button).Assembly, typeof(ManagedFileChooser).Assembly };
        var controlTypes = assemblys
            .Distinct()
            .SelectMany(x => x.GetTypes().Where(t => t is { IsPublic: true, IsAbstract: false } && typeof(Control).IsAssignableFrom(t)))
            .Select(x => new ControlInfo(x))
            .Where(x => string.IsNullOrWhiteSpace(x.GroupName) is false)
            .GroupBy(x => x.GroupName)
            .OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
            .ToList();

        // Check control support level
        foreach (var grouping in controlTypes)
        {
            var supportLevel = ControlSupportLevel.Unsupported;

            if (File.Exists(Path.Combine(options.BaseControlsDir, $"{grouping.Key}.axaml")))
                supportLevel = ControlSupportLevel.Basic;
            else if (File.Exists(Path.Combine(options.ControlsDir, $"M{grouping.Key}.cs")))
                supportLevel = File.Exists(Path.Combine(options.ControlsDir, $"M{grouping.Key}.axaml")) ? ControlSupportLevel.Advanced : ControlSupportLevel.AdvancedNoXaml;
            else if (File.Exists(Path.Combine(options.ControlsDir, $"{grouping.Key}.cs")))
                supportLevel = File.Exists(Path.Combine(options.ControlsDir, $"{grouping.Key}.axaml")) ? ControlSupportLevel.Advanced : ControlSupportLevel.AdvancedNoXaml;

            foreach (var controlInfo in grouping)
                controlInfo.SupportLevel = supportLevel;
        }

        // Clear directory
        if (Directory.Exists(options.SupportedControlsDir))
            Directory.Delete(options.SupportedControlsDir, true);
        Directory.CreateDirectory(options.SupportedControlsDir);
        if (Directory.Exists(options.UnSupportedControlsDir))
            Directory.Delete(options.UnSupportedControlsDir, true);
        Directory.CreateDirectory(options.UnSupportedControlsDir);

        // Create xaml files for controls
        foreach (var grouping in controlTypes)
            switch (grouping.First().SupportLevel)
            {
                case ControlSupportLevel.Advanced:
                    CreateXamlFile(options.SupportedControlsDir, grouping, info => $"Please use the '{(info.ImplName.StartsWith('M') ? info.ImplName : $"M{info.ImplName}")}'");
                    break;

                case ControlSupportLevel.AdvancedNoXaml:
                    break;

                case ControlSupportLevel.Unsupported:
                    CreateXamlFile(options.UnSupportedControlsDir, grouping, info => $"The control '{info.ImplName}' is not supported");
                    break;

                case ControlSupportLevel.Basic:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
    }

    static void CreateXamlFile(string targetDir, IGrouping<string, ControlInfo> grouping, Func<ControlInfo, string> formatMessage)
    {
        var file = Path.Combine(targetDir, $"{(grouping.Key.StartsWith('M') ? grouping.First().ContorlType.Name : grouping.Key)}.axaml");
        using var writer = new StreamWriter(file, false);
        writer.WriteLine("<ResourceDictionary");
        writer.WriteLine("    xmlns=\"https://github.com/avaloniaui\"");
        writer.WriteLine("    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
        writer.WriteLine("    x:ClassModifier=\"internal\">");

        var sb = new StringBuilder();

        foreach (var info in grouping)
            if (info.ContorlType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Any(x => x.GetParameters().Length == 0))
                sb.AppendLine($"            <{info.ContorlType.Name} />");

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

        foreach (var info in grouping)
            if (typeof(TemplatedControl).IsAssignableFrom(info.ContorlType))
            {
                writer.WriteLine($"    <ControlTheme x:Key=\"{{x:Type {info.ContorlType.Name}}}\" TargetType=\"{info.ContorlType.Name}\">");
                writer.WriteLine("        <Setter Property=\"Template\">");
                writer.WriteLine("            <ControlTemplate>");

                var templateParts = GetTemplateParts(info.ContorlType).Where(x => x.IsRequired).ToList();

                if (templateParts.Count > 0)
                {
                    writer.WriteLine("                <Panel>");
                    writer.WriteLine($"                    <SelectableTextBlock Foreground=\"{{MBrush Danger}}\" Text=\"{formatMessage(info)}\" />");

                    foreach (var templatePart in templateParts)
                        writer.WriteLine($"                    <{templatePart.Type.Name} Name=\"{templatePart.Name}\" IsVisible=\"False\" />");

                    writer.WriteLine("                </Panel>");
                }
                else
                {
                    writer.WriteLine($"                <SelectableTextBlock Foreground=\"{{MBrush Danger}}\" Text=\"{formatMessage(info)}\" />");
                }

                writer.WriteLine("            </ControlTemplate>");
                writer.WriteLine("        </Setter>");
                writer.WriteLine("    </ControlTheme>");
            }
            else
            {
                var hasText = info.ContorlType.GetProperty("Text") is not null;

                if (hasText is false)
                    writer.WriteLine($"    <!-- {formatMessage(info)} -->");
                writer.WriteLine($"    <ControlTheme x:Key=\"{{x:Type {info.ContorlType.Name}}}\" TargetType=\"{info.ContorlType.Name}\">");
                writer.WriteLine("        <Setter Property=\"Foreground\" Value=\"{MBrush Danger}\" />");
                if (hasText)
                    writer.WriteLine($"        <Setter Property=\"Text\" Value=\"{formatMessage(info)}\" />");
                writer.WriteLine("    </ControlTheme>");
            }

        writer.WriteLine("</ResourceDictionary>");
    }

    static IEnumerable<TemplatePartAttribute> GetTemplateParts(Type type)
    {
        var attributes = type.GetCustomAttributes(typeof(TemplatePartAttribute), true);
        foreach (var attribute in attributes)
            if (attribute is TemplatePartAttribute templatePartAttribute)
                yield return templatePartAttribute;
    }

    enum ControlSupportLevel
    {
        Unsupported,
        Basic,
        Advanced,
        AdvancedNoXaml
    }

    sealed class ControlInfo
    {
        public Type ContorlType { get; }
        public string GroupName { get; } = string.Empty;
        public string ImplName { get; } = string.Empty;
        public ControlSupportLevel SupportLevel { get; set; } = ControlSupportLevel.Unsupported;

        static readonly Type[] _excludeTypes =
        [
            typeof(ScrollContentPresenter),
            typeof(ContentPresenter),
            typeof(ItemsPresenter),

            typeof(Decorator),
            typeof(Control),
            typeof(TemplatedControl),
            typeof(UserControl),
            typeof(HeaderedItemsControl),
            typeof(SelectingItemsControl),
            typeof(HeaderedSelectingItemsControl),
            typeof(HeaderedContentControl),
            typeof(ContentControl),
            typeof(LayoutTransformControl),
            typeof(ExperimentalAcrylicBorder),

            typeof(DateTimePickerPanel),
            typeof(NativeControlHost),
            typeof(Popup),
            typeof(TickBar),
            typeof(Track),
            typeof(Canvas),
            typeof(Image),
            typeof(Viewbox),
            typeof(AdornerLayer),
            typeof(OverlayLayer),
            typeof(ChromeOverlayLayer),
            typeof(LightDismissOverlayLayer),
            typeof(OverlayLayer),
            typeof(TextSelectorLayer),
            typeof(TextSelectionHandle),
            typeof(TextPresenter),
            typeof(VisualLayerManager),

            typeof(Grid),
            typeof(UniformGrid),
            typeof(WrapPanel),
            typeof(StackPanel),
            typeof(RelativePanel),
            typeof(DockPanel),
            typeof(VirtualizingCarouselPanel),
            typeof(VirtualizingStackPanel),
            typeof(ReversibleStackPanel),

            typeof(Arc),
            typeof(Ellipse),
            typeof(Line),
            typeof(Avalonia.Controls.Shapes.Path),
            typeof(Polygon),
            typeof(Polyline),
            typeof(Rectangle),
            typeof(Sector),

            typeof(Window),
            typeof(WindowBase),
            typeof(AboutAvaloniaDialog),

            // Other platform specific controls
            typeof(RefreshContainer),
            typeof(RefreshVisualizer),
            typeof(RemoteWidget),
            typeof(ChildFitter),
            typeof(ManagedFileChooser),
            typeof(ManagedFileChooserOverwritePrompt)
        ];

        public ControlInfo(Type type)
        {
            ContorlType = type;

            if (_excludeTypes.Contains(ContorlType))
                return;

            ImplName = type.Name switch
            {
                nameof(SelectableTextBlock) => "SelectableTextBlock",
                nameof(TextBlock)           => "TextBlock",
                nameof(UserControl)         => "MView",
                nameof(ItemsControl)        => "MItems",
                nameof(CaptionButtons)      => "MTitleBarButtons",

                _ => type.Name
            };
            GroupName = ImplName switch
            {
                "MTitleBarButtons"        => nameof(TitleBar),
                nameof(OverlayPopupHost)  => nameof(PopupRoot),
                nameof(CalendarButton)    => nameof(Calendar),
                nameof(CalendarDayButton) => nameof(Calendar),
                nameof(CalendarItem)      => nameof(Calendar),
                nameof(ListBoxItem)       => nameof(ListBox),
                nameof(ComboBoxItem)      => nameof(ComboBox),
                nameof(MenuItem)          => nameof(Menu),
                nameof(TabStripItem)      => nameof(TabStrip),
                nameof(TabItem)           => nameof(TabControl),
                nameof(TreeViewItem)      => nameof(TreeView),
                nameof(ScrollBar)         => nameof(ScrollViewer),

                _ => ImplName
            };
        }
    }
}
