using ASG.Application.ApolloAttendances.Interfaces;
using ASG.Domain.ApolloAttendances;
using ASG.Domain.Common;
using ASG.Infrastructure.ApolloAttendences.DynamicDbSchemas;
using ASG.Infrastructure.Common.SqlServerDbContexts;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.ApolloAttendences.Persistence;

public class ApolloAttendenceRepository : IApolloAttendanceRepository
{
    private readonly AsiaTubeManageDbContext _asiaTubeManageDbContext;

    public ApolloAttendenceRepository(AsiaTubeManageDbContext asiaTubeManageDbContext)
    {
        _asiaTubeManageDbContext = asiaTubeManageDbContext;
    }

    public async Task<ApolloAttendance?> GetApolloAttendance(
        Guid companyId, 
        Guid userEmployeeId,
        DateOnly attendanceDate, 
        AttendanceType attendanceType
    )
    {
        var connectionString = await _asiaTubeManageDbContext.GetCompanyDbConnectionString(companyId);
        
        using (var context = new DynamicDbContext(connectionString))
        {
            // Perform database operations
            var attendanceHistories = await context.AttendanceHistories
                .Where(history => history.CompanyId == companyId 
                                  && history.EmployeeId == userEmployeeId 
                                  && history.iAttendanceType == AttendanceHistory.GetAttendanceTypeValue(attendanceType) 
                                  && history.AttendanceDate.Date == attendanceDate.ToDateTime(new TimeOnly(0, 0)).Date)
                .ToListAsync();
            foreach (var history in attendanceHistories)
            {
                Console.WriteLine($"LocationName: {history.LocationName}");
            }
            
            return new ApolloAttendance
            {
                CompanyId = companyId,
                UserEmployeeId = userEmployeeId,
                ApolloAttendanceHistories = attendanceHistories.Select(history => new ApolloAttendanceHistory
                {
                    AttendanceMethod = history.GetAttendanceMethodEnum(), // Map iOriginType to AttendanceMethod
                    AttendanceOn = history.AttendanceOn,
                    IsEffective = history.IsEffect
                }).ToList(),
                Apollo1001Forms = null
            };
        }
    }
}