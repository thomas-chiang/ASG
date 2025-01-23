using ASG.Domain.ApolloAttendances;
using ASG.Domain.ApolloAttendances.Enums;
using ASG.Domain.ApolloSyncGaia1001FormOperations;
using FluentAssertions;
using TestCommon.ApolloAttendances;
using TestCommon.ApolloSyncGaia1001FormOperations;
using TestCommon.TestConstants;

namespace ASG.Domain.UnitTests.ApolloSyncGaia1001FormOperations;

public partial class ApolloSyncGaia1001FormOperationTests
{
    [Fact]
    public void SetSituation_WhenNoOtherEffectiveAttendanceMethodAndNoUpdatedApolloAttendance_ShouldFail()
    {
        // Arrange
        var apolloSyncGaia1001FormOperation =
            ApolloSyncGaia1001FormOperationFactory.CreateApolloSyncGaia1001FormOperation();

        // Act
        var setSituationResult = apolloSyncGaia1001FormOperation.SetSituation(null);

        // Assert
        setSituationResult.IsError.Should().BeTrue();
        setSituationResult.FirstError.Should()
            .Be(ApolloSyncGaia1001FormOperationErrors.ApolloAttendanceNotFetchedAgain);
    }
}