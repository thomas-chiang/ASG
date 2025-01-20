using ASG.Api;
using ASG.Application.Common.Interfaces;
using ASG.Application.SubcutaneousTests.Common.Infrastructure.Common;
using ASG.Application.SubcutaneousTests.Common.Infrastructure.TestDatabases;
using ASG.Infrastructure.Common.SqlServerDbContexts;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASG.Application.SubcutaneousTests.Common;

public class MediatorFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    public AsiaFlowDbTestDatabase AsiaFlowDbTestDatabase = null!;
    public AsiaTubeManageDbTestDatabase AsiaTubeManageDbTestDatabase = null!;
    public AsiaTubeDbTestDatabase AsiaTubeDbTestDatabase = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        AsiaFlowDbTestDatabase = AsiaFlowDbTestDatabase.CreateAndInitialize();
        AsiaTubeManageDbTestDatabase = AsiaTubeManageDbTestDatabase.CreateAndInitialize();
        AsiaTubeDbTestDatabase = AsiaTubeDbTestDatabase.CreateAndInitialize();

        builder.ConfigureTestServices(services =>
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
                .AddTransient<IAsiaTubeDbSetter, MockAsiaTubeDbSetter>()
                ;
        });
    }

    public IMediator CreateMediator()
    {
        var serviceScope = Services.CreateScope();

        AsiaFlowDbTestDatabase.ResetDatabase();
        AsiaTubeManageDbTestDatabase.ResetDatabase();
        AsiaTubeDbTestDatabase.ResetDatabase();

        return serviceScope.ServiceProvider.GetRequiredService<IMediator>();
    }
    
    public Task InitializeAsync()
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