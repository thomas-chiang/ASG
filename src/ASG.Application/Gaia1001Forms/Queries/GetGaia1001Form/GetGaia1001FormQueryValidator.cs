using FluentValidation;

namespace ASG.Application.Gaia1001Forms.Queries.GetGaia1001Form;

public class GetGaia1001FormQueryValidator : AbstractValidator<GetGaia1001FormQuery>
{
    public GetGaia1001FormQueryValidator()
    {
        RuleFor(getGaia1001FormQuery => getGaia1001FormQuery)
            .ApplyFetchGaia1001FormRules();
    }
}

public static class Gaia1001FormFluentValidationExtensions
{
    public static IRuleBuilderOptions<T, dynamic> ApplyFetchGaia1001FormRules<T>(
        this IRuleBuilder<T, dynamic> ruleBuilder)
    {
        return ruleBuilder
            .Must(anonymousObject => anonymousObject != null && anonymousObject?.FormKind != null)
            .WithMessage("FormKind must not be null or empty.")
            .Must(anonymousObject => anonymousObject.FormKind.Contains("1001"))
            .WithMessage("FormKind must contain the string '1001'.")
            .Must(anonymousObject => anonymousObject != null && anonymousObject?.FormNo != null)
            .WithMessage("FormNo must not be null or empty.")
            .Must(anonymousObject => anonymousObject.FormNo > 0)
            .WithMessage("FormNo must be a positive number.");
    }
}