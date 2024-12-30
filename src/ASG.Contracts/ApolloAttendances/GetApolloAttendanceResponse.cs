namespace ASG.Contracts.ApolloAttendances;

public record GetApolloAttendanceResponse
{ 
    public Guid CompanyId { get; init; }
    
    public Guid UserEmployeeId { get; init; }
    public List<ApolloAttendanceHistoryResponse>? ApolloAttendanceHistories { get; init; }
    public List<Apollo1001FromResponse>? Apollo1001FromResponses { get; init; }
};