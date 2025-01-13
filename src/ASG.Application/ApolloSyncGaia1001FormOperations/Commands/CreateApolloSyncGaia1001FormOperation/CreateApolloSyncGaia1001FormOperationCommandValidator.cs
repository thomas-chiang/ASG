using ASG.Application.Gaia1001Forms.Queries.GetGaia1001Form;
using FluentValidation;

namespace ASG.Application.ApolloSyncGaia1001FormOperations.Commands.CreateApolloSyncGaia1001FormOperation;

public class
    CreateApolloSyncGaia1001FormOperationCommandValidator : AbstractValidator<
    CreateApolloSyncGaia1001FormOperationCommand>
{
    public CreateApolloSyncGaia1001FormOperationCommandValidator()
    {
        RuleFor(createApolloSyncGaia1001FormOperationCommand => createApolloSyncGaia1001FormOperationCommand)
            .ApplyFetchGaia1001FormRules();
    }
}