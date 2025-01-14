using ASG.Domain.ApolloSyncGaia1001FormOperations;
using FluentAssertions;
using TestCommon.ApolloSyncGaia1001FormOperations;

namespace ASG.Domain.UnitTests.ApolloSyncGaia1001FormOperations;

public class ApolloSyncGaia1001FormOperationTests
{
    [Fact]
    public void SetSituationWhenNoOtherEffectiveAttendanceMethodAndNoUpdatedApolloAttendance_ShouldFail()
    {
        // Arrange
        var apolloSyncGaia1001FormOperation = ApolloSyncGaia1001FormOperationFactory.CreateApolloSyncGaia1001FormOperation();
        
        // Act
        var setSituationResult = apolloSyncGaia1001FormOperation.SetSituation();

        // Assert
        setSituationResult.IsError.Should().BeTrue();
        setSituationResult.FirstError.Should()
            .Be(ApolloSyncGaia1001FormOperationErrors.ApolloAttendanceNotFetchedAgain);
    }
}