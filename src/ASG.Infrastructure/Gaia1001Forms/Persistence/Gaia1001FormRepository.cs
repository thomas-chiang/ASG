using ASG.Application.Gaia1001Forms.Interfaces;
using ASG.Domain.Gaia1001Forms;
using ASG.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.Gaia1001Forms.Persistence;

public class Gaia1001FormRepository : IGaia1001FormRepository
{
    
    public readonly SqlDbContext _dbContext;

    public Gaia1001FormRepository(SqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Gaia1001Form?> GetByFormKindPlusFormNoAsync(string formKindPlusFormNo)
    {
        var pTSyncForms =  await _dbContext.PTSyncForms
            .Where(form => form.FormKind == "WVT9.FORM.1001" && form.FormNo == 48585)
            .ToListAsync();

        
        foreach (var form in pTSyncForms)
        {
            Console.WriteLine($"Id: {form.Id}");
            Console.WriteLine($"CompanyId: {form.CompanyId}");
            Console.WriteLine($"UserEmployeeId: {form.UserEmployeeId}");
            Console.WriteLine($"FormKind: {form.FormKind}");
            Console.WriteLine($"FormNo: {form.FormNo}");
            Console.WriteLine($"FormContent: {form.FormContent}");
            Console.WriteLine($"FormAction: {form.FormAction}");
            Console.WriteLine($"CreatedOn: {form.CreatedOn}");
            Console.WriteLine($"ModifiedOn: {form.ModifiedOn}");
            Console.WriteLine($"Flag: {form.Flag}");
            Console.WriteLine($"RetryCount: {form.RetryCount}");
            Console.WriteLine(new string('-', 50)); // separator line for readability
        }
        
        return new Gaia1001Form
        {
            FormStatus = Gaia1001FormStatus.AP
        };
    }
}