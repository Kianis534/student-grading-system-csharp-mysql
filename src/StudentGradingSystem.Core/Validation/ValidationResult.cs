namespace StudentGradingSystem.Core.Validation;

/// <summary>Result of a business-rule check: either valid, or invalid with a user-facing message.</summary>
public sealed class ValidationResult
{
    private ValidationResult(bool isValid, string? error)
    {
        IsValid = isValid;
        Error = error;
    }

    public static ValidationResult Success { get; } = new ValidationResult(true, null);

    public bool IsValid { get; }

    /// <summary>Why validation failed; <c>null</c> when <see cref="IsValid"/> is true.</summary>
    public string? Error { get; }

    public static ValidationResult Fail(string error) => new ValidationResult(false, error);
}
