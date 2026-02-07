namespace Manta.NodeEditor.TypeSystem;

public delegate object? TypeConvertFunc(object? value);

public sealed class TypeConverter
{
    public required Type SourceType { get; init; }
    public required Type TargetType { get; init; }
    public required TypeConvertFunc Convert { get; init; }
}
