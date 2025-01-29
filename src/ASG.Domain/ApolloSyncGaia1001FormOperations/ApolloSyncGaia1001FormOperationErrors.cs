using ErrorOr;

namespace ASG.Domain.ApolloSyncGaia1001FormOperations;

public class ApolloSyncGaia1001FormOperationErrors
{
    public static readonly Error FailedApolloSyncGaia1001FormOperation = Error.Validation(
        "ApolloSyncGaia1001FormOperation.FailedSync",
        "Failed to sync Gaia 1001 form to Apollo. Need further investigation"
    );
}