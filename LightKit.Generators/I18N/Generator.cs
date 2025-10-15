using Microsoft.CodeAnalysis;

namespace LightKit.Generators.I18N;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var textFiles = context.AdditionalTextsProvider.Where(static file => file.Path.EndsWith(".yml", StringComparison.OrdinalIgnoreCase) && Path.GetFileName(Path.GetDirectoryName(file.Path))!.Equals("Langs", StringComparison.OrdinalIgnoreCase));
        var compilationProvider = context.CompilationProvider.Combine(textFiles.Collect());

        context.RegisterSourceOutput(compilationProvider, (spc, compilation) =>
        {
            try
            {
                if (compilation.Right.Length <= 0)
                {
                    spc.WriteSkip("No language files to process were found");
                    return;
                }

                var core = new GeneratorCore(spc, compilation.Left, compilation.Right);
                core.CheckMissing();
                core.CheckRedundant();
                core.GenerateLangKey();
                core.GenerateAssemblyInfo();
            }
            catch (Exception e)
            {
                spc.WriteError(e.ToString());
            }
        });
    }
}
