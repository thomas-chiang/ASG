using ASG.Api;
using ASG.Application.Common.Interfaces;
using ASG.Application.SubcutaneousTests.Common.Infrastructure.Common;
using ASG.Application.SubcutaneousTests.Common.Infrastructure.Common.SqlServerDbContexts;
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
    private AsiaFlowDbTestDatabase _asiaFlowDbTestDatabase = null!;
    private AsiaTubeManageDbTestDatabase _asiaTubeManageDbTestDatabase = null!;
    private AsiaTubeDbTestDatabase _asiaTubeDbTestDatabase = null!;
    

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _asiaFlowDbTestDatabase = AsiaFlowDbTestDatabase.CreateAndInitialize();
        _asiaTubeManageDbTestDatabase = AsiaTubeManageDbTestDatabase.CreateAndInitialize();
        _asiaTubeDbTestDatabase = AsiaTubeDbTestDatabase.CreateAndInitialize();

        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<DbContextOptions<AsiaFlowDbContext>>()
                .AddDbContext<AsiaFlowDbContext>((sp, options) => options.UseSqlServer(_asiaFlowDbTestDatabase.Connection))
                
                .RemoveAll<DbContextOptions<AsiaTubeManageDbContext>>()
                .AddDbContext<AsiaTubeManageDbContext>((sp, options) => options.UseSqlServer(_asiaTubeManageDbTestDatabase.Connection))
                .RemoveAll<AsiaTubeManageDbContext>()
                .AddTransient(serviceProvider => MockAsiaTubeManageDbContext.CreateMock().Object)
                
                .RemoveAll<IAnonymousRequestSender>()
                .AddTransient<IAnonymousRequestSender, MockAnonymousRequestSender>()
                
                .RemoveAll<IDbAccessor>()
                .AddTransient<IDbAccessor, MockDbAccessor>()
                ;
                
        });
    }

    public IMediator CreateMediator()
    {
        var serviceScope = Services.CreateScope();

        _asiaFlowDbTestDatabase.ResetDatabase();
        _asiaTubeManageDbTestDatabase.ResetDatabase();
        _asiaTubeDbTestDatabase.ResetDatabase();

        return serviceScope.ServiceProvider.GetRequiredService<IMediator>();
    }


    public Task InitializeAsync() => Task.CompletedTask;

    public new Task DisposeAsync()
    {
        _asiaFlowDbTestDatabase.Dispose();
        _asiaTubeManageDbTestDatabase.Dispose();
        _asiaTubeDbTestDatabase.Dispose();

        return Task.CompletedTask;
    }
}