using ASG.Application.Common.Interfaces;
using Microsoft.Data.SqlClient;

namespace ASG.Infrastructure.Common;

public class DbAccessor : IDbAccessor
{
    public async Task GainAccess()
    {
        await using var connection =
            new SqlConnection(
                "Server=sea-asia-tube-sqlsrv.database.windows.net;Authentication=Active Directory Interactive; Encrypt=True");
        await connection.OpenAsync();
    }
}