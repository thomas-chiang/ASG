using ASG.Domain.Common;
using ASG.Domain.Common.Enums;

namespace ASG.Domain.ApolloAttendances;

public class ApolloAttendance
{
    public Guid CompanyId { get; set; }

    public Guid UserEmployeeId { get; set; }
    
    public DateOnly AttendanceDate { get; set; }
    
    // 打卡目的: 上班，下班
    // TODO: Rename to AttendancePurpose
    public AttendanceType AttendanceType { get; set; }

    public List<ApolloAttendanceHistory> ApolloAttendanceHistories { get; set; } = new List<ApolloAttendanceHistory>();
    
    public List<Apollo1001Form> Apollo1001Forms { get; set; } = new List<Apollo1001Form>();
}