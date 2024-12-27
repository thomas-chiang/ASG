using ASG.Application.Gaia1001Forms.Interfaces;
using ASG.Domain.Gaia1001Forms;
using ASG.Infrastructure.Common.SqlPersistence;
using ASG.Infrastructure.Gaia1001Forms.SqlSchemas;
using ASG.Infrastructure.Gaia1001Forms.Views;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.Gaia1001Forms.Persistence;

public class Gaia1001FormRepository : IGaia1001FormRepository
{
    
    public readonly SqlDbContext _dbContext;

    public Gaia1001FormRepository(SqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Gaia1001Form?> GetGaia1001Form(string formKind, int formNo)
    {
        var pTSyncForms =  await _dbContext.PTSyncForms
            .Where(form => form.FormKind == formKind && form.FormNo == formNo)
            .ToListAsync();
        
        var attendanceInfo= await GetAttendanceInfo(
            formKind, 
            formNo,
            "gbpm." + formKind.Replace(".", "")
        );
        
        return new Gaia1001Form
        {
            FormKind = formKind,
            FormNo = formNo,
            CompanyId = pTSyncForms.First().CompanyId,
            UserEmployeeId = pTSyncForms.First().UserEmployeeId,
            PtSyncFormOperations = SetPtSyncFormOperations(pTSyncForms),
            AttendanceOn = attendanceInfo.AttendanceOn,
            FormStatus = attendanceInfo.GetFormStatusEnum(),
            AttendanceType = attendanceInfo.GetAttendanceTypeEnum(),
        };
    }
    
    private List<PTSyncFormOperation> SetPtSyncFormOperations(List<PTSyncForm> pTSyncForms)
    {
        var ptSyncFormOperations = new List<PTSyncFormOperation>();

        foreach (var form in pTSyncForms)
        {
            var operation = new PTSyncFormOperation
            {
                FormContent = form.FormContent,
                FormAction = form.FormAction,
                CreatedOn = form.CreatedOn,
                ModifiedOn = form.ModifiedOn,
                Flag = form.Flag,
                RetryCount = form.RetryCount
            };

            ptSyncFormOperations.Add(operation);
        }

        return ptSyncFormOperations;
    }
    
    private async Task<Gaia1001Attendance> GetAttendanceInfo(string formKind, int formNo, string tableName)
    {
        string attendanceQuery = $@"
            SELECT
                h.form_status,
                f.ATTENDANCETYPE AS AttendanceType,
                CAST(CONVERT(DATETIMEOFFSET, (CONVERT(NVARCHAR, f.[DATETIME], 126) + f.TIMEZONE)) AT TIME ZONE 'UTC' AS DATETIME) AS AttendanceOn
            FROM {tableName} f
            JOIN gbpm.fm_form_header h ON f.form_no = h.form_no
            WHERE h.form_kind = @formKind AND h.form_no = @formNo";

        var gaia1001Attendance = await _dbContext.Set<Gaia1001Attendance>()
            .FromSqlRaw(attendanceQuery, 
                new SqlParameter("@formKind", formKind),
                new SqlParameter("@formNo", formNo))
            .FirstOrDefaultAsync(); 
        

        return gaia1001Attendance ?? throw new InvalidOperationException($"No attendance information found for formKind: {formKind} and formNo: {formNo}.");
        ;
    }
}