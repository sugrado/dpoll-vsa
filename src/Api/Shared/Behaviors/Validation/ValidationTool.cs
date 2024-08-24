using FluentValidation;
using FluentValidation.Results;

namespace Api.Shared.Behaviors.Validation;

public static class ValidationTool
{
    public static void Validate(IValidator validator, object entity)
    {
        ValidationContext<object> context = new(entity);
        ValidationResult result = validator.Validate(context);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}
