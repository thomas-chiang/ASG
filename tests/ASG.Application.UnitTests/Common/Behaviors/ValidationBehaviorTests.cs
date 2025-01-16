using ErrorOr;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using ASG.Application.Common.Behaviors;
using MediatR;
using NSubstitute;
using TestCommon.ApolloSyncGaia1001FormOperations;

// Aliases for long type names
using ReqType =
    ASG.Application.ApolloSyncGaia1001FormOperations.Commands.CreateApolloSyncGaia1001FormOperation.
    CreateApolloSyncGaia1001FormOperationCommand;
using ResType = ASG.Domain.ApolloSyncGaia1001FormOperations.ApolloSyncGaia1001FormOperation;

namespace ASG.Application.UnitTests.Common.Behaviors;

public class ValidationBehaviorTests
{
    private readonly ValidationBehavior<ReqType, ErrorOr<ResType>> _validationBehavior;
    private readonly IValidator<ReqType> _mockValidator;
    private readonly RequestHandlerDelegate<ErrorOr<ResType>> _mockNextBehavior;

    public ValidationBehaviorTests()
    {
        // Create a next behavior (mock)
        _mockNextBehavior = Substitute.For<RequestHandlerDelegate<ErrorOr<ResType>>>();

        // Create validator (mock)
        _mockValidator = Substitute.For<IValidator<ReqType>>();

        // Create validation behavior (SUT: System Under Test)
        _validationBehavior = new ValidationBehavior<ReqType, ErrorOr<ResType>>(_mockValidator);
    }

    [Fact]
    public async Task InvokeBehavior_WhenValidatorResultIsValid_ShouldInvokeNextBehavior()
    {
        // Arrange
        var request = ApolloSyncGaia1001FormOperationCommandFactory
            .CreateCreateApolloSyncGaia1001FormOperationCommand();
        var response = ApolloSyncGaia1001FormOperationFactory.CreateApolloSyncGaia1001FormOperation();

        _mockValidator
            .ValidateAsync(request, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult() // ValidationResult from FluentValidation by default .IsValid is true
            ); // .Returns from NSubstitute

        _mockNextBehavior.Invoke().Returns(response); // .Returns from NSubstitute 

        // Act
        var result = await _validationBehavior.Handle(request, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task InvokeBehavior_WhenValidatorResultIsNotValid_ShouldReturnListOfErrors()
    {
        // Arrange
        var request = ApolloSyncGaia1001FormOperationCommandFactory
            .CreateCreateApolloSyncGaia1001FormOperationCommand();
        List<ValidationFailure> validationFailures = [new("foo", "bad foo")];

        _mockValidator
            .ValidateAsync(request, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));

        // Act
        var result = await _validationBehavior.Handle(request, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("foo");
        result.FirstError.Description.Should().Be("bad foo");
    }
}