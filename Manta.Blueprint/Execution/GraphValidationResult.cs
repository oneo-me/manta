namespace Manta.Blueprint.Execution;

public sealed class GraphValidationResult
{
    public bool IsValid { get; init; }
    public List<string> Errors { get; init; } = [];
    public List<string> Warnings { get; init; } = [];

    public static GraphValidationResult Success() => new() { IsValid = true };

    public static GraphValidationResult Failure(params string[] errors) => new()
    {
        IsValid = false,
        Errors = [..errors]
    };
}
