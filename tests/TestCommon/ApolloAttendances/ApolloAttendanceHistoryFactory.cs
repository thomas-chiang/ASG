using ASG.Domain.ApolloAttendances;
using ASG.Domain.ApolloAttendances.Enums;
using TestCommon.TestConstants;

namespace TestCommon.ApolloAttendances;

public class ApolloAttendanceHistoryFactory
{
    public static ApolloAttendanceHistory CreateApolloAttendanceHistory(
        Guid? attendanceHistoryId = null,
        AttendanceMethod? attendanceMethod = null,
        DateTime? attendanceOn = null,
        bool? isEffective = null
    )
    {
        return new ApolloAttendanceHistory
        {
            AttendanceHistoryId = attendanceHistoryId ?? Constants.ApolloAttendances.DefaultAttendanceHistoryId,
            AttendanceMethod = attendanceMethod ?? Constants.ApolloAttendances.DefaultAttendanceMethod,
            AttendanceOn = attendanceOn ?? Constants.Common.DefaultAttendanceOn,
            IsEffective = isEffective ?? true
        };
    }
}