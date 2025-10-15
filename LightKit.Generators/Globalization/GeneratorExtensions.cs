using Microsoft.CodeAnalysis;

namespace LightKit.Generators.Globalization;

static class GeneratorExtensions
{
    static void WriteLog(this SourceProductionContext spc, int id, object message, DiagnosticSeverity diagnosticSeverity, Location? location = null)
    {
        var diagnostic = Diagnostic.Create(new($"{GeneratorConfigs.GeneratorName}_{id:D3}", GeneratorConfigs.GeneratorName, "{0}", GeneratorConfigs.GeneratorName, diagnosticSeverity, true), location ?? Location.None, message);
        spc.ReportDiagnostic(diagnostic);
    }

    public static void WriteSkip(this SourceProductionContext spc, object message, Location? location = null)
    {
        spc.WriteLog(0, message, DiagnosticSeverity.Warning, location);
    }

    public static void WriteError(this SourceProductionContext spc, object message, Location? location = null)
    {
        spc.WriteLog(1, message, DiagnosticSeverity.Error, location);
    }

    public static void WriteWarning(this SourceProductionContext spc, object message, Location? location = null)
    {
        spc.WriteLog(2, message, DiagnosticSeverity.Warning, location);
    }

    public static void WriteMissing(this SourceProductionContext spc, object message, Location? location = null)
    {
        spc.WriteLog(3, message, DiagnosticSeverity.Error, location);
    }

    public static void WriteRedundant(this SourceProductionContext spc, object message, Location? location = null)
    {
        spc.WriteLog(4, message, DiagnosticSeverity.Warning, location);
    }

    public static void WriteDebug(this SourceProductionContext spc, object message, Location? location = null)
    {
#if DEBUG
        spc.WriteLog(999, message, DiagnosticSeverity.Warning, location);
#endif
    }
}
