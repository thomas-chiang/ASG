using ASG.Application.ApolloAttendances.Queries.GetApolloAttendance;
using ASG.Application.ApolloSyncActions.Commands.CreateApolloSyncAction;
using ASG.Contracts.ApolloAttendances;
using ASG.Contracts.ApolloSyncActions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ASG.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ApolloAttendanceController : ControllerBase
{
    private readonly ISender _mediator;

    public ApolloAttendanceController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(GetApolloAttendanceResponse), 200)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    public async Task<IActionResult> GetApolloAttendance([FromQuery] GetApolloAttendanceRequest request)
    {

        var query = new GetApolloAttendanceQuery (request.FormKind, request.FormNo);
        
        var getApolloAttendanceResult = await _mediator.Send(query);
        
        return getApolloAttendanceResult.MatchFirst(
            apolloAttendance => Ok(new GetApolloAttendanceResponse {
                CompanyId = apolloAttendance.CompanyId,
                UserEmployeeId = apolloAttendance.UserEmployeeId,
                ApolloAttendanceHistories = apolloAttendance.ApolloAttendanceHistories?.Select(history => 
                    new ApolloAttendanceHistoryResponse(
                        history.AttendanceOn,
                        history.AttendanceMethod.ToString(),
                        history.IsEffective
                    )
                ).ToList(),
                Apollo1001FromResponses = null
            }),
            error => Problem()
        );
    }
}