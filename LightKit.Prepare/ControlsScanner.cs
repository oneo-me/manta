using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Remote;
using Avalonia.Controls.Shapes;
using Avalonia.Dialogs;
using Avalonia.Dialogs.Internal;
using Path = System.IO.Path;

namespace LightKit.Prepare;

sealed class ControlsScanner(Options options)
{
    public void Prepare()
    {
        var assemblys = new[] { typeof(Button).Assembly, typeof(ManagedFileChooser).Assembly };
        var controlTypes = assemblys
            .SelectMany(x => x.GetTypes().Where(t => t is { IsPublic: true, IsAbstract: false } && typeof(Control).IsAssignableFrom(t)))
            .Select(x => new ControlInfo(x))
            .Where(x => string.IsNullOrWhiteSpace(x.ImplName) is false)
            .GroupBy(x => x.ImplName)
            .OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
            .ToList();

        // Check control support level
        foreach (var grouping in controlTypes)
        {
            var supportLevel = ControlSupportLevel.Unsupported;

            if (File.Exists(Path.Combine(options.BaseControlsDir, $"{grouping.Key}.axaml")))
                supportLevel = ControlSupportLevel.Basic;
            else if (File.Exists(Path.Combine(options.ControlsDir, $"Light{grouping.Key}.axaml")))
                supportLevel = ControlSupportLevel.Advanced;
            else if (File.Exists(Path.Combine(options.ControlsDir, $"{grouping.Key}.axaml")))
                supportLevel = ControlSupportLevel.Advanced;

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
                    CreateXamlFile(options.SupportedControlsDir, grouping, type => $"Please use the 'Light{type.Name}'");
                    break;

                case ControlSupportLevel.Unsupported:
                    CreateXamlFile(options.UnSupportedControlsDir, grouping, type => $"The control '{type.Name}' is not supported");
                    break;

                case ControlSupportLevel.Basic:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
    }

    static void CreateXamlFile(string targetDir, IGrouping<string, ControlInfo> grouping, Func<Type, string> formatMessage)
    {
        var file = Path.Combine(targetDir, $"{grouping.Key}.axaml");
        using var writer = new StreamWriter(file, false);
        writer.WriteLine("<ResourceDictionary");
        writer.WriteLine("    xmlns=\"https://github.com/avaloniaui\"");
        writer.WriteLine("    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
        writer.WriteLine("    x:ClassModifier=\"internal\">");
        writer.WriteLine("    <Design.PreviewWith>");
        writer.WriteLine("        <LightPanel Mode=\"Vertical\" Spacing=\"12\" Margin=\"12\">");

        foreach (var info in grouping)
            writer.WriteLine($"            <{info.ContorlType.Name} />");

        writer.WriteLine("        </LightPanel>");
        writer.WriteLine("    </Design.PreviewWith>");

        foreach (var info in grouping)
        {
            writer.WriteLine($"    <ControlTheme x:Key=\"{{x:Type {info.ContorlType.Name}}}\" TargetType=\"{info.ContorlType.Name}\">");

            if (typeof(TemplatedControl).IsAssignableFrom(info.ContorlType))
            {
                writer.WriteLine("        <Setter Property=\"Template\">");
                writer.WriteLine("            <ControlTemplate>");

                var templateParts = GetTemplateParts(info.ContorlType).ToList();

                if (templateParts.Count > 0)
                {
                    writer.WriteLine("                <Panel>");
                    writer.WriteLine($"                    <LightSelectableText Theme.ColorScheme=\"Danger\" Text=\"{formatMessage(info.ContorlType)}\" />");

                    foreach (var templatePart in templateParts)
                        if (templatePart.IsRequired)
                            writer.WriteLine($"                    <{templatePart.Type.Name} x:Name=\"{templatePart.Name}\" IsVisible=\"False\" />");

                    writer.WriteLine("                </Panel>");
                }
                else
                {
                    writer.WriteLine($"                <LightSelectableText Theme.ColorScheme=\"Danger\" Text=\"{formatMessage(info.ContorlType)}\" />");
                }

                writer.WriteLine("            </ControlTemplate>");
                writer.WriteLine("        </Setter>");
            }

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
        Advanced
    }

    sealed class ControlInfo
    {
        public Type ContorlType { get; }
        public string ImplName { get; } = string.Empty;
        public ControlSupportLevel SupportLevel { get; set; } = ControlSupportLevel.Unsupported;

        static readonly Type[] _excludeTypes =
        [
            typeof(ContentPresenter),
            typeof(ItemsPresenter),

            typeof(HeaderedContentControl),
            typeof(ContentControl),
            typeof(Viewbox),
            typeof(TextSelectionHandle),
            typeof(TextPresenter),
            typeof(VisualLayerManager),
            typeof(ThemeVariantScope),

            // Temporarily not considering support
            typeof(RemoteWidget),
            typeof(ChildFitter),
            typeof(ManagedFileChooser),
            typeof(ManagedFileChooserOverwritePrompt),
            typeof(ManagedFileChooserOverwritePrompt)
        ];

        static readonly Type[] _excludeBaseTypes =
        [
            typeof(Panel),
            typeof(Shape),
            typeof(Image),
            typeof(WindowBase)
        ];

        public ControlInfo(Type type)
        {
            ContorlType = type;

            if (_excludeTypes.Contains(ContorlType) || _excludeBaseTypes.Any(x => x.IsAssignableFrom(ContorlType)))
                return;

            ImplName = type.Name switch
            {
                // Rename controls
                nameof(SelectableTextBlock) => "LightSelectableText",
                nameof(TextBlock)           => "LightText",
                nameof(UserControl)         => "LightView",

                // Merge controls
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

                _ => type.Name
            };
        }
    }
}
