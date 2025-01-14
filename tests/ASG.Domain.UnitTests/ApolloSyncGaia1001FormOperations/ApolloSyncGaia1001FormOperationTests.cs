using ASG.Domain.ApolloSyncGaia1001FormOperations;
using FluentAssertions;
using TestCommon.ApolloAttendances;
using TestCommon.Gaia1001Forms;

namespace ASG.Domain.UnitTests.ApolloSyncGaia1001FormOperations;

public class ApolloSyncGaia1001FormOperationTests
{
    [Fact]
    public void SetSituationWhenNoOtherEffectiveAttendanceMethodAndNoUpdatedApolloAttendance_ShouldFail()
    {
        // Arrange
        var gaia1001Form = Gaia1001FormFactory.CreateGaia1001Form();
        var apolloAttendance = ApolloAttendanceFactory.CreateApolloAttendance();
        var apolloSyncGaia1001FormOperation = new ApolloSyncGaia1001FormOperation
        {
            Gaia1001Form = gaia1001Form,
            ApolloAttendance = apolloAttendance,
        };
        
        // Act
        var setSituationResult = apolloSyncGaia1001FormOperation.SetSituation();

        // Assert
        setSituationResult.IsError.Should().BeTrue();
        setSituationResult.FirstError.Should()
            .Be(ApolloSyncGaia1001FormOperationErrors.ApolloAttendanceNotFetchedAgain);
    }
}