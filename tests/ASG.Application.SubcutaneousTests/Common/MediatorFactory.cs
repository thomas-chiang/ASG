using ASG.Api;
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
    private InMemoryTestDatabase _testDatabase = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _testDatabase = InMemoryTestDatabase.CreateAndInitialize();

        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<DbContextOptions<AsiaFlowDbContext>>()
                .AddDbContext<AsiaFlowDbContext>((sp, options) => options.UseInMemoryDatabase(_testDatabase.InMemoryDatabaseName));
        });
    }

    public IMediator CreateMediator()
    {
        var serviceScope = Services.CreateScope();

        _testDatabase.ResetDatabase();

        return serviceScope.ServiceProvider.GetRequiredService<IMediator>();
    }


    public Task InitializeAsync() => Task.CompletedTask;

    public new Task DisposeAsync()
    {
        _testDatabase.Dispose();

        return Task.CompletedTask;
    }
}