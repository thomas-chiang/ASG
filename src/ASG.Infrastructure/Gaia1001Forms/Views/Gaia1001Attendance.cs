using ASG.Domain.Common;
using ASG.Domain.Gaia1001Forms;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.Gaia1001Forms.Views;

[Keyless]
public class Gaia1001Attendance
{
    public string FormStatus { get; set; }
    public string AttendanceType { get; set; }
    public DateTime AttendanceOn { get; set; }

    public Gaia1001FormStatus GetFormStatusEnum()
    {
        return FormStatus switch
        {
            "NC" => Gaia1001FormStatus.NotComplete,
            "SA" => Gaia1001FormStatus.Save,
            "WA" => Gaia1001FormStatus.WaitingApprove,
            "UA" => Gaia1001FormStatus.UnderApproving,
            "RJ" => Gaia1001FormStatus.Rejected,
            "AP" => Gaia1001FormStatus.Approved,
            "RC" => Gaia1001FormStatus.Recall,
            "DE" => Gaia1001FormStatus.Deleted,
            _ => throw new InvalidOperationException($"Invalid form_status value: {FormStatus}")
        };
    }

    public AttendanceType GetAttendanceTypeEnum()
    {
        return AttendanceType switch
        {
            "0" => Domain.Common.AttendanceType.Unknown,
            "1" => Domain.Common.AttendanceType.ClockIn,
            "2" => Domain.Common.AttendanceType.ClockOut,
            "4" => Domain.Common.AttendanceType.OutForBusiness,
            "5" => Domain.Common.AttendanceType.BreakStart,
            "6" => Domain.Common.AttendanceType.BreakEnd,
            _ => throw new InvalidOperationException($"Invalid form_status value: {AttendanceType}")
        };
    }
}