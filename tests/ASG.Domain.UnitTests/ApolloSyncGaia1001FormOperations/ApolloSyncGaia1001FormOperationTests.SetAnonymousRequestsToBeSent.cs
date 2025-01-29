using ASG.Domain.ApolloAttendances;
using ASG.Domain.ApolloAttendances.Enums;
using ASG.Domain.ApolloSyncGaia1001FormOperations;
using ASG.Domain.ApolloSyncGaia1001FormOperations.Requests;
using ASG.Domain.Common.Enums;
using ASG.Domain.Gaia1001Forms;
using ASG.Domain.Gaia1001Forms.Enums;
using FluentAssertions;
using TestCommon.ApolloAttendances;
using TestCommon.ApolloSyncGaia1001FormOperations;
using TestCommon.Gaia1001Forms;

namespace ASG.Domain.UnitTests.ApolloSyncGaia1001FormOperations;

public partial class ApolloSyncGaia1001FormOperationTests
{
    [Fact]
    public void
        SetAnonymousRequestsToBeSent_ShouldAppendCreateAndApproveRequest_WhenFormIsApprovedAndNoMatchingFormExists()
    {
        // Arrange
        var operation =
            ApolloSyncGaia1001FormOperationFactory.CreateApolloSyncGaia1001FormOperation();

        // Act
        operation.SetAnonymousRequestsToBeSent(null);

        // Assert
        operation.AnonymousRequests.Should().HaveCount(1);
        var request = operation.AnonymousRequests.First();
        request.Should().BeOfType<ApplyReCheckInFormRequest>();
    }

    [Fact]
    public void
        SetAnonymousRequestsToBeSent_ShouldAppendApproveRequest_WhenFormIsApprovedAndMatchingFormHasUnknownApprovalStatus()
    {
        // Arrange
        var operation =
            ApolloSyncGaia1001FormOperationFactory.CreateApolloSyncGaia1001FormOperation();
        var matchingForm = Apollo1001FormFactory.CreateApollo1001Form(approvalStatus: Apollo1001ApprovalStatus.Unknown);

        // Act
        operation.SetAnonymousRequestsToBeSent(matchingForm);

        // Assert
        operation.AnonymousRequests.Should().HaveCount(1);
        var request = operation.AnonymousRequests.First();
        request.Should().BeOfType<ApproveReCheckInFormRequest>();
    }

    [Fact]
    public void
        SetAnonymousRequestsToBeSent_ShouldAppendRecallRequest_WhenFormIsDeletedAndMatchingFormHasOkApprovalStatus()
    {
        // Arrange
        var operation = ApolloSyncGaia1001FormOperationFactory.CreateApolloSyncGaia1001FormOperation(
            Gaia1001FormFactory.CreateGaia1001Form(gaia1001FormStatus: Gaia1001FormStatus.Deleted)
        );
        var matchingForm = Apollo1001FormFactory.CreateApollo1001Form(approvalStatus: Apollo1001ApprovalStatus.Ok);

        // Act
        operation.SetAnonymousRequestsToBeSent(matchingForm);

        // Assert
        operation.AnonymousRequests.Should().HaveCount(1);
        var request = operation.AnonymousRequests.First();
        request.Should().BeOfType<RecalledReCheckInFormRequest>();
    }

    [Fact]
    public void SetAnonymousRequestsToBeSent_ShouldAppendFailedRequests_WhenNoConditionMatches()
    {
        // Arrange
        var operation = ApolloSyncGaia1001FormOperationFactory.CreateApolloSyncGaia1001FormOperation(
            Gaia1001FormFactory.CreateGaia1001Form(gaia1001FormStatus: Gaia1001FormStatus.WaitingApprove)
        );
        var matchingForm = Apollo1001FormFactory.CreateApollo1001Form(approvalStatus: Apollo1001ApprovalStatus.Unknown);
        // Act
        operation.SetAnonymousRequestsToBeSent(matchingForm);

        // Assert
        operation.AnonymousRequests.Should().BeEmpty();
    }
}