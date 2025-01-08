using ASG.Domain.ApolloAttendances.Enums;

namespace ASG.Domain.ApolloAttendances;

public class ApolloAttendanceHistory
{
    public Guid AttendanceHistoryId { get; set; }
    // 打卡方式: APP打卡...
    public AttendanceMethod AttendanceMethod { get; set; }
    
    public DateTime AttendanceOn { get; set; }

    public Boolean IsEffective;
}