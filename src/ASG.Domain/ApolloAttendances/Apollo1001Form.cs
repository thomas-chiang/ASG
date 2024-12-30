namespace ASG.Domain.ApolloAttendances;

public class Apollo1001Form
{
    public required string FormKind { get; set; }

    public required int FormNo { get; set; }
    
    public Apollo1001ApprovalStatus ApprovalStatus { get; set; }
}