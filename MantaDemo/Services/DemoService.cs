using System.Text;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Manta;
using Manta.Controls;
using Manta.Reactive.Services;
using Manta.Reactive.Views;
using MantaDemo.Reactive;

namespace MantaDemo.Services;

sealed class DemoService
{
    readonly List<DemoViewModel> _demos = [];

    static IEnumerable<string> GetCodeFiles(string dir, params string[] extensions)
    {
        foreach (var subDir in Directory.EnumerateDirectories(dir))
        {
            var dirName = Path.GetFileName(subDir);
            if (dirName == "bin" || dirName == "obj" || dirName.StartsWith('.'))
                continue;

            foreach (var codeFile in GetCodeFiles(subDir, extensions))
                yield return codeFile;
        }

        var hashSet = new HashSet<string>(extensions, StringComparer.OrdinalIgnoreCase);

        foreach (var file in Directory.GetFiles(dir))
            if (hashSet.Contains(Path.GetExtension(file)))
                yield return file;
    }

    readonly string _projectDir;

    public DemoService()
    {
        _projectDir = Path.GetFullPath(Path.Combine(MApp.Folder, "../../../.."));

        // Register demos
        foreach (var type in GetType().Assembly.GetTypes().Where(x => typeof(DemoViewModel).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false } && x != typeof(XamlDemo)))
        {
            var demoViewModel = Service.CreateInstance<DemoViewModel>(type);
            _demos.Add(demoViewModel);
        }

        // Scan XAML files for demos
        var mantaProjectDir = Path.Combine(_projectDir, "Manta");
        var controlsDir = Path.Combine(mantaProjectDir, "Controls");
        var baseControlsDir = Path.Combine(controlsDir, "Base");

        ScanXamls(controlsDir, null);
        ScanXamls(baseControlsDir, DemoSubGroup.Base);

        _demos.Sort((a, b) =>
        {
            var subGroupComparison = a.SubGroup.CompareTo(b.SubGroup);
            if (subGroupComparison != 0)
                return subGroupComparison;

            var groupComparison = a.Group.CompareTo(b.Group);
            if (groupComparison != 0)
                return groupComparison;

            return string.Compare(a.Header, b.Header, StringComparison.Ordinal);
        });
    }

    public int GetCodeLines()
    {
        var codeFiles = GetCodeFiles(_projectDir, ".cs", ".axaml", ".swift");
        var codeLines = 0;
        foreach (var codeFile in codeFiles)
        {
            var lines = File.ReadLines(codeFile);
            codeLines += lines.Count(line => !string.IsNullOrWhiteSpace(line));
        }

        return codeLines;
    }

    void ScanXamls(string controlsDir, DemoSubGroup? subGroup)
    {
        foreach (var file in Directory.GetFiles(controlsDir, "*.axaml"))
        {
            var header = Path.GetFileNameWithoutExtension(file);
            if (_demos.Any(x => string.Equals(x.Header, header, StringComparison.Ordinal)))
                continue;

            var sb = new StringBuilder();
            var isStart = false;

            foreach (var line in File.ReadLines(file, Encoding.UTF8))
            {
                if (line.Contains("<Design.PreviewWith>"))
                {
                    isStart = true;
                    continue;
                }

                if (line.Contains("</Design.PreviewWith>"))
                    break;

                if (isStart)
                    sb.AppendLine(line);
            }

            if (sb.Length > 0)
            {
                var xaml = sb.ToString();
                _demos.Add(new XamlDemo(header, xaml, subGroup));
            }
        }
    }

    [View<MView>]
    sealed class XamlDemo : DemoViewModel
    {
        public override DemoSubGroup SubGroup { get; }

        readonly string _xaml;

        public XamlDemo(string header, string xaml, DemoSubGroup? subGroup)
        {
            Header = header;

            if (subGroup is null)
            {
                foreach (var checkGroup in Enum.GetValues<DemoSubGroup>())
                    if (xaml.Contains($"<!-- {checkGroup} -->"))
                    {
                        SubGroup = checkGroup;
                        break;
                    }
            }
            else
            {
                SubGroup = subGroup.Value;
            }

            if (SubGroup == DemoSubGroup.None)
                SubGroup = DemoSubGroup.Custom;

            var sb = new StringBuilder();
            sb.AppendLine("<MView");
            sb.AppendLine("    xmlns=\"https://github.com/avaloniaui\"");
            sb.AppendLine("    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
            sb.AppendLine("    xmlns:asyncImageLoader=\"clr-namespace:AsyncImageLoader;assembly=AsyncImageLoader.Avalonia\"");
            sb.AppendLine("    xmlns:generic=\"clr-namespace:System.Collections.Generic;assembly=System.Collections\"");
            sb.AppendLine("    xmlns:system=\"clr-namespace:System;assembly=System.Runtime\">");
            sb.AppendLine(xaml);
            sb.AppendLine("</MView>");

            _xaml = sb.ToString();
        }

        public override DemoGroup Group => DemoGroup.Controls;

        public override string Header { get; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var contentControl = new ContentControl
            {
                Content = AvaloniaRuntimeXamlLoader.Load(_xaml, typeof(MApp).Assembly),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            this.GetView().Content = contentControl;
        }
    }

    public IReadOnlyCollection<DemoViewModel> GetDemos(DemoGroup group)
    {
        return _demos.Where(x => x.Group == group).ToArray();
    }
}
