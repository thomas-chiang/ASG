using ASG.Infrastructure.Common.SqlServerDbContexts;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ASG.Application.SubcutaneousTests.Common.Infrastructure.Common.SqlServerDbContexts;

public class MockAsiaTubeManageDbContext 
{
    public static Mock<AsiaTubeManageDbContext> CreateMock()
    {
        var mockDbContext = new Mock<AsiaTubeManageDbContext>(
            new DbContextOptions<AsiaTubeManageDbContext>()) { CallBase = true };

        // Mock the GetCompanyDbConnectionString method
        mockDbContext
            .Setup(db => db.GetCompanyDbConnectionString(It.IsAny<Guid>()))
            .ReturnsAsync((Guid companyId) =>
            {
                return "Server=localhost,1433;User=sa;Password=YourStrong@Password;Database=AsiaTubeDbTestDatabase;Encrypt=False;";
            });

        return mockDbContext;
    }
}