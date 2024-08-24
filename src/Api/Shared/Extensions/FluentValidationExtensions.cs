using FluentValidation;

namespace Api.Shared.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> IsGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(value => Guid.TryParse(value, out _)).WithMessage("Invalid ObjectId");
    }
}
