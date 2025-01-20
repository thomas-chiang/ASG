using ASG.Application.Common.Interfaces;
using ASG.Application.SubcutaneousTests.Common.Infrastructure.Common;
using ASG.Application.SubcutaneousTests.Common.Infrastructure.Common.SqlServerDbContexts;
using ASG.Application.SubcutaneousTests.Common.Infrastructure.TestDatabases;
using ASG.Infrastructure.Common.SqlServerDbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASG.Api.IntegrationTests.Common;

public class AsgApiFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    public AsiaFlowDbTestDatabase AsiaFlowDbTestDatabase = null!;
    public AsiaTubeManageDbTestDatabase AsiaTubeManageDbTestDatabase = null!;
    public AsiaTubeDbTestDatabase AsiaTubeDbTestDatabase = null!;

    public HttpClient HttpClient { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        AsiaFlowDbTestDatabase = AsiaFlowDbTestDatabase.CreateAndInitialize();
        AsiaTubeManageDbTestDatabase = AsiaTubeManageDbTestDatabase.CreateAndInitialize();
        AsiaTubeDbTestDatabase = AsiaTubeDbTestDatabase.CreateAndInitialize();

        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<DbContextOptions<AsiaFlowDbContext>>()
                .AddDbContext<AsiaFlowDbContext>((sp, options) =>
                    options.UseSqlServer(AsiaFlowDbTestDatabase.Connection))
                .RemoveAll<DbContextOptions<AsiaTubeManageDbContext>>()
                .AddDbContext<AsiaTubeManageDbContext>((sp, options) =>
                    options.UseSqlServer(AsiaTubeManageDbTestDatabase.Connection))
                .RemoveAll<AsiaTubeManageDbContext>()
                .AddTransient(serviceProvider => MockAsiaTubeManageDbContext.CreateMock().Object)
                .RemoveAll<IAnonymousRequestSender>()
                .AddTransient<IAnonymousRequestSender, MockAnonymousRequestSender>()
                .RemoveAll<IDbAccessor>()
                .AddTransient<IDbAccessor, MockDbAccessor>()
                ;
        });
    }

    public Task InitializeAsync()
    {
        HttpClient = CreateClient();

        return Task.CompletedTask;
    }


    public new Task DisposeAsync()
    {
        AsiaFlowDbTestDatabase.Dispose();
        AsiaTubeManageDbTestDatabase.Dispose();
        AsiaTubeDbTestDatabase.Dispose();

        return Task.CompletedTask;
    }

    public void ResetDatabase()
    {
        AsiaFlowDbTestDatabase.ResetDatabase();
        AsiaTubeManageDbTestDatabase.ResetDatabase();
        AsiaTubeDbTestDatabase.ResetDatabase();
    }
}