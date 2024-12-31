using ASG.Application.ApolloAttendances.Queries.GetGaia1001FormWithApolloAttendance;
using ASG.Contracts.ApolloAttendences;
using ASG.Contracts.Gaia1001Forms;
using ASG.Contracts.Gaia1001FormWithApolloAttendances;
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
                    new GetGaia1001FormResponse
                    {
                        FormKind = result.gaia1001Form.FormKind,               
                        FormNo = result.gaia1001Form.FormNo,                 
                        CompanyId = result.gaia1001Form.CompanyId,              
                        UserEmployeeId = result.gaia1001Form.UserEmployeeId,         
                        PtSyncFormOperations = result.gaia1001Form.PtSyncFormOperations?.Select(operation => new PTSyncFormOperationResponse
                        {
                            FormContent = operation.FormContent,
                            FormAction = operation.FormAction.ToString(),
                            CreatedOn = operation.CreatedOn,
                            ModifiedOn = operation.ModifiedOn,
                            Flag = operation.Flag.ToString(),
                            RetryCount = operation.RetryCount,
                            ApplyReCheckInFormJson = operation.ApplyReCheckInFormRequestBody != null
                                ? JsonConvert.DeserializeObject<ApplyReCheckInFormJsonResponse>(operation.FormContent)
                                : null,
                            ApproveReCheckInFormJson = operation.ApproveReCheckInFormRequestBody != null
                                ? JsonConvert.DeserializeObject<ApproveReCheckInFormJsonResponse>(operation.FormContent)
                                : null,
                            RecalledReCheckInFormJson = operation.RecalledReCheckInFormRequestBody != null
                                ? JsonConvert.DeserializeObject<RecalledReCheckInFormJsonResponse>(operation.FormContent)
                                : null
                        }).ToList() ?? new List<PTSyncFormOperationResponse>(),  
                        FormStatus = result.gaia1001Form.FormStatus.ToString(),
                        AttendanceOn = result.gaia1001Form.AttendanceOn,
                        AttendanceType = result.gaia1001Form.AttendanceType.ToString()
                    },
                    new ApolloAttendanceResponse
                    {
                        CompanyId = result.apolloAttendance.CompanyId,
                        UserEmployeeId = result.apolloAttendance.UserEmployeeId,
                        AttendanceDate = result.apolloAttendance.AttendanceDate,
                        AttendanceType = result.apolloAttendance.AttendanceType.ToString(),
                        ApolloAttendanceHistories = result.apolloAttendance.ApolloAttendanceHistories?.Select(history =>
                            new ApolloAttendanceHistoryResponse(
                                history.AttendanceOn,
                                history.AttendanceMethod.ToString(),
                                history.IsEffective
                            )
                        ).ToList(),
                        Apollo1001Froms = result.apolloAttendance.Apollo1001Forms?.Select(form =>
                            new Apollo1001FromResponse(
                                form.FormKind,
                                form.FormNo,
                                form.ApprovalStatus.ToString()
                            )
                        ).ToList()
                    }
                )
            ),
            error => Problem()
        );
    }
}