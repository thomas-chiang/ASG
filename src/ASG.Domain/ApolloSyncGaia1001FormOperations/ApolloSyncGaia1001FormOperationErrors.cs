using ErrorOr;

namespace ASG.Domain.ApolloSyncGaia1001FormOperations;

public class ApolloSyncGaia1001FormOperationErrors
{
    public static readonly Error ApolloAttendanceNotFetchedAgain = Error.Validation(
        "ApolloAttendance.NotFetched",
        "Need to fetch Apollo attendance again."
    );
}