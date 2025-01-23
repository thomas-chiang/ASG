using ASG.Domain.ApolloAttendances;
using ASG.Domain.ApolloAttendances.Enums;
using TestCommon.TestConstants;

namespace TestCommon.ApolloAttendances;

public class Apollo1001FormFactory
{
    public static Apollo1001Form CreateApollo1001Form(
        Guid? attendanceHistoryId = null,
        string? formKind = null,
        int? formNo = null,
        Apollo1001ApprovalStatus? approvalStatus = null
    )
    {
        return new Apollo1001Form
        {
            AttendanceHistoryId = attendanceHistoryId ?? Constants.ApolloAttendances.DefaultAttendanceHistoryId,
            FormKind = formKind ?? Constants.Gaia1001Forms.DefaultFormKind,
            FormNo = formNo ?? Constants.Gaia1001Forms.DefaultFormNo,
            ApprovalStatus = approvalStatus ?? Constants.ApolloAttendances.DefaultApprovalStatus
        };
    }
}