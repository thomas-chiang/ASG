using ASG.Application.ApolloAttendances.Interfaces;
using ASG.Application.Common.Interfaces;
using ASG.Application.Gaia1001Forms.Interfaces;
using ASG.Infrastructure.ApolloAttendances.Persistence;
using ASG.Infrastructure.Common.SqlServerDbContexts;
using ASG.Infrastructure.Common;
using ASG.Infrastructure.Gaia1001Forms.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var connectionString = @"Server=sea-asia-tube-sqlsrv.database.windows.net;"
                               + "Authentication=Active Directory Interactive; Encrypt=True";

        using (var connection = new SqlConnection(connectionString))
        {
            connection.OpenAsync();
        }

        services.AddDbContext<AsiaFlowDbContext>();
        services.AddDbContext<AsiaTubeManageDbContext>();

        services
            .AddScoped<IGaia1001FormRepository, Gaia1001FormRepository>()
            .AddScoped<IApolloAttendanceRepository, ApolloAttendenceRepository>();

        services.AddHttpClient<IAnonymousRequestSender, AnonymousRequestSender>();

        return services;
    }
}