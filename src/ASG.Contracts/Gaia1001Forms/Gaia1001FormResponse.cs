namespace ASG.Contracts.Gaia1001Forms;

// public record class Gaia1001FormResponse(
//     string FormKind,
//     int FormNo,
//     Guid CompanyId,
//     Guid UserEmployeeId,
//     List<PTSyncFormOperation> PtSyncFormOperations,
//     string FormStatus,
//     DateTime AttendanceOn,
//     string AttendanceType
// );

public record Gaia1001FormResponse
{
    public string FormKind { get; init; }
    public int FormNo { get; init; }
    public Guid CompanyId { get; init; }
    public Guid UserEmployeeId { get; init; }
    public List<PtSyncFormOperation> PtSyncFormOperations { get; init; }
    public string FormStatus { get; init; }
    public DateTime AttendanceOn { get; init; }
    public string AttendanceType { get; init; }
}