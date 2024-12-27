using ASG.Application.ApolloSyncActions.Interfaces;
using ASG.Application.Gaia1001Forms.Interfaces;
using ASG.Infrastructure.ApolloSyncActions.Persistence;
using ASG.Infrastructure.Common.Persistence;
using ASG.Infrastructure.Gaia1001Forms.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        Console.WriteLine($"AddInfrastructure");
        string connectionString = @"Server=sea-asia-tube-sqlsrv.database.windows.net;"
                                  + "Authentication=Active Directory Interactive; Encrypt=True; Database=AsiaFlowDB";
        
        using (var connection = new SqlConnection(connectionString))
        {
            connection.OpenAsync();
            Console.WriteLine("Connected to the database successfully!"); 
        }
        
        services.AddDbContext<SqlDbContext>(options =>
            options.UseSqlServer(connectionString));
        
        services.AddScoped<IApolloSyncActionRepository, ApolloSyncActionRepository>()
            .AddScoped<IGaia1001FormRepository, Gaia1001FormRepository>();
        
        return services;
    }
}