using ASG.Contracts.ApolloAttendences;
using ASG.Contracts.Gaia1001Forms;

namespace ASG.Contracts.ApolloSyncGaia1001FormOperation;

public record CreateApolloSyncGaia1001FormOperationResponse(
    GetGaia1001FormResponse Gaia1001Form, 
    ApolloAttendanceResponse ApolloAttendance,
    ApolloSyncGaia1001FormOperationResponse ApolloSyncGaia1001FormOperation,
    ApolloAttendanceResponse? UpdatedApolloAttendance
);