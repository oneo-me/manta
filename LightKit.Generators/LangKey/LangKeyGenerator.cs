using System.Collections.Immutable;
using System.Text;
using LightKit.Generators.LangKey.Globalization.Models;
using LightKit.Globalization.Core;
using Microsoft.CodeAnalysis;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace LightKit.Generators.LangKey;

sealed class LangKeyGenerator : GeneratorBase
{
    public override string Name => "LangKeyGenerator";
    public override int Version => 2;

    public override bool MatchFile(AdditionalText file)
    {
        if (file.Path.EndsWith(".yml", StringComparison.OrdinalIgnoreCase) is false)
            return false;

        var dirName = Path.GetFileName(Path.GetDirectoryName(file.Path));

        return string.Equals(dirName, "Langs", StringComparison.OrdinalIgnoreCase);
    }

    public override void Execute(SourceProductionContext context, Compilation compilation, ImmutableArray<AdditionalText> files)
    {
        var assemblyName = compilation.AssemblyName ?? throw new InvalidOperationException("Unable to retrieve the assembly name of the project");
        var filterFiles = FilterFiles(files);

        var config = ParserConfig(filterFiles);
        var data = ParserFiles(context, filterFiles);

        var baseData = data.TryGetValue(config.Base, out var bd) ? bd : throw new InvalidOperationException($"The base language file '{config.Base}.yml' is missing.");
        var commentData = data.TryGetValue(config.Comment, out var cd) ? cd : baseData;

        data.Remove(config.Base);

        CheckMissing(context, data, baseData);
        CheckRedundant(context, data, baseData);

        GenerateLangKey(context, config, assemblyName, baseData, commentData);
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
            return LangConfig.Load(yml);

        return new();
    }

    Dictionary<LangType, LangData> ParserFiles(SourceProductionContext context, Dictionary<string, AdditionalText> filterFiles)
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

    void ParserFile(SourceProductionContext context, LangData data)
    {
        var yml = data.File.GetText()?.ToString();
        if (yml is null)
        {
            WriteWarning(context, $"The file '{data.File.Path}' is empty.", data.Location);
            return;
        }

        var yamlParser = new Parser(new StringReader(yml));

        LangValue? item = null;

        while (yamlParser.MoveNext())
        {
            if (yamlParser.Current is not Scalar scalar)
                continue;

            if (item is null)
            {
                item = new()
                {
                    Key = scalar.Value,
                    KeyLocation = Location.Create(data.File.Path, new((int)scalar.Start.Index, (int)(scalar.End.Index - scalar.Start.Index)), new(new((int)scalar.Start.Line - 1, (int)scalar.Start.Column - 1), new((int)scalar.End.Line - 1, (int)scalar.End.Column - 1)))
                };
            }
            else
            {
                item.Value = scalar.Value;

                data.Values.Add(item.Key, item);
                item = null;
            }
        }
    }

    void CheckMissing(SourceProductionContext context, Dictionary<LangType, LangData> data, LangData baseData)
    {
        foreach (var dataValue in data.Values)
        foreach (var baseDataValue in baseData.Values)
            if (dataValue.Values.ContainsKey(baseDataValue.Key) is false)
                WriteWarning(context, $"Not found [{baseDataValue.Key}] in the language file.", dataValue.Location);
    }

    void CheckRedundant(SourceProductionContext context, Dictionary<LangType, LangData> data, LangData baseData)
    {
        foreach (var dataValue in data.Values)
        foreach (var dataItem in dataValue.Values)
            if (baseData.Values.ContainsKey(dataItem.Key) is false)
                WriteWarning(context, $"The key [{dataItem.Key}] is redundant in the language file.", dataItem.Value.KeyLocation);
    }

    void GenerateLangKey(SourceProductionContext context, LangConfig config, string assemblyName, LangData baseData, LangData commentData)
    {
        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated />");
        sb.AppendLine("//");
        sb.AppendLine($"// The list is generated based on {config.Base}.yml. ");
        sb.AppendLine($"// Generator version: {Version}");
        sb.AppendLine();
        sb.AppendLine("using System.Reflection;");
        sb.AppendLine();
        sb.AppendLine($"namespace {assemblyName}.Langs;");
        sb.AppendLine();

        if (string.Equals(config.Modifier, "private", StringComparison.OrdinalIgnoreCase))
            sb.AppendLine($"static class {config.ClassName}");
        else
            sb.AppendLine($"{config.Modifier} static class {config.ClassName}");

        sb.AppendLine("{");

        foreach (var baseValue in baseData.Values)
        {
            sb.AppendLine("    /// <summary>");
            if (commentData.Values.TryGetValue(baseValue.Key, out var comment))
                sb.AppendLine($"    ///     {comment.Value}");
            else
                sb.AppendLine($"    ///     {baseValue.Value}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public const string {baseValue.Key} = \"{assemblyName}.{baseValue.Key}\";");
            sb.AppendLine();
        }

        sb.AppendLine("}");

        context.AddSource($"{config.ClassName}.g.cs", sb.ToString());
    }
}
