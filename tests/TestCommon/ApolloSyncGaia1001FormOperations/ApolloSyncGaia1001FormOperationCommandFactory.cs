using ASG.Application.ApolloSyncGaia1001FormOperations.Commands.CreateApolloSyncGaia1001FormOperation;
using TestCommon.TestConstants;

namespace TestCommon.ApolloSyncGaia1001FormOperations;

public static class ApolloSyncGaia1001FormOperationCommandFactory
{
    public static CreateApolloSyncGaia1001FormOperationCommand CreateCreateApolloSyncGaia1001FormOperationCommand(
        string formKind = Constants.Gaia1001Forms.DefaultFormKind,
        int formNo = Constants.Gaia1001Forms.DefaultFormNo)
    {
        return new CreateApolloSyncGaia1001FormOperationCommand(
            formKind,
            formNo);
    }
}