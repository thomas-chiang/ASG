using ASG.Application.ApolloAttendances.Queries.GetGaia1001FormWithApolloAttendance;
using ASG.Application.Gaia1001Forms.Queries.GetGaia1001Form;
using ASG.Application.SubcutaneousTests.Common;
using FluentAssertions;
using MediatR;
using TestCommon.ApolloSyncGaia1001FormOperations;
using TestCommon.TestConstants;

namespace ASG.Application.SubcutaneousTests.ApolloSyncGaia1001FormOperations.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class CreateApolloSyncGaia1001FormOperationTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    [Fact]
    public async Task
        CreateApolloSyncGaia1001FormOperation_WhenValidCommand_ShouldCreateApolloSyncGaia1001FormOperation()
    {
        // Arrange
        var createApolloSyncGaia1001FormOperationCommand = ApolloSyncGaia1001FormOperationCommandFactory
            .CreateCreateApolloSyncGaia1001FormOperationCommand();

        // Act
        var createApolloSyncGaia1001FormOperationResult =
            await _mediator.Send(createApolloSyncGaia1001FormOperationCommand);

        // Assert
        createApolloSyncGaia1001FormOperationResult.IsError.Should().BeFalse();
        createApolloSyncGaia1001FormOperationResult.Value.Situation.Should().BeNull();
    }

    [Theory]
    [InlineData("1001")]
    [InlineData("9.001")]
    [InlineData("001")]
    public async Task
        CreateApolloSyncGaia1001FormOperation_WhenCommandContainsInvalidFormKind_ShouldReturnValidationError(
            string formKind)
    {
        // Arrange
        var createApolloSyncGaia1001FormOperationCommand = ApolloSyncGaia1001FormOperationCommandFactory
            .CreateCreateApolloSyncGaia1001FormOperationCommand(formKind);

        // Act
        var createApolloSyncGaia1001FormOperationResult =
            await _mediator.Send(createApolloSyncGaia1001FormOperationCommand);

        // Assert
        createApolloSyncGaia1001FormOperationResult.IsError.Should().BeTrue();
        createApolloSyncGaia1001FormOperationResult.FirstError.Code.Should().Be("FormKind");
    }
}