using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Manta.Globalization.Core;

public static class Extensions
{
    extension(LangType langType)
    {
        LangMetadataAttribute GetMetadataCore()
        {
            var enumType = langType.GetType();
            var memberInfo = enumType.GetMember(langType.ToString());
            if (memberInfo.Length > 0)
            {
                var attribute = memberInfo[0].GetCustomAttribute<LangMetadataAttribute>(false);
                if (attribute != null)
                    return attribute;
            }

            throw new InvalidOperationException($"Culture {langType} has no CultureMetadataAttribute");
        }

        public LangMetadata GetMetadata()
        {
            var langMetadataAttribute = langType.GetMetadataCore();
            return new(langType, langMetadataAttribute.Emoji, langMetadataAttribute.EnglishName, langMetadataAttribute.NativeName, langMetadataAttribute.SourceType);
        }
    }

    static readonly IDeserializer _deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    public static T DeserializeYaml<T>(this string yml) where T : class
    {
        return _deserializer.Deserialize<T>(yml) ?? throw new InvalidOperationException("Deserialize result is null");
    }
}
