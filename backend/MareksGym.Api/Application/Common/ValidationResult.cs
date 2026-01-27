namespace MareksGym.Api.Application.Common;

public sealed class ValidationResult
{
    public Dictionary<string, List<string>> Errors { get; } = new();

    public bool IsValid => Errors.Count == 0;

    public void AddError(string field, string message)
    {
        if (!Errors.TryGetValue(field, out var messages))
        {
            messages = new List<string>();
            Errors[field] = messages;
        }

        messages.Add(message);
    }
}
