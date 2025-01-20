using ASG.Application.Common.Interfaces;

namespace ASG.Application.SubcutaneousTests.Common.Infrastructure.Common;

public class MockAsiaTubeDbSetter : IAsiaTubeDbSetter
{
    public Task SetAsiaTubeDb(Guid companyId)
    {
        return Task.CompletedTask;
    }
}