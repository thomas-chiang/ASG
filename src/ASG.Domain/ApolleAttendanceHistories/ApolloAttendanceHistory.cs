using ASG.Domain.Common;

namespace ASG.Domain.ApolleAttendanceHistories;

public class ApolloAttendanceHistory
{
    public Guid CompanyId { get; set; }

    public Guid UserEmployeeId { get; set; }
    
    public DateOnly AttendanceDate { get; set; }
    
    public AttendanceType AttendanceType { get; set; }
    
    public AttendanceMethod ?AttendanceMethod { get; set; }
    
    
}