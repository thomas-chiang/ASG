using ErrorOr;

namespace ASG.Application.ApolloSyncGaia1001FormOperations.Commands.CreateApolloSyncGaia1001FormOperation;

public class CreateApolloSyncGaia1001FormOperationCommandErrors
{
    public static Error Gaia1001FormNotFound(string formKind, int formNo)
    {
        return Error.NotFound(
            "Gaia1001Form.NotFound",
            $"Gaia1001 form with FormKind '{formKind}' and FormNo '{formNo}' not found."
        );
    }
}