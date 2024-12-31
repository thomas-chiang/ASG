namespace ASG.Contracts.ApolloAttendences;

public record ApolloAttendanceResponse
{ 
    public Guid CompanyId { get; init; }
    
    public Guid UserEmployeeId { get; init; }
    
    public DateOnly AttendanceDate { get; init; }
    
    public string AttendanceType { get; init; }
    public List<ApolloAttendanceHistoryResponse>? ApolloAttendanceHistories { get; init; }
    public List<Apollo1001FromResponse>? Apollo1001Froms { get; init; }
};