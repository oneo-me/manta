using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MantaDemo.Helpers;

[YamlStaticContext]
#pragma warning disable CA1515
sealed partial class ConfigYamlStaticContext
#pragma warning restore CA1515
{
    public static IDeserializer Deserializer { get; } = new StaticDeserializerBuilder(new ConfigYamlStaticContext())
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    public static ISerializer Serializer { get; } = new StaticSerializerBuilder(new ConfigYamlStaticContext())
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();
}
