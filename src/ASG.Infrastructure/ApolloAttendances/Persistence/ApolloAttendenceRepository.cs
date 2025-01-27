using ASG.Application.ApolloAttendances.Interfaces;
using ASG.Domain.ApolloAttendances;
using ASG.Domain.Common;
using ASG.Domain.Common.Enums;
using ASG.Infrastructure.ApolloAttendances.AsiaTubeDbSchemas;
using ASG.Infrastructure.Common.SqlServerDbContexts;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.ApolloAttendances.Persistence;

public class ApolloAttendenceRepository : IApolloAttendanceRepository
{
    private readonly AsiaTubeManageDbContext _asiaTubeManageDbContext;
    private readonly AsiaTubeDbContext _asiaTubeDbContext;

    public ApolloAttendenceRepository(AsiaTubeManageDbContext asiaTubeManageDbContext,
        AsiaTubeDbContext asiaTubeDbContext)
    {
        _asiaTubeManageDbContext = asiaTubeManageDbContext;
        _asiaTubeDbContext = asiaTubeDbContext;
    }

    public async Task<ApolloAttendance> GetApolloAttendance(
        Guid companyId,
        Guid userEmployeeId,
        DateOnly attendanceDate,
        AttendanceType attendanceType
    )
    {
        var attendanceHistories = await _asiaTubeDbContext.AttendanceHistories
            .Where(history => history.CompanyId == companyId
                              && history.EmployeeId == userEmployeeId
                              && history.iAttendanceType == AttendanceHistory.GetAttendanceTypeValue(attendanceType)
                              && history.AttendanceDate.Date == attendanceDate.ToDateTime(new TimeOnly(0, 0)).Date)
            .ToListAsync();

        var attendanceHistoryRecords = await _asiaTubeDbContext.AttendanceHistoryRecords
            .Where(record => record.CompanyId == companyId
                             && record.EmployeeId == userEmployeeId
                             && record.iAttendanceType == AttendanceHistory.GetAttendanceTypeValue(attendanceType)
                             && record.AttendanceDate.Date == attendanceDate.ToDateTime(new TimeOnly(0, 0)).Date)
            .ToListAsync();

        return new ApolloAttendance
        {
            CompanyId = companyId,
            UserEmployeeId = userEmployeeId,
            AttendanceDate = attendanceDate,
            AttendanceType = attendanceType,
            ApolloAttendanceHistories = attendanceHistories.Select(history => new ApolloAttendanceHistory
            {
                AttendanceHistoryId = history.AttendanceHistoryId,
                AttendanceMethod = history.GetAttendanceMethodEnum(),
                AttendanceOn = history.AttendanceOn,
                IsEffective = history.IsEffect
            }).ToList(),
            Apollo1001Forms = attendanceHistoryRecords.Select(record => new Apollo1001Form
            {
                AttendanceHistoryId = record.AttendanceHistoryId,
                FormKind = record.SourceFormKind,
                FormNo = record.SourceFormNo,
                ApprovalStatus = record.GetApollo1001ApprovalStatusEnum()
            }).ToList()
        };
    }
}