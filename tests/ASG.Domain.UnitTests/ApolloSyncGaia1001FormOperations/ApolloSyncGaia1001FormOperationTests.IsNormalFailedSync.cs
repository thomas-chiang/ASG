using System.Reflection;
using ASG.Domain.ApolloAttendances;
using ASG.Domain.ApolloAttendances.Enums;
using ASG.Domain.ApolloSyncGaia1001FormOperations;
using ASG.Domain.Gaia1001Forms;
using ASG.Domain.Gaia1001Forms.Enums;
using FluentAssertions;
using TestCommon.ApolloAttendances;
using TestCommon.Gaia1001Forms;

namespace ASG.Domain.UnitTests.ApolloSyncGaia1001FormOperations;

public partial class ApolloSyncGaia1001FormOperationTests
{
    [Theory]
    [InlineData(Apollo1001ApprovalStatus.Ok, Gaia1001FormStatus.Approved, true)]
    [InlineData(Apollo1001ApprovalStatus.Unknown, Gaia1001FormStatus.WaitingApprove, true)]
    [InlineData(Apollo1001ApprovalStatus.Unknown, Gaia1001FormStatus.UnderApproving, true)]
    [InlineData(Apollo1001ApprovalStatus.Deny, Gaia1001FormStatus.Rejected, true)]
    [InlineData(Apollo1001ApprovalStatus.Delete, Gaia1001FormStatus.Deleted, true)]
    [InlineData(Apollo1001ApprovalStatus.Ok, Gaia1001FormStatus.Rejected, false)]
    public void IsNormalFailSync_ValidatesConditionsCorrectly(
        Apollo1001ApprovalStatus apollo1001ApprovalStatus,
        Gaia1001FormStatus gaia1001FormStatus,
        bool expectedResult)
    {
        // Arrange
        var operation = new ApolloSyncGaia1001FormOperation
        {
            Gaia1001Form = Gaia1001FormFactory.CreateGaia1001Form(gaia1001FormStatus: gaia1001FormStatus),
            ApolloAttendance = null!
        };
        var updatedApollo1001Form =
            Apollo1001FormFactory.CreateApollo1001Form(approvalStatus: apollo1001ApprovalStatus);

        // Use reflection to access the private method
        var methodInfo = typeof(ApolloSyncGaia1001FormOperation).GetMethod(
            "IsNormalFailedSync",
            BindingFlags.NonPublic | BindingFlags.Instance);

        Assert.NotNull(methodInfo); // Ensure the method exists

        // Act
        var result = (bool)methodInfo.Invoke(operation, new object[] { updatedApollo1001Form })!;

        // Assert
        result.Should().Be(expectedResult,
            $"apollo1001ApprovalStatus: {apollo1001ApprovalStatus}, gaia1001FormStatus: {gaia1001FormStatus}");
    }
}