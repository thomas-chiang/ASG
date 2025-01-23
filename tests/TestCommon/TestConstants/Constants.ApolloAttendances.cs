using ASG.Domain.ApolloAttendances.Enums;

namespace TestCommon.TestConstants;

public static partial class Constants
{
    public static class ApolloAttendances
    {
        public static readonly DateOnly DefaultAttendanceDate = new(2025, 1, 1);

        public static readonly Guid DefaultAttendanceHistoryId = new("00000000-0000-0000-0000-000000000001");

        public const AttendanceMethod DefaultAttendanceMethod = AttendanceMethod.App;

        public const Apollo1001ApprovalStatus DefaultApprovalStatus = Apollo1001ApprovalStatus.Ok;
    }
}