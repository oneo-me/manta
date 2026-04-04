using YamlDotNet.RepresentationModel;

namespace Manta.Globalization.Core;

static class YamlExtensions
{
    static void WalkMapping(YamlMappingNode node, string prefix, Dictionary<string, string> output)
    {
        foreach (var kv in node.Children)
        {
            var key = (kv.Key as YamlScalarNode)?.Value ?? "";
            if (string.IsNullOrEmpty(key))
                continue;

            var path = string.IsNullOrEmpty(prefix) ? key : $"{prefix}.{key}";

            switch (kv.Value)
            {
                case YamlScalarNode scalar:
                    output[path] = scalar.Value ?? "";
                    break;

                case YamlMappingNode map:
                    WalkMapping(map, path, output);
                    break;

                case YamlSequenceNode seq:
                    output[path] = string.Join(",", seq.Children
                        .OfType<YamlScalarNode>()
                        .Select(s => s.Value ?? ""));
                    break;
            }
        }
    }

    extension(string yaml)
    {
        public Dictionary<string, string> LoadData()
        {
            using var stringReader = new StringReader(yaml);
            var yamlStream = new YamlStream();
            yamlStream.Load(stringReader);

            if (yamlStream.Documents.Count == 0 || yamlStream.Documents[0].RootNode is not YamlMappingNode root)
                return new(StringComparer.Ordinal);

            var result = new Dictionary<string, string>(StringComparer.Ordinal);

            WalkMapping(root, "", result);

            return result;
        }

        public LangConfig LoadConfig()
        {
            var result = new LangConfig();

            var loadData = yaml.LoadData();
            if (loadData.TryGetValue("base", out var baseStr) && Enum.TryParse<LangType>(baseStr, true, out var baseLangType))
                result.Base = baseLangType;
            if (loadData.TryGetValue("comment", out var commentStr) && Enum.TryParse<LangType>(commentStr, true, out var commentLangType))
                result.Comment = commentLangType;
            if (loadData.TryGetValue("modifier", out var modifierStr))
                result.Modifier = modifierStr;
            if (loadData.TryGetValue("className", out var classNameStr))
                result.Modifier = classNameStr;

            return result;
        }
    }
}
