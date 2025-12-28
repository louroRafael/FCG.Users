using FluentValidation;
using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace FCG.Users.Domain.DTOs.Requests;

public abstract class RequestBase<T> where T : RequestBase<T>
{
    private ValidationResult _validationResult;
    [JsonIgnore]
    public ValidationResult ValidationResult => _validationResult;

    public bool IsValid()
    {
        var validator = GetValidator();
        if (validator == null)
            throw new InvalidOperationException("Validator not provided.");

        _validationResult = validator.Validate((T)this);
        return _validationResult.IsValid;
    }

    protected abstract IValidator<T> GetValidator();
}
