// using ASG.Domain.ApolloAttendances;
// using ASG.Domain.ApolloAttendances.Enums;
// using ASG.Domain.ApolloSyncGaia1001FormOperations;
// using ASG.Domain.ApolloSyncGaia1001FormOperations.Enums;
// using ASG.Domain.Gaia1001Forms.Enums;
// using FluentAssertions;
// using TestCommon.ApolloAttendances;
// using TestCommon.ApolloSyncGaia1001FormOperations;
// using TestCommon.Gaia1001Forms;
// using TestCommon.TestConstants;
//
// namespace ASG.Domain.UnitTests.ApolloSyncGaia1001FormOperations;
//
// public partial class ApolloSyncGaia1001FormOperationTests
// {
//     [Fact]
//     public void SetSituation_WhenNoOtherEffectiveAttendanceMethodAndNoUpdatedApolloAttendance_ShouldFail()
//     {
//         // Arrange
//         var apolloSyncGaia1001FormOperation =
//             ApolloSyncGaia1001FormOperationFactory.CreateApolloSyncGaia1001FormOperation();
//         
//         // Act
//         var setSituationResult = apolloSyncGaia1001FormOperation.SetSituationWithExistedEffectiveAttendance(null);
//
//         // Assert
//         // setSituationResult.IsError.Should().BeTrue();
//         // setSituationResult.FirstError.Should()
//         //     .Be(ApolloSyncGaia1001FormOperationErrors.ApolloAttendanceNotFetchedAgain);
//     }
//     
//     [Fact]
//     public void SetSituation_SetsSituationToNeedFurtherInvestigation_WhenUpdatedApollo1001FormIsNull()
//     {
//         // Arrange
//         var operation = ApolloSyncGaia1001FormOperationFactory.CreateApolloSyncGaia1001FormOperation();
//         // operation.UpdatedApolloAttendance = ApolloAttendanceFactory.CreateApolloAttendance();
//         
//
//         // Act
//         var result = operation.SetSituationWithExistedEffectiveAttendance(null);
//
//         // Assert
//         result.IsError.Should().BeFalse();
//         operation.Situation.Should().Be(Sync1001FormSituation.NeedFurtherInvestigation);
//     }
//
//     [Fact]
//     public void SetSituation_SetsSituationToNormalFailedSync_WhenIsNormalFailedSyncReturnsTrue()
//     {
//         // Arrange
//         var updatedApollo1001Form = Apollo1001FormFactory.CreateApollo1001Form(approvalStatus: Apollo1001ApprovalStatus.Ok);
//         var operation = ApolloSyncGaia1001FormOperationFactory.CreateApolloSyncGaia1001FormOperation(
//             gaia1001Form: Gaia1001FormFactory.CreateGaia1001Form(gaia1001FormStatus: Gaia1001FormStatus.Approved)
//             );
//         // operation.UpdatedApolloAttendance = ApolloAttendanceFactory.CreateApolloAttendance();
//     
//         // Act
//         var result = operation.SetSituationWithExistedEffectiveAttendance(updatedApollo1001Form);
//     
//         // Assert
//         result.IsError.Should().BeFalse();
//         operation.Situation.Should().Be(Sync1001FormSituation.NormalFailedSync);
//     }
//     
//     [Fact]
//     public void SetSituation_SetsSituationToNeedFurtherInvestigation_WhenIsNormalFailedSyncReturnsFalse()
//     {
//         // Arrange
//         var updatedApollo1001Form = Apollo1001FormFactory.CreateApollo1001Form(approvalStatus: Apollo1001ApprovalStatus.Ok);
//         var operation = ApolloSyncGaia1001FormOperationFactory.CreateApolloSyncGaia1001FormOperation(
//             gaia1001Form: Gaia1001FormFactory.CreateGaia1001Form(gaia1001FormStatus: Gaia1001FormStatus.Rejected)
//         );
//         // operation.UpdatedApolloAttendance = ApolloAttendanceFactory.CreateApolloAttendance();
//     
//         // Act
//         var result = operation.SetSituationWithExistedEffectiveAttendance(updatedApollo1001Form);
//     
//         // Assert
//         result.IsError.Should().BeFalse();
//         operation.Situation.Should().Be(Sync1001FormSituation.NeedFurtherInvestigation);
//     }
// }

