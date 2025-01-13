using FluentValidation;

namespace ASG.Application.Gaia1001Forms.Queries.GetGaia1001Form;

public class GetGaia1001FormQueryValidator : AbstractValidator<GetGaia1001FormQuery>
{
    public GetGaia1001FormQueryValidator()
    {
        RuleFor(getGaia1001FormQuery => getGaia1001FormQuery.FormKind)
            .ApplyGetGaia1001FormKindRules();
        RuleFor(getGaia1001FormQuery => getGaia1001FormQuery.FormNo)
            .GreaterThan(0).WithMessage("FormNo must be a positive number.");
    }
}

public static class Gaia1001FormFluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ApplyGetGaia1001FormKindRules<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(formKind => formKind.Contains(".1001"))
            .WithMessage("FormKind must contain the string '.1001'.")
            .Must(formKind => formKind.Contains("9."))
            .WithMessage("FormKind must contain the string '9.'.");
    }
}