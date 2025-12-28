using System.Text;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Manta.Controls;
using Manta.Reactive.Services;
using Manta.Reactive.Views;
using Manta.Demo.Reactive;

namespace Manta.Demo.Services;

sealed class DemoService
{
    readonly List<DemoViewModel> _demos = [];

    public DemoService()
    {
        foreach (var type in GetType().Assembly.GetTypes().Where(x => typeof(DemoViewModel).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false } && x != typeof(XamlDemo)))
        {
            var demoViewModel = Service.CreateInstance<DemoViewModel>(type);
            _demos.Add(demoViewModel);
        }

        var projectDir = Path.Combine(Path.GetFullPath(Path.Combine(LightApp.Folder, "../../../..")), "Manta");
        var controlsDir = Path.Combine(projectDir, "Controls");
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

    [View<LightView>]
    sealed class XamlDemo : DemoViewModel
    {
        public override DemoSubGroup SubGroup { get; }

        readonly string _xaml;

        public XamlDemo(string header, string xaml, DemoSubGroup? subGroup)
        {
            Header = header;

            if (subGroup is null)
            {
                foreach (var chekcGroup in Enum.GetValues<DemoSubGroup>())
                    if (xaml.Contains($"<!-- {chekcGroup} -->"))
                    {
                        SubGroup = chekcGroup;
                        break;
                    }
            }
            else
            {
                SubGroup = subGroup.Value;
            }

            if (SubGroup == DemoSubGroup.None)
                SubGroup = DemoSubGroup.Other;

            var sb = new StringBuilder();
            sb.AppendLine("<LightView");
            sb.AppendLine("    xmlns=\"https://github.com/avaloniaui\"");
            sb.AppendLine("    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
            sb.AppendLine("    xmlns:asyncImageLoader=\"clr-namespace:AsyncImageLoader;assembly=AsyncImageLoader.Avalonia\"");
            sb.AppendLine("    xmlns:generic=\"clr-namespace:System.Collections.Generic;assembly=System.Collections\"");
            sb.AppendLine("    xmlns:system=\"clr-namespace:System;assembly=System.Runtime\">");
            sb.AppendLine(xaml);
            sb.AppendLine("</LightView>");

            _xaml = sb.ToString();
        }

        public override DemoGroup Group => DemoGroup.Controls;

        public override string Header { get; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var contentControl = new ContentControl
            {
                Content = AvaloniaRuntimeXamlLoader.Load(_xaml, typeof(LightApp).Assembly),
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
