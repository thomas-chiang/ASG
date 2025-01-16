using ASG.Domain.ApolloAttendances;
using ASG.Domain.ApolloSyncGaia1001FormOperations;
using ASG.Domain.Gaia1001Forms;
using TestCommon.ApolloAttendances;
using TestCommon.Gaia1001Forms;

namespace TestCommon.ApolloSyncGaia1001FormOperations;

public class ApolloSyncGaia1001FormOperationFactory
{
    public static ApolloSyncGaia1001FormOperation CreateApolloSyncGaia1001FormOperation(
        Gaia1001Form? gaia1001Form = null,
        ApolloAttendance? apolloAttendance = null
    )
    {
        return new ApolloSyncGaia1001FormOperation
        {
            Gaia1001Form = gaia1001Form ?? Gaia1001FormFactory.CreateGaia1001Form(),
            ApolloAttendance = apolloAttendance ?? ApolloAttendanceFactory.CreateApolloAttendance()
        };
    }
}