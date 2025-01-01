using ASG.Domain.Common;

namespace ASG.Domain.Gaia1001Forms;

public class Gaia1001Form
{
    // TODO: create class FormKind
    public required string FormKind { get; set; }

    public required int FormNo { get; set; }
    
    public Guid CompanyId { get; set; }

    public Guid UserEmployeeId { get; set; }
    
    public List<PtSyncFormOperation>? PtSyncFormOperations { get; set; }
    
    public Gaia1001FormStatus FormStatus { get; set; }
    
    public AttendanceType AttendanceType { get; set; }
    
    public DateTime AttendanceOn { get; set; }
    
    public bool IsBehalf { get; set; }
    
    public Guid PunchesLocationId { get; set; }
    
    public string? LocationDetails { get; set; }
    
    public string? ReasonsForMissedClocking { get; set; }
    
    public string? ExtendWorkHourType { get; set; }
    
    public string? CheckInTimeoutType { get; set; }
    
    public Guid? CheckInPersonalReasonTypeId { get; set; }
    
    public string? CheckInPersonalReason { get; set; }
    
    public bool IsApproved { get; set; }
}