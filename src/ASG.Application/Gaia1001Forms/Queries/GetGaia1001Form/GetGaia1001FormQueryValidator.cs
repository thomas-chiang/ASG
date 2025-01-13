using FluentValidation;

namespace ASG.Application.Gaia1001Forms.Queries.GetGaia1001Form;

public class GetGaia1001FormQueryValidator : AbstractValidator<GetGaia1001FormQuery>
{
    public GetGaia1001FormQueryValidator()
    {
        RuleFor(getGaia1001FormQuery => getGaia1001FormQuery.FormKind)
            .ApplyGaia1001FormFormKindRules();
    }
}

public static class Gaia1001FormFluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ApplyGaia1001FormFormKindRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("FormKind must not be empty.")
            .Must(formKind => formKind.Contains("1001")).WithMessage("FormKind must contain the string '1001'.");
    }
}