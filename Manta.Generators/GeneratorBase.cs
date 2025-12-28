using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Manta.Generators;

abstract class GeneratorBase
{
    public abstract string Name { get; }
    public abstract int Version { get; }
    public abstract bool MatchFile(AdditionalText file);

    void WriteLog(SourceProductionContext spc, int id, object message, DiagnosticSeverity diagnosticSeverity, Location? location = null)
    {
        var diagnostic = Diagnostic.Create(new($"{Name}_{id:D3}", Name, "{0}", Name, diagnosticSeverity, true), location ?? Location.None, message);
        spc.ReportDiagnostic(diagnostic);
    }

    public void WriteSkip(SourceProductionContext spc, object message, Location? location = null)
    {
        WriteLog(spc, 0, message, DiagnosticSeverity.Warning, location);
    }

    public void WriteError(SourceProductionContext spc, object message, Location? location = null)
    {
        WriteLog(spc, 1, message, DiagnosticSeverity.Error, location);
    }

    public void WriteWarning(SourceProductionContext spc, object message, Location? location = null)
    {
        WriteLog(spc, 2, message, DiagnosticSeverity.Warning, location);
    }

    public void WriteDebug(SourceProductionContext spc, object message, Location? location = null)
    {
#if DEBUG
        WriteLog(spc, 999, message, DiagnosticSeverity.Warning, location);
#endif
    }

    public abstract void Execute(SourceProductionContext context, Compilation compilation, ImmutableArray<AdditionalText> files);
}
