namespace Manta.NodeEditor.Execution;

public sealed class GraphValidationResult
{
    public bool IsValid { get; init; }
    public List<string> Errors { get; init; } = [];
    public List<string> Warnings { get; init; } = [];

    public static GraphValidationResult Success()
    {
        return new() { IsValid = true };
    }

    public static GraphValidationResult Failure(params string[] errors)
    {
        return new()
        {
            IsValid = false,
            Errors = [..errors]
        };
    }
}
