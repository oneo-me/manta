using Microsoft.CodeAnalysis;

namespace Axe.Icons.Generator;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var textFiles = context.AdditionalTextsProvider.Where(static file => file.Path.EndsWith(".svg", StringComparison.OrdinalIgnoreCase) && Path.GetFileName(Path.GetDirectoryName(file.Path))!.Equals("Icons", StringComparison.OrdinalIgnoreCase));
        var compilationProvider = context.CompilationProvider.Combine(textFiles.Collect());

        context.RegisterSourceOutput(compilationProvider, (spc, compilation) =>
        {
            try
            {
                if (compilation.Right.Length <= 0)
                {
                    spc.WriteSkip("No icons to process were found");
                    return;
                }

                var core = new GeneratorCore(spc, compilation.Left, compilation.Right);
                core.GenerateIconKey();
                core.GenerateIconData();
                core.GenerateAssemblyInfo();
            }
            catch (Exception e)
            {
                spc.WriteError(e.ToString());
            }
        });
    }
}
