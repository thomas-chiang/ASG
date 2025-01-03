using ASG.Application.ApolloAttendances.Queries.GetGaia1001FormWithApolloAttendance;
using ASG.Contracts.ApolloAttendences;
using ASG.Contracts.Gaia1001Forms;
using ASG.Contracts.Gaia1001FormWithApolloAttendances;
using ASG.Domain.ApolloAttendances;
using ASG.Domain.Gaia1001Forms;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASG.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class Gaia1001FormWithApolloAttendanceController : ControllerBase
{
    private readonly ISender _mediator;

    public Gaia1001FormWithApolloAttendanceController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetGaia1001FormWithApolloAttendanceResponse), 200)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    public async Task<IActionResult> GetGaia1001FormWithApolloAttendance(
        [FromQuery] GetGaia1001FormWithApolloAttendanceRequest request)
    {
        var query = new GetGaia1001FormWithApolloAttendanceQuery(request.FormKind, request.FormNo);

        var getGaia1001FormWithApolloAttendanceQueryResult = await _mediator.Send(query);

        return getGaia1001FormWithApolloAttendanceQueryResult.MatchFirst(
            result => Ok(
                new GetGaia1001FormWithApolloAttendanceResponse(
                    Gaia1001FormController.ToGetGaia1001FormResponse(result.Gaia1001Form),
                    ToApolloAttendanceResponse(result.ApolloAttendance)
                )
            ),
            error => Problem()
        );
    }

    public static ApolloAttendanceResponse ToApolloAttendanceResponse(ApolloAttendance apolloAttendance)
    {
        return new ApolloAttendanceResponse
        {
            CompanyId = apolloAttendance.CompanyId,
            UserEmployeeId = apolloAttendance.UserEmployeeId,
            AttendanceDate = apolloAttendance.AttendanceDate,
            AttendanceType = apolloAttendance.AttendanceType.ToString(),
            ApolloAttendanceHistories = apolloAttendance.ApolloAttendanceHistories?.Select(history =>
                new ApolloAttendanceHistoryResponse(
                    history.AttendanceHistoryId.ToString(),
                    history.AttendanceOn,
                    history.AttendanceMethod.ToString(),
                    history.IsEffective
                )
            ).ToList(),
            Apollo1001Froms = apolloAttendance.Apollo1001Forms?.Select(form =>
                new Apollo1001FromResponse(
                    form.AttendanceHistoryId.ToString(),
                    form.FormKind,
                    form.FormNo,
                    form.ApprovalStatus.ToString()
                )
            ).ToList()
        };
    }
}