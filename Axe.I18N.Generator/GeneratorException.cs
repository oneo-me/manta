using Microsoft.CodeAnalysis;

namespace Axe.I18N.Generator;

sealed class GeneratorException(string message, Location? location = null) : Exception(message)
{
    public Location? Location { get; } = location;
}
