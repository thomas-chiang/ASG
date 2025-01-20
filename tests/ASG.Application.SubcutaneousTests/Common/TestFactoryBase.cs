using ASG.Application.Common.Interfaces;
using ASG.Application.SubcutaneousTests.Common.Infrastructure.Common;
using ASG.Application.SubcutaneousTests.Common.Infrastructure.TestDatabases;
using ASG.Infrastructure.Common.SqlServerDbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASG.Application.SubcutaneousTests.Common;

public abstract class TestFactoryBase<TEntryPoint> : WebApplicationFactory<TEntryPoint>, IAsyncLifetime
    where TEntryPoint : class
{
    public AsiaFlowDbTestDatabase AsiaFlowDbTestDatabase = null!;
    public AsiaTubeManageDbTestDatabase AsiaTubeManageDbTestDatabase = null!;
    public AsiaTubeDbTestDatabase AsiaTubeDbTestDatabase = null!;

    protected TestFactoryBase(AsiaFlowDbTestDatabase asiaFlowDbTestDatabase,
        AsiaTubeManageDbTestDatabase asiaTubeManageDbTestDatabase, AsiaTubeDbTestDatabase asiaTubeDbTestDatabase)
    {
        AsiaFlowDbTestDatabase = asiaFlowDbTestDatabase;
        AsiaTubeManageDbTestDatabase = asiaTubeManageDbTestDatabase;
        AsiaTubeDbTestDatabase = asiaTubeDbTestDatabase;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureTestServices);
    }

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {
        services
            .RemoveAll<AsiaFlowDbContext>()
            .AddDbContext<AsiaFlowDbContext>((sp, options) =>
                options.UseSqlServer(AsiaFlowDbTestDatabase.Connection))
            .RemoveAll<AsiaTubeManageDbContext>()
            .AddDbContext<AsiaTubeManageDbContext>((sp, options) =>
                options.UseSqlServer(AsiaTubeManageDbTestDatabase.Connection))
            .RemoveAll<AsiaTubeDbContext>()
            .AddDbContext<AsiaTubeDbContext>((sp, options) =>
                options.UseSqlServer(AsiaTubeDbTestDatabase.Connection))
            .RemoveAll<IAnonymousRequestSender>()
            .AddTransient<IAnonymousRequestSender, MockAnonymousRequestSender>()
            .RemoveAll<IDbAccessor>()
            .AddTransient<IDbAccessor, MockDbAccessor>()
            .RemoveAll<IAsiaTubeDbSetter>()
            .AddTransient<IAsiaTubeDbSetter, MockAsiaTubeDbSetter>();
    }

    public void ResetDatabase()
    {
        AsiaFlowDbTestDatabase.ResetDatabase();
        AsiaTubeManageDbTestDatabase.ResetDatabase();
        AsiaTubeDbTestDatabase.ResetDatabase();
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public new Task DisposeAsync()
    {
        AsiaFlowDbTestDatabase.Dispose();
        AsiaTubeManageDbTestDatabase.Dispose();
        AsiaTubeDbTestDatabase.Dispose();

        return Task.CompletedTask;
    }
}