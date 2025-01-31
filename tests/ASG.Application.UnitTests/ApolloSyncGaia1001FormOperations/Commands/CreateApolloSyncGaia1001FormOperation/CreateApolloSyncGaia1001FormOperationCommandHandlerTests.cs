using NSubstitute;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using ASG.Application.ApolloSyncGaia1001FormOperations.Commands.CreateApolloSyncGaia1001FormOperation;
using ASG.Application.Gaia1001Forms.Interfaces;
using ASG.Application.ApolloAttendances.Interfaces;
using ASG.Application.Common.Interfaces;
using ASG.Domain.ApolloSyncGaia1001FormOperations.Enums;
using ASG.Domain.Gaia1001Forms;
using TestCommon.ApolloAttendances;
using TestCommon.Gaia1001Forms;
using TestCommon.TestConstants;


namespace ASG.Application.UnitTests.ApolloSyncGaia1001FormOperations.Commands.CreateApolloSyncGaia1001FormOperation;

public class CreateApolloSyncGaia1001FormOperationCommandHandlerTests
{
    private readonly IGaia1001FormRepository _gaia1001FormRepository;
    private readonly IApolloAttendanceRepository _apolloAttendanceRepository;
    private readonly IAnonymousRequestSender _anonymousRequestSender;
    private readonly IAsiaTubeDbSetter _asiaTubeDbSetter;
    private readonly IDomainEventAdapter _domainEventAdapter;
    private readonly CreateApolloSyncGaia1001FormOperationCommandHandler _handler;

    public CreateApolloSyncGaia1001FormOperationCommandHandlerTests()
    {
        _gaia1001FormRepository = Substitute.For<IGaia1001FormRepository>();
        _apolloAttendanceRepository = Substitute.For<IApolloAttendanceRepository>();
        _anonymousRequestSender = Substitute.For<IAnonymousRequestSender>();
        _asiaTubeDbSetter = Substitute.For<IAsiaTubeDbSetter>();
        _domainEventAdapter = Substitute.For<IDomainEventAdapter>();

        _handler = new CreateApolloSyncGaia1001FormOperationCommandHandler(
            _apolloAttendanceRepository,
            _gaia1001FormRepository,
            _anonymousRequestSender,
            _asiaTubeDbSetter,
            _domainEventAdapter);
    }

    [Fact]
    public async Task Handle_Gaia1001FormNotFound_ReturnsError()
    {
        // Arrange
        var command = new CreateApolloSyncGaia1001FormOperationCommand("FormKindA", 123);
        _gaia1001FormRepository.GetGaia1001Form(command.FormKind, command.FormNo).Returns((Gaia1001Form)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Code == "Gaia1001Form.NotFound");
    }

    [Fact]
    public async Task Handle_CallsSetAsiaTubeDb_WhenGaia1001FormExists()
    {
        // Arrange
        var command = new CreateApolloSyncGaia1001FormOperationCommand(
            Constants.Gaia1001Forms.DefaultFormKind,
            Constants.Gaia1001Forms.DefaultFormNo);
        var gaia1001Form = Gaia1001FormFactory.CreateGaia1001Form();
        _gaia1001FormRepository.GetGaia1001Form(command.FormKind, command.FormNo).Returns(gaia1001Form);
        _apolloAttendanceRepository.GetApolloAttendance(
            gaia1001Form.CompanyId,
            gaia1001Form.UserEmployeeId,
            DateOnly.FromDateTime(gaia1001Form.AttendanceOn),
            gaia1001Form.AttendanceType
        ).Returns(ApolloAttendanceFactory.CreateApolloAttendance());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _asiaTubeDbSetter.Received(1).SetAsiaTubeDb(gaia1001Form.CompanyId);
    }

    [Fact]
    public async Task
        Handle_SetsUpdatedApolloAttendance_WhenNoExistedEffectiveAttendanceAndFailedApolloSyncGaia1001FormOperation()
    {
        // Arrange
        var command = new CreateApolloSyncGaia1001FormOperationCommand("FormKindA", 123);
        var gaia1001Form = Gaia1001FormFactory.CreateGaia1001Form();
        var initialApolloAttendance = ApolloAttendanceFactory.CreateApolloAttendance();
        var updatedApolloAttendance = ApolloAttendanceFactory.CreateApolloAttendance();
        _gaia1001FormRepository.GetGaia1001Form(command.FormKind, command.FormNo).Returns(gaia1001Form);
        _apolloAttendanceRepository.GetApolloAttendance(
                gaia1001Form.CompanyId,
                gaia1001Form.UserEmployeeId,
                DateOnly.FromDateTime(gaia1001Form.AttendanceOn),
                gaia1001Form.AttendanceType)
            .Returns(initialApolloAttendance, updatedApolloAttendance);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.UpdatedApolloAttendance.Should().Be(updatedApolloAttendance);
        result.Value.Situation.Should().Be(Sync1001FormSituation.NeedFurtherInvestigation);
    }
}