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

public class MediatorFactory : TestFactoryBase<IAssemblyMarker>
{
    public MediatorFactory() : base(
        AsiaFlowDbTestDatabase.CreateAndInitialize(),
        AsiaTubeManageDbTestDatabase.CreateAndInitialize(),
        AsiaTubeDbTestDatabase.CreateAndInitialize())
    {
    }

    public IMediator CreateMediator()
    {
        var serviceScope = Services.CreateScope();
        ResetDatabase();
        return serviceScope.ServiceProvider.GetRequiredService<IMediator>();
    }
}