namespace LightKit.Globalization.Core;

sealed class LangConfig
{
    public LangType Base { get; set; } = LangType.en;
    public LangType Comment { get; set; } = LangType.en;
    public string Modifier { get; set; } = "public";
    public string ClassName { get; set; } = "LangKey";

    public static LangConfig Load(string yaml)
    {
        return yaml.DeserializeYaml<LangConfig>();
    }
}
