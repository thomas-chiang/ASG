using ASG.Application.Gaia1001Forms.Queries.GetGaia1001Form;
using FluentValidation;

namespace ASG.Application.ApolloAttendances.Queries.GetGaia1001FormWithApolloAttendance;

public class GetGaia1001FormWithApolloAttendanceQueryValidator : AbstractValidator<GetGaia1001FormWithApolloAttendanceQuery>
{
    public GetGaia1001FormWithApolloAttendanceQueryValidator()
    {
        RuleFor(getGaia1001FormWithApolloAttendanceQuery => getGaia1001FormWithApolloAttendanceQuery.FormKind)
            .ApplyGaia1001FormFormKindRules();
    }
}