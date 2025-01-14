using ASG.Application.SubcutaneousTests.Common;
using FluentAssertions;
using MediatR;
using TestCommon.ApolloSyncGaia1001FormOperations;

namespace ASG.Application.SubcutaneousTests.ApolloSyncGaia1001FormOperations.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class CreateApolloSyncGaia1001FormOperationTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();
    
    [Fact]
    public async Task CreateApolloSyncGaia1001FormOperation_WhenValidCommand_ShouldCreateApolloSyncGaia1001FormOperation()
    {
        // Arrange
        var createApolloSyncGaia1001FormOperationCommand = ApolloSyncGaia1001FormOperationCommandFactory.CreateCreateApolloSyncGaia1001FormOperationCommand();
        
        // Act
        var createApolloSyncGaia1001FormOperationResult = await _mediator.Send(createApolloSyncGaia1001FormOperationCommand);
        
        // Assert
        createApolloSyncGaia1001FormOperationResult.IsError.Should().BeFalse();
        createApolloSyncGaia1001FormOperationResult.Value.UpdatedApolloAttendance.Should().BeNull();
    }
}