using ASG.Domain.ApolloAttendances;
using ASG.Domain.ApolloAttendances.Enums;
using ASG.Domain.ApolloSyncGaia1001FormOperations;
using ASG.Domain.Gaia1001Forms.Enums;
using FluentAssertions;
using TestCommon.ApolloAttendances;
using TestCommon.TestConstants;

namespace ASG.Domain.UnitTests.ApolloSyncGaia1001FormOperations;

public partial class ApolloSyncGaia1001FormOperationTests
{
    [Fact]
    public void HasOtherEffectiveAttendanceMethod_ReturnsFasle_WhenGaia1001FormStatusIsRecall()
    {
        // Arrange
        var histories = new List<ApolloAttendanceHistory>();
        var forms = new List<Apollo1001Form>();

        // Act
        var result =
            ApolloSyncGaia1001FormOperation.HasOtherEffectiveAttendanceMethod(histories, forms,
                Constants.Gaia1001Forms.DefaultFormNo, Gaia1001FormStatus.Recall);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HasOtherEffectiveAttendanceMethod_ReturnsTrue_WhenHistoryHasOtherMethod()
    {
        // Arrange
        var histories = new List<ApolloAttendanceHistory>
        {
            ApolloAttendanceHistoryFactory.CreateApolloAttendanceHistory()
        };
        var forms = new List<Apollo1001Form>();

        // Act
        var result =
            ApolloSyncGaia1001FormOperation.HasOtherEffectiveAttendanceMethod(histories, forms,
                Constants.Gaia1001Forms.DefaultFormNo, Gaia1001FormStatus.Approved);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasOtherEffectiveAttendanceMethod_ReturnsTrue_WhenFormWithDifferentNumberExists()
    {
        // Arrange
        var histories = new List<ApolloAttendanceHistory>
        {
            ApolloAttendanceHistoryFactory.CreateApolloAttendanceHistory(attendanceMethod: AttendanceMethod.Approval)
        };

        var forms = new List<Apollo1001Form>
        {
            Apollo1001FormFactory.CreateApollo1001Form(formNo: Constants.Gaia1001Forms.DefaultOtherFormNo)
        };

        // Act
        var result =
            ApolloSyncGaia1001FormOperation.HasOtherEffectiveAttendanceMethod(histories, forms,
                Constants.Gaia1001Forms.DefaultFormNo, Gaia1001FormStatus.Approved);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasOtherEffectiveAttendanceMethod_ReturnsFalse_WhenNoEffectiveHistoryOrForm()
    {
        // Arrange
        var histories = new List<ApolloAttendanceHistory>
        {
            ApolloAttendanceHistoryFactory.CreateApolloAttendanceHistory(isEffective: false)
        };
        var forms = new List<Apollo1001Form>();

        // Act
        var result =
            ApolloSyncGaia1001FormOperation.HasOtherEffectiveAttendanceMethod(histories, forms,
                Constants.Gaia1001Forms.DefaultFormNo, Gaia1001FormStatus.Approved);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HasOtherEffectiveAttendanceMethod_ReturnsFalse_WhenOnlyCurrentFormExists()
    {
        // Arrange
        var histories = new List<ApolloAttendanceHistory>();
        var forms = new List<Apollo1001Form>
        {
            new() { ApprovalStatus = Apollo1001ApprovalStatus.Unknown, FormNo = Constants.Gaia1001Forms.DefaultFormNo }
        };
        // Act
        var result =
            ApolloSyncGaia1001FormOperation.HasOtherEffectiveAttendanceMethod(histories, forms,
                Constants.Gaia1001Forms.DefaultFormNo, Gaia1001FormStatus.Approved);

        // Assert
        result.Should().BeFalse();
    }
}