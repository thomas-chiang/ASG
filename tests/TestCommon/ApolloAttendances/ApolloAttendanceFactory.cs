using ASG.Domain.ApolloAttendances;
using ASG.Domain.Common.Enums;
using TestCommon.TestConstants;

namespace TestCommon.ApolloAttendances;

public class ApolloAttendanceFactory
{
    public static ApolloAttendance CreateApolloAttendance(
        Guid? companyId = null,
        Guid? userEmployeeId = null,
        DateOnly? attendanceDate = null,
        AttendanceType attendanceType = Constants.Common.DefaultAttendanceType,
        List<ApolloAttendanceHistory>? apolloAttendanceHistories = null,
        List<Apollo1001Form>? apollo1001Forms = null
    )
    {
        return new ApolloAttendance
        {
            CompanyId = companyId ?? Constants.Common.DefaultCompanyId,
            UserEmployeeId = userEmployeeId ?? Constants.Common.DefaultUserEmployeeId,
            AttendanceDate = attendanceDate ?? Constants.ApolloAttendances.DefaultAttendanceDate,
            AttendanceType = attendanceType,
            ApolloAttendanceHistories = apolloAttendanceHistories ?? new List<ApolloAttendanceHistory>(),
            Apollo1001Forms = apollo1001Forms ?? new List<Apollo1001Form>()
        };
    }
}