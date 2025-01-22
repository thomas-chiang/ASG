using ASG.Api;
using ASG.Application.SubcutaneousTests.Common.Infrastructure.TestDatabases;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


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