using ASG.Application.Common.Interfaces;
using ASG.Infrastructure.Common.SqlServerDbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.Common;

public class AsiaTubeDbSetter : IAsiaTubeDbSetter
{
    private readonly AsiaTubeManageDbContext _asiaTubeManageDbContext;
    private readonly AsiaTubeDbContext _asiaTubeDbContext;

    public AsiaTubeDbSetter(AsiaTubeManageDbContext asiaTubeManageDbContext, AsiaTubeDbContext asiaTubeDbContext)
    {
        _asiaTubeManageDbContext = asiaTubeManageDbContext;
        _asiaTubeDbContext = asiaTubeDbContext;
    }

    public async Task SetAsiaTubeDb(Guid companyId)
    {
        var companyCode = await _asiaTubeManageDbContext.Companies
            .Where(c => c.CompanyId == companyId)
            .Select(c => c.CompanyCode)
            .FirstOrDefaultAsync();
        if (string.IsNullOrEmpty(companyCode))
            throw new InvalidOperationException($"Company with ID {companyId} not found.");

        var connectionString = @"Server=sea-asia-tube-sqlsrv.database.windows.net;"
                               + $"Authentication=Active Directory Interactive; Encrypt=True; Database=AsiaTube{companyCode}";
        try
        {
            await using var comConnection = new SqlConnection(connectionString);
            await comConnection.OpenAsync();
        }
        catch
        {
            connectionString = @"Server=sea-asia-tube-sqlsrv.database.windows.net;"
                               + "Authentication=Active Directory Interactive; Encrypt=True; Database=AsiaTubeDB";
        }

        _asiaTubeDbContext.Database.SetConnectionString(connectionString);
    }
}