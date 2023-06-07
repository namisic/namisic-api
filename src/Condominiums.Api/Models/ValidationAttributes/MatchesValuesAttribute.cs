namespace System.ComponentModel.DataAnnotations;

/// <summary>
/// Validates that the value entered matches one of the allowed values.
/// </summary>
public class MatchesValuesAttribute : ValidationAttribute
{
    private readonly object[] _possibleValues;

    public MatchesValuesAttribute(params object[] possibleValues)
    {
        _possibleValues = possibleValues;
    }

    public string GetErrorMessage() => ErrorMessage ?? $"The value entered does not match some of the allowed values. Possible values are: {string.Join(", ", _possibleValues)}.";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null && _possibleValues != null && !_possibleValues.Contains(value))
        {
            return new ValidationResult(GetErrorMessage());
        }

        return ValidationResult.Success;
    }
}
