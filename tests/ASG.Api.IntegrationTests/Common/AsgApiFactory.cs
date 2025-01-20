using ASG.Application.SubcutaneousTests.Common;
using ASG.Application.SubcutaneousTests.Common.Infrastructure.TestDatabases;

namespace ASG.Api.IntegrationTests.Common;

public class AsgApiFactory : TestFactoryBase<IAssemblyMarker>
{
    public AsgApiFactory() : base(
        AsiaFlowDbTestDatabase.CreateAndInitialize(),
        AsiaTubeManageDbTestDatabase.CreateAndInitialize(),
        AsiaTubeDbTestDatabase.CreateAndInitialize())
    {
    }

    public HttpClient HttpClient { get; private set; } = null!;

    public override Task InitializeAsync()
    {
        HttpClient = CreateClient();
        return base.InitializeAsync();
    }
}