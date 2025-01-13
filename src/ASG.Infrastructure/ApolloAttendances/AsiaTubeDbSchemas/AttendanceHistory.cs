using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASG.Domain.ApolloAttendances;
using ASG.Domain.ApolloAttendances.Enums;
using ASG.Domain.Common;
using ASG.Domain.Common.Enums;

namespace ASG.Infrastructure.ApolloAttendances.AsiaTubeDbSchemas;

[Table("AttendanceHistory", Schema = "pt")]
public class AttendanceHistory
{
    [Key] public Guid AttendanceHistoryId { get; set; }

    public Guid EmployeeId { get; set; }
    public int iOriginType { get; set; }
    public int iAttendanceType { get; set; }
    public DateTime AttendanceDate { get; set; }
    public DateTime AttendanceOn { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsEffect { get; set; }
    public Guid PunchesLocationId { get; set; }

    [MaxLength(40)] public string? LocationName { get; set; }

    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    [MaxLength(500)] public string? LocationDetails { get; set; }

    [MaxLength(60)] public string? ExtendWorkHourType { get; set; }

    [MaxLength(60)] public string? CheckInTimeoutType { get; set; }

    public Guid? CheckInPersonalReasonTypeId { get; set; }

    [MaxLength(10)] public string? CheckInPersonalReasonCode { get; set; }

    [MaxLength(1000)] public string? CheckInPersonalReason { get; set; }

    public Guid CompanyId { get; set; }
    public Guid Creater { get; set; }
    public DateTime CreateTime { get; set; }
    public Guid LatestUpdater { get; set; }
    public DateTime LatestUpdateTime { get; set; }

    [MaxLength(500)] public string? IdentifyCode { get; set; }

    public int? EditTimes { get; set; }
    public int? iLastType { get; set; }

    [MaxLength(60)] public string? AdjustCheckInTimeoutType { get; set; }

    public Guid? AdjustCheckInPersonalReasonTypeId { get; set; }

    [MaxLength(10)] public string? AdjustCheckInPersonalReasonCode { get; set; }

    [MaxLength(1000)] public string? AdjustCheckInPersonalReason { get; set; }

    public static int GetAttendanceTypeValue(AttendanceType attendanceType)
    {
        switch (attendanceType)
        {
            case AttendanceType.Unknown:
                return 0;
            case AttendanceType.ClockIn:
                return 1;
            case AttendanceType.ClockOut:
                return 2;
            case AttendanceType.OutForBusiness:
                return 4;
            case AttendanceType.BreakStart:
                return 5;
            case AttendanceType.BreakEnd:
                return 6;
            default:
                throw new ArgumentOutOfRangeException(nameof(attendanceType), attendanceType, null);
        }
    }

    public AttendanceMethod GetAttendanceMethodEnum()
    {
        return iOriginType switch
        {
            0 => AttendanceMethod.Unknown, // Unknown
            1 => AttendanceMethod.TimeClock, // TimeClock
            2 => AttendanceMethod.App, // App
            3 => AttendanceMethod.Location, // Location
            4 => AttendanceMethod.Approval, // Approval
            5 => AttendanceMethod.Retroactive, // Retroactive
            8 => AttendanceMethod.ClientIn, // ClientIn
            16 => AttendanceMethod.Web, // Web
            17 => AttendanceMethod.Tablet, // Tablet
            18 => AttendanceMethod.Correction, // Correction
            _ => throw new ArgumentOutOfRangeException(nameof(iOriginType), iOriginType, "Invalid iOriginType value")
        };
    }
}