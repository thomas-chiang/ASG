using ASG.Application.Gaia1001Forms.Interfaces;
using ASG.Domain.Gaia1001Forms;
using ASG.Infrastructure.Common.SqlServerDbContexts;
using ASG.Infrastructure.Gaia1001Forms.AisaFlowDbSchemas;
using ASG.Infrastructure.Gaia1001Forms.Views;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.Gaia1001Forms.Persistence;

public class Gaia1001FormRepository : IGaia1001FormRepository
{
    private readonly AsiaFlowDbContext _dbContext;

    public Gaia1001FormRepository(AsiaFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Gaia1001Form?> GetGaia1001Form(string formKind, int formNo)
    {
        var pTSyncForms = await _dbContext.PtSyncForms
            .Where(form => form.FormKind == formKind && form.FormNo == formNo)
            .ToListAsync();

        var attendanceInfo = await GetAttendanceInfo(
            formKind,
            formNo,
            "gbpm." + formKind.Replace(".", "")
        );

        var gaia1001Form = new Gaia1001Form
        {
            FormKind = formKind,
            FormNo = formNo,
            CompanyId = pTSyncForms.First().CompanyId,
            UserEmployeeId = pTSyncForms.First().UserEmployeeId,
            PtSyncFormOperations = GetPtSyncFormOperations(pTSyncForms),
            AttendanceOn = attendanceInfo.AttendanceOn,
            FormStatus = attendanceInfo.GetFormStatusEnum(),
            AttendanceType = attendanceInfo.GetAttendanceTypeEnum()
        };

        if (gaia1001Form.AttendanceOn.Year <= 2024)
            gaia1001Form.PtSyncFormOperations.AddRange(await Get2024ArchivedPtSyncFormOperations(formKind, formNo));
        else
            gaia1001Form.PtSyncFormOperations.AddRange(await Get2024ArchivedPtSyncFormOperations(formKind, formNo));

        gaia1001Form.PtSyncFormOperations = gaia1001Form.PtSyncFormOperations
            .OrderBy(op => op.CreatedOn)
            .ToList();

        return gaia1001Form;
    }

    public async Task<List<PtSyncFormOperation>> Get2024ArchivedPtSyncFormOperations(string formKind, int formNo)
    {
        return await _dbContext.PtSyncFormsArchive2024
            .Where(form => form.FormKind == formKind && form.FormNo == formNo)
            .Select(form => new PtSyncFormOperation
            {
                FormContent = form.FormContent,
                FormAction = form.GetFormActionEnum(),
                CreatedOn = form.CreatedOn,
                ModifiedOn = form.ModifiedOn,
                Flag = form.GetFlagEnum(),
                RetryCount = form.RetryCount
            }).ToListAsync();
    }

    public async Task<List<PtSyncFormOperation>> Get2025ArchivedPtSyncFormOperations(string formKind, int formNo)
    {
        return await _dbContext.PtSyncFormsArchive2025
            .Where(form => form.FormKind == formKind && form.FormNo == formNo)
            .Select(form => new PtSyncFormOperation
            {
                FormContent = form.FormContent,
                FormAction = form.GetFormActionEnum(),
                CreatedOn = form.CreatedOn,
                ModifiedOn = form.ModifiedOn,
                Flag = form.GetFlagEnum(),
                RetryCount = form.RetryCount
            }).ToListAsync();
    }

    private List<PtSyncFormOperation> GetPtSyncFormOperations(List<PtSyncForm> pTSyncForms)
    {
        return pTSyncForms.Select(form => new PtSyncFormOperation
        {
            FormContent = form.FormContent,
            FormAction = form.GetFormActionEnum(),
            CreatedOn = form.CreatedOn,
            ModifiedOn = form.ModifiedOn,
            Flag = form.GetFlagEnum(),
            RetryCount = form.RetryCount
        }).ToList();
    }

    private async Task<Gaia1001Attendance> GetAttendanceInfo(string formKind, int formNo, string tableName)
    {
        var attendanceQuery = $@"
            SELECT
                h.form_status as FormStatus,
                f.ATTENDANCETYPE AS AttendanceType,
                DATEADD(HOUR, 8, CAST(CONVERT(DATETIMEOFFSET, (CONVERT(NVARCHAR, f.[DATETIME], 126) + f.TIMEZONE)) AT TIME ZONE 'UTC' AS DATETIME)) AS AttendanceOn
            FROM {tableName} f
            JOIN gbpm.fm_form_header h ON f.form_no = h.form_no
            WHERE h.form_kind = @formKind AND h.form_no = @formNo";

        var gaia1001Attendance = await _dbContext.Set<Gaia1001Attendance>()
            .FromSqlRaw(attendanceQuery,
                new SqlParameter("@formKind", formKind),
                new SqlParameter("@formNo", formNo))
            .FirstOrDefaultAsync();


        return gaia1001Attendance ??
               throw new InvalidOperationException(
                   $"No attendance information found for formKind: {formKind} and formNo: {formNo}.");
        ;
    }
}