using ASG.Domain.ApolloAttendances;
using ASG.Domain.Common;

namespace ASG.Application.ApolloAttendances.Interfaces;

public interface IApolloAttendanceRepository
{
    Task<ApolloAttendance?> GetApolloAttendance(
        Guid companyId,
        Guid userEmployeeId,
        DateOnly attendanceDate,
        AttendanceType attendanceType
    );
}