using ASG.Domain.Gaia1001Forms;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.Gaia1001Forms.Views;

[Keyless]
public class Gaia1001Attendance
{
    public string form_status { get; set; }
    public string AttendanceType { get; set; }
    public DateTime AttendanceOn { get; set; }
    
    public Gaia1001FormStatus GetFormStatusEnum()
    {
        if (Enum.TryParse(form_status, out Gaia1001FormStatus formStatus))
        {
            return formStatus;
        }
        else
        {
            throw new ArgumentException($"Invalid form_status value: {form_status}");
        }
    }
    
    public AttendanceType GetAttendanceTypeEnum()
    {
        if (Enum.TryParse(AttendanceType, out AttendanceType attendanceType))
        {
            return attendanceType;
        }
        else
        {
            throw new ArgumentException($"Invalid AttendanceType value: {AttendanceType}");
        }
    }
}