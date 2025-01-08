using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASG.Domain.ApolloAttendances;
using ASG.Domain.ApolloAttendances.Enums;

namespace ASG.Infrastructure.ApolloAttendances.AsiaTubeDbSchemas;

[Table("AttendanceHistoryRecord", Schema = "pt")]
public class AttendanceHistoryRecord
{
    [Key] 
    public Guid AttendanceHistoryRecordId { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid? BehalfEmployeeId { get; set; }

    public Guid? AttendanceHistoryId { get; set; }

    public int iAttendanceType { get; set; }

    public DateTime AttendanceDate { get; set; }

    public DateTime AttendanceOn { get; set; }

    public Guid PunchesLocationId { get; set; }

    [MaxLength(40)] public string? LocationName { get; set; }

    [MaxLength(500)] public string? LocationDetails { get; set; }

    [MaxLength(500)] public string? ReasonsForMissedClocking { get; set; }

    public int iApprovalType { get; set; }

    public int iApprovalResult { get; set; }

    [MaxLength(30)] public string? ExtendWorkHourType { get; set; }

    [MaxLength(30)] public string? CheckInTimeoutType { get; set; }

    public Guid CheckInPersonalReasonTypeId { get; set; }

    [MaxLength(10)] public string? CheckInPersonalReasonCode { get; set; }

    [MaxLength(500)] public string? CheckInPersonalReason { get; set; }

    [MaxLength(-1)] public string? strNowStepApproveEmployeeId { get; set; }

    [MaxLength(-1)] public string? strApproveEmployeeId { get; set; }

    [MaxLength(-1)] public string? strApproveHistory { get; set; }

    public Guid CompanyId { get; set; }

    public Guid Creater { get; set; }

    public DateTime CreateTime { get; set; }

    public Guid LatestUpdater { get; set; }

    public DateTime LatestUpdateTime { get; set; }

    public int? SourceFormNo { get; set; }

    [MaxLength(100)] public string? SourceFormKind { get; set; }

    public DateTime? OriginalAttendanceOn { get; set; }

    [MaxLength(30)] public string? AdjustCheckInTimeoutType { get; set; }

    public Guid? AdjustCheckInPersonalReasonTypeId { get; set; }

    [MaxLength(10)] public string? AdjustCheckInPersonalReasonCode { get; set; }

    [MaxLength(500)] public string? AdjustCheckInPersonalReason { get; set; }
    
    public Apollo1001ApprovalStatus GetApollo1001ApprovalStatusEnum()
    {
        return iApprovalResult switch
        {
            0 => Apollo1001ApprovalStatus.Unknown,    // 簽核中
            1 => Apollo1001ApprovalStatus.Ok,         // 同意
            2 => Apollo1001ApprovalStatus.Deny,       // 不同意
            3 => Apollo1001ApprovalStatus.Complete,   // 完成
            99 => Apollo1001ApprovalStatus.Delete,    // 刪除作廢
            _ => throw new ArgumentOutOfRangeException(nameof(iApprovalResult), iApprovalResult, "Invalid iApprovalResult value")
        };
    }
}