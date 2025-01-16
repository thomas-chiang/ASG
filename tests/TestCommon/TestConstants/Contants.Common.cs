using ASG.Domain.Common.Enums;

namespace TestCommon.TestConstants;

public static partial class Constants
{
    public static class Common
    {
        public static readonly Guid DefaultUserEmployeeId = new("00000000-0000-0000-0000-000000000001");

        public static readonly Guid DefaultCompanyId = new("00000000-0000-0000-0000-000000000001");

        public const AttendanceType DefaultAttendanceType = AttendanceType.ClockIn;

        public static readonly Guid DefaultPunchesLocationId = new("00000000-0000-0000-0000-000000000001");

        public static readonly DateTime DefaultAttendanceOn = new(2025, 1, 1);
    }
}