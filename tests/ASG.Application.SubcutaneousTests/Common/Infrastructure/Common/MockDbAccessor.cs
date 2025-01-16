using ASG.Application.Common.Interfaces;

namespace ASG.Application.SubcutaneousTests.Common.Infrastructure.Common;

public class MockDbAccessor : IDbAccessor
{
    public Task GainAccess()
    {
        return Task.CompletedTask;
    }
}