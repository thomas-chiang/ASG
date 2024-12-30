namespace ASG.Domain.ApolloAttendances;

public class ApolloAttendanceHistory
{
    // 打卡方式: APP打卡...
    public AttendanceMethod AttendanceMethod { get; set; }
    
    public DateTime AttendanceOn { get; set; }

    public Boolean IsEffective;
}