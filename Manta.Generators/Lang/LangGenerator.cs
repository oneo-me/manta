using System.Collections.Immutable;
using System.Text;
using Manta.Generators.Lang.Helpers;
using Manta.Generators.Lang.Models;
using Manta.Globalization.Core;
using Microsoft.CodeAnalysis;

namespace Manta.Generators.Lang;

[Generator]
public sealed class LangGenerator : IIncrementalGenerator
{
    const string Name = "LangGenerator";
    const int Version = 2;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var langFiles = context.AdditionalTextsProvider.Where(MatchFile);
        var pipeline = context.CompilationProvider.Combine(langFiles.Collect());

        context.RegisterSourceOutput(pipeline, (spc, source) =>
        {
            var (compilation, files) = source;

            if (files.Length <= 0)
                return;

            try
            {
                Execute(spc, compilation, files);
            }
            catch (Exception e)
            {
                ReportDiagnostic(spc, 1, e.ToString(), DiagnosticSeverity.Error);
            }
        });
    }

    static bool MatchFile(AdditionalText file)
    {
        if (file.Path.EndsWith(".yml", StringComparison.OrdinalIgnoreCase) is false)
            return false;

        var dirName = Path.GetFileName(Path.GetDirectoryName(file.Path));

        return string.Equals(dirName, "Langs", StringComparison.OrdinalIgnoreCase);
    }

    static void Execute(SourceProductionContext context, Compilation compilation, ImmutableArray<AdditionalText> files)
    {
        var assemblyName = compilation.AssemblyName ?? throw new InvalidOperationException("Unable to retrieve the assembly name of the project");
        var filterFiles = FilterFiles(files);

        var config = ParserConfig(filterFiles);
        var data = ParserFiles(context, filterFiles);

        var baseData = data.TryGetValue(config.Base, out var bd) ? bd : throw new InvalidOperationException($"The base language file '{config.Base}.yml' is missing.");
        var commentData = data.TryGetValue(config.Comment, out var cd) ? cd : baseData;
        var allData = new Dictionary<LangType, LangData>(data);

        data.Remove(config.Base);

        CheckMissing(context, data, baseData);
        CheckRedundant(context, data, baseData);

        GenerateLangKey(context, config, assemblyName, baseData, commentData);
        GenerateLangData(context, config, assemblyName, allData);
    }

    static Dictionary<string, AdditionalText> FilterFiles(ImmutableArray<AdditionalText> files)
    {
        var dic = new Dictionary<string, AdditionalText>();

        foreach (var file in files)
        {
            var dirName = Path.GetFileName(Path.GetDirectoryName(file.Path) ?? string.Empty);
            if (dirName != "Langs")
                continue;

            var extName = Path.GetExtension(file.Path);
            if (string.Equals(extName, ".yml", StringComparison.OrdinalIgnoreCase) is false && string.Equals(extName, ".yaml", StringComparison.OrdinalIgnoreCase) is false)
                continue;

            var fileName = Path.GetFileNameWithoutExtension(file.Path);
            dic.Add(fileName, file);
        }

        return dic;
    }

    static LangConfig ParserConfig(Dictionary<string, AdditionalText> filterFiles)
    {
        if (filterFiles.TryGetValue("@config", out var configText) is false)
            return new();

        var yml = configText.GetText()?.ToString();
        if (yml is not null)
            try
            {
                return LangYamlParser.LoadConfig(yml);
            }
            catch
            {
                // ignore invalid config, use defaults
            }

        return new();
    }

    static Dictionary<LangType, LangData> ParserFiles(SourceProductionContext context, Dictionary<string, AdditionalText> filterFiles)
    {
        var data = new Dictionary<LangType, LangData>();

        foreach (var langType in Enum.GetValues(typeof(LangType)).Cast<LangType>())
            if (filterFiles.TryGetValue($"{langType}", out var file))
            {
                var langData = new LangData(file);
                ParserFile(context, langData);
                data.Add(langType, langData);
            }

        return data;
    }

    static void ParserFile(SourceProductionContext context, LangData data)
    {
        var yml = data.File.GetText()?.ToString();
        if (yml is null)
        {
            ReportDiagnostic(context, 2, $"The file '{data.File.Path}' is empty.", DiagnosticSeverity.Warning, data.Location);
            return;
        }

        try
        {
            foreach (var item in LangYamlParser.LoadData(yml))
                data.Values[item.Key] = new() { Key = item.Key, Value = item.Value, KeyLocation = data.Location };
        }
        catch (Exception ex)
        {
            ReportDiagnostic(context, 1, $"Failed to parse '{data.File.Path}': {ex.Message}", DiagnosticSeverity.Error, data.Location);
        }
    }

    static void CheckMissing(SourceProductionContext context, Dictionary<LangType, LangData> data, LangData baseData)
    {
        foreach (var dataValue in data.Values)
        foreach (var baseDataValue in baseData.Values)
            if (dataValue.Values.ContainsKey(baseDataValue.Key) is false)
                ReportDiagnostic(context, 2, $"Not found [{baseDataValue.Key}] in the language file.", DiagnosticSeverity.Warning, dataValue.Location);
    }

    static void CheckRedundant(SourceProductionContext context, Dictionary<LangType, LangData> data, LangData baseData)
    {
        foreach (var dataValue in data.Values)
        foreach (var dataItem in dataValue.Values)
            if (baseData.Values.ContainsKey(dataItem.Key) is false)
                ReportDiagnostic(context, 2, $"The key [{dataItem.Key}] is redundant in the language file.", DiagnosticSeverity.Warning, dataItem.Value.KeyLocation);
    }

    static void ReportDiagnostic(SourceProductionContext spc, int id, object message, DiagnosticSeverity severity, Location? location = null)
    {
        var diagnostic = Diagnostic.Create(new($"{Name}_{id:D3}", Name, "{0}", Name, severity, true), location ?? Location.None, message);
        spc.ReportDiagnostic(diagnostic);
    }

    static void GenerateLangKey(SourceProductionContext context, LangConfig config, string assemblyName, LangData baseData, LangData commentData)
    {
        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated />");
        sb.AppendLine("#nullable enable");
        sb.AppendLine("//");
        sb.AppendLine($"// The list is generated based on {config.Base}.yml. ");
        sb.AppendLine($"// Generator version: {Version}");
        sb.AppendLine();
        sb.AppendLine($"namespace {assemblyName}.Langs;");
        sb.AppendLine();

        if (string.Equals(config.Modifier, "private", StringComparison.OrdinalIgnoreCase))
            sb.AppendLine($"static class {config.LangKeyClassName}");
        else
            sb.AppendLine($"{config.Modifier} static class {config.LangKeyClassName}");

        sb.AppendLine("{");

        var replace = assemblyName.Replace(".", "_");

        foreach (var baseValue in baseData.Values.OrderBy(item => item.Key, StringComparer.Ordinal))
        {
            sb.AppendLine("    /// <summary>");
            if (commentData.Values.TryGetValue(baseValue.Key, out var comment))
                sb.AppendLine($"    ///     {comment.Value}");
            else
                sb.AppendLine($"    ///     {baseValue.Value}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public const string {baseValue.Key} = \"{replace}_{baseValue.Key}\";");
            sb.AppendLine();
        }

        sb.AppendLine("}");

        context.AddSource($"{config.LangKeyClassName}.g.cs", sb.ToString());
    }

    static void GenerateLangData(SourceProductionContext context, LangConfig config, string assemblyName, Dictionary<LangType, LangData> data)
    {
        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated />");
        sb.AppendLine("#nullable enable");
        sb.AppendLine("//");
        sb.AppendLine($"// The list is generated based on {config.Base}.yml. ");
        sb.AppendLine($"// Generator version: {Version}");
        sb.AppendLine();
        sb.AppendLine($"namespace {assemblyName}.Langs;");
        sb.AppendLine();

        if (string.Equals(config.Modifier, "private", StringComparison.OrdinalIgnoreCase))
            sb.AppendLine($"static class {config.LangDataClassName}");
        else
            sb.AppendLine($"{config.Modifier} static class {config.LangDataClassName}");
        sb.AppendLine("{");
        sb.AppendLine("    static readonly global::Manta.Globalization.LangDataBase _instance = new GeneratedLangData();");
        sb.AppendLine();
        sb.AppendLine("    public static global::Manta.Globalization.LangDataBase Load()");
        sb.AppendLine("    {");
        sb.AppendLine("        return _instance;");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    sealed class GeneratedLangData : global::Manta.Globalization.LangDataBase");
        sb.AppendLine("    {");
        sb.AppendLine($"        public GeneratedLangData() : base(global::Manta.Globalization.Core.LangType.{config.Base})");
        sb.AppendLine("        {");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine("        protected override string? GetOverride(global::Manta.Globalization.Core.LangType langType, string key)");
        sb.AppendLine("        {");
        sb.AppendLine("            return langType switch");
        sb.AppendLine("            {");

        foreach (var item in data.OrderBy(entry => entry.Key.ToString(), StringComparer.Ordinal))
            sb.AppendLine($"                global::Manta.Globalization.Core.LangType.{item.Key} => Get{item.Key}(key),");

        sb.AppendLine("                _ => null");
        sb.AppendLine("            };");
        sb.AppendLine("        }");
        sb.AppendLine();

        var keyPrefix = assemblyName.Replace(".", "_");

        foreach (var item in data.OrderBy(entry => entry.Key.ToString(), StringComparer.Ordinal))
        {
            sb.AppendLine($"        static string? Get{item.Key}(string key)");
            sb.AppendLine("        {");
            sb.AppendLine("            return key switch");
            sb.AppendLine("            {");

            foreach (var value in item.Value.Values.OrderBy(entry => entry.Key, StringComparer.Ordinal))
                sb.AppendLine($"                \"{keyPrefix}_{value.Key}\" => {ToLiteral(value.Value.Value)},");

            sb.AppendLine("                _ => null");
            sb.AppendLine("            };");
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");

        context.AddSource($"{config.LangDataClassName}.g.cs", sb.ToString());
    }

    static string ToLiteral(string value)
    {
        var sb = new StringBuilder(value.Length + 2);
        sb.Append('"');

        foreach (var c in value)
            sb.Append(c switch
            {
                '\\' => @"\\",
                '"'  => "\\\"",
                '\r' => "\\r",
                '\n' => "\\n",
                '\t' => "\\t",
                _    => c.ToString()
            });

        sb.Append('"');
        return sb.ToString();
    }
}
