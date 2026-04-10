using System.Text;

namespace MantaPrepare;

sealed class DemosScanner(Options options)
{
    public void Prepare()
    {
        if (string.IsNullOrWhiteSpace(options.AutoDemosDir))
            return;

        Directory.Delete(options.AutoDemosDir, true);
        Directory.CreateDirectory(options.AutoDemosDir);

        var items = new List<string>();

        foreach (var (controlName, xamlLines, group) in ScanXamls())
        {
            var substring = controlName.StartsWith('M') ? controlName.Substring(1) : controlName;
            var viewName = $"{substring}View";

            items.Add($"{viewName}_Model");

            CreateXamlFile(Path.Combine(options.AutoDemosDir, $"{viewName}.axaml"), viewName, xamlLines);
            CreateXamlCodeFile(Path.Combine(options.AutoDemosDir, $"{viewName}.axaml.cs"), viewName);
            CreateViewModelCodeFile(Path.Combine(options.AutoDemosDir, $"{viewName}_Model.cs"), viewName, controlName, group);
        }

        CreateAutoDemosFile(Path.Combine(options.AutoDemosDir, "_AutoDemos.cs"), items);
    }

    static void CreateAutoDemosFile(string file, List<string> items)
    {
        using var writer = new StreamWriter(file, false);
        writer.WriteLine("using MantaDemo.Reactive;");
        writer.WriteLine();
        writer.WriteLine("namespace MantaDemo.Demos.AutoDemos;");
        writer.WriteLine();
        writer.WriteLine("static class AutoDemos");
        writer.WriteLine("{");
        writer.WriteLine("    public static IEnumerable<DemoViewModel> CreateDemos()");
        writer.WriteLine("    {");
        foreach (var item in items)
            writer.WriteLine($"        yield return new {item}();");
        writer.WriteLine("    }");
        writer.WriteLine("}");
    }

    static void CreateXamlFile(string file, string viewName, List<string> xamlLines)
    {
        using var writer = new StreamWriter(file, false);
        writer.WriteLine("<MView");
        writer.WriteLine($"    x:Class=\"MantaDemo.Demos.AutoDemos.{viewName}\"");
        writer.WriteLine("    xmlns=\"https://github.com/avaloniaui\"");
        writer.WriteLine("    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
        if (xamlLines.Any(x => x.Contains("<generic:")))
            writer.WriteLine("    xmlns:generic=\"clr-namespace:System.Collections.Generic;assembly=System.Collections\"");
        if (xamlLines.Any(x => x.Contains("<system:")))
            writer.WriteLine("    xmlns:system=\"clr-namespace:System;assembly=mscorlib\"");
        writer.WriteLine("    xmlns:local=\"clr-namespace:MantaDemo.Demos.AutoDemos\"");
        writer.WriteLine($"    x:DataType=\"local:{viewName}_Model\">");
        writer.WriteLine("    <Design.DataContext>");
        writer.WriteLine($"        <local:{viewName}_Model />");
        writer.WriteLine("    </Design.DataContext>");
        writer.WriteLine("    <Panel HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\">");

        foreach (var xamlLine in xamlLines)
            writer.WriteLine(xamlLine);

        writer.WriteLine("    </Panel>");
        writer.WriteLine("</MView>");
    }

    static void CreateXamlCodeFile(string file, string viewName)
    {
        using var writer = new StreamWriter(file, false);
        writer.WriteLine("using Manta.Controls;");
        writer.WriteLine();
        writer.WriteLine("namespace MantaDemo.Demos.AutoDemos;");
        writer.WriteLine();
        writer.WriteLine($"sealed partial class {viewName} : MView");
        writer.WriteLine("{");
        writer.WriteLine($"    public {viewName}()");
        writer.WriteLine("    {");
        writer.WriteLine("        InitializeComponent();");
        writer.WriteLine("    }");
        writer.WriteLine("}");
    }

    static void CreateViewModelCodeFile(string file, string viewName, string controlName, ControlGroup group)
    {
        using var writer = new StreamWriter(file, false);

        if (group is ControlGroup.Base)
            writer.WriteLine("using Avalonia.Controls;");

        writer.WriteLine("using Manta.Controls;");
        writer.WriteLine("using MantaDemo.Reactive;");
        writer.WriteLine();
        writer.WriteLine("namespace MantaDemo.Demos.AutoDemos;");
        writer.WriteLine();
        writer.WriteLine($"sealed class {viewName}_Model : DemoViewModel");
        writer.WriteLine("{");
        writer.WriteLine("    protected override MView CreateView()");
        writer.WriteLine("    {");
        writer.WriteLine($"        return new {viewName}();");
        writer.WriteLine("    }");
        writer.WriteLine();
        writer.WriteLine("    public override DemoGroup Group => DemoGroup.Controls;");
        writer.WriteLine($"    public override DemoSubGroup SubGroup => DemoSubGroup.{group};");
        writer.WriteLine($"    public override string Header => nameof({controlName});");
        writer.WriteLine("}");
    }

    enum ControlGroup
    {
        Advanced,
        Window,
        Base
    }

    IEnumerable<(string controlName, List<string> xamlLines, ControlGroup group)> ScanXamls()
    {
        foreach (var (controlName, xamlLines) in ScanXamls(options.ControlsDir))
            yield return (controlName, xamlLines, GetControlGroup(controlName));

        foreach (var (controlName, xamlLines) in ScanXamls(options.BaseControlsDir))
            yield return (controlName, xamlLines, ControlGroup.Base);
    }

    static ControlGroup GetControlGroup(string controlName)
    {
        if (controlName.Contains("Window"))
            return ControlGroup.Window;

        if (controlName.Contains("TitleBar"))
            return ControlGroup.Window;

        return ControlGroup.Advanced;
    }

    static IEnumerable<(string controlName, List<string> xamlLines)> ScanXamls(string controlsDir)
    {
        foreach (var file in Directory.GetFiles(controlsDir, "*.axaml"))
        {
            var controlName = Path.GetFileNameWithoutExtension(file);

            var lines = new List<string>();
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

                if (isStart is false)
                    continue;

                if (line.Trim().StartsWith("<!--"))
                    continue;

                lines.Add(line);
            }

            if (lines.Count > 0)
                yield return (controlName, lines);
        }
    }
}
