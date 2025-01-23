using ASG.Domain.Common.Enums;
using ASG.Domain.Gaia1001Forms.Enums;

namespace TestCommon.TestConstants;

public static partial class Constants
{
    public static class Gaia1001Forms
    {
        public const string DefaultFormKind = "CompanyCode9.FORM.1001";

        public const int DefaultFormNo = 1;

        public const int DefaultOtherFormNo = 2;

        public const Gaia1001FormStatus DefaultGaia1001FormStatus = Gaia1001FormStatus.Approved;

        public static readonly Guid DefaultLocationId = new("00000000-0000-0000-0000-000000000001");

        public const AttendanceType DefaultAttendanceType = AttendanceType.ClockIn;
    }
}