using Manta.Generators.LangKey;
using Microsoft.CodeAnalysis;

namespace Manta.Generators;

[Generator]
public class Generators : IIncrementalGenerator
{
    readonly List<GeneratorBase> _generators = new();

    public Generators()
    {
        _generators.Add(new LangKeyGenerator());
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        foreach (var generator in _generators)
        {
            var textFiles = context.AdditionalTextsProvider.Where(generator.MatchFile);
            var compilationProvider = context.CompilationProvider.Combine(textFiles.Collect());

            context.RegisterSourceOutput(compilationProvider, (spc, compilation) =>
            {
                try
                {
                    if (compilation.Right.Length <= 0)
                    {
                        generator.WriteSkip(spc, "No file found that needs to be processed by the generator");
                        return;
                    }

                    generator.Execute(spc, compilation.Left, compilation.Right);
                }
                catch (Exception e)
                {
                    generator.WriteError(spc, e.ToString());
                }
            });
        }
    }
}
