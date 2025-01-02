namespace ASG.Domain.ApolloAttendances;

public class Apollo1001Form
{
    public Guid? AttendanceHistoryId { get; set; }
    public  string? FormKind { get; set; }

    public  int? FormNo { get; set; }
    
    public Apollo1001ApprovalStatus ApprovalStatus { get; set; }
}