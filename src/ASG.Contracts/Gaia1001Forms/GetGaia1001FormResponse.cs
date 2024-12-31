namespace ASG.Contracts.Gaia1001Forms;

public record GetGaia1001FormResponse
{
    public string FormKind { get; init; }
    public int FormNo { get; init; }
    public Guid CompanyId { get; init; }
    public Guid UserEmployeeId { get; init; }
    public List<PTSyncFormOperationResponse> PtSyncFormOperations { get; init; }
    public string FormStatus { get; init; }
    public DateTime AttendanceOn { get; init; }
    public string AttendanceType { get; init; }
}