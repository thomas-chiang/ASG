using ASG.Application.Gaia1001Forms.Queries.GetGaia1001Form;
using ASG.Contracts.Gaia1001Forms;
using ASG.Domain.Gaia1001Forms;
using ASG.Domain.Gaia1001Forms.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASG.Api.Controllers;

[Route("[controller]")]
public class Gaia1001FormController : ApiController
{
    private readonly ISender _mediator;

    public Gaia1001FormController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetGaia1001FormResponse), 200)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    public async Task<IActionResult> GetGaia1001Form([FromQuery] GetGaia1001FormRequest request)
    {
        var query = new GetGaia1001FormQuery(request.FormKind, request.FormNo);

        var getGaia1001FormResult = await _mediator.Send(query);

        return getGaia1001FormResult.Match(
            gaia1001Form => Ok(ToGetGaia1001FormResponse(gaia1001Form)),
            errors => Problem(errors)
        );
    }

    public static GetGaia1001FormResponse ToGetGaia1001FormResponse(Gaia1001Form gaia1001Form)
    {
        return new GetGaia1001FormResponse
        {
            FormKind = gaia1001Form.FormKind,
            FormNo = gaia1001Form.FormNo,
            CompanyId = gaia1001Form.CompanyId,
            UserEmployeeId = gaia1001Form.UserEmployeeId,
            PtSyncFormOperations = gaia1001Form.PtSyncFormOperations?.Select(operation =>
                new PTSyncFormOperationResponse
                {
                    FormContent = operation.FormContent,
                    FormAction = operation.FormAction.ToString(),
                    CreatedOn = operation.CreatedOn,
                    ModifiedOn = operation.ModifiedOn,
                    Flag = operation.Flag.ToString(),
                    RetryCount = operation.RetryCount,
                    ApplyReCheckInFormJson = operation.FormAction == FormAction.Apply
                        ? JsonConvert.DeserializeObject<ApplyReCheckInFormJsonResponse>(operation.FormContent)
                        : null,
                    ApproveReCheckInFormJson = operation.FormAction == FormAction.Approve
                        ? JsonConvert.DeserializeObject<ApproveReCheckInFormJsonResponse>(operation.FormContent)
                        : null,
                    RecalledReCheckInFormJson = operation.FormAction == FormAction.Recalled
                        ? JsonConvert.DeserializeObject<RecalledReCheckInFormJsonResponse>(operation.FormContent)
                        : null
                }).ToList() ?? new List<PTSyncFormOperationResponse>(),
            FormStatus = gaia1001Form.FormStatus.ToString(),
            AttendanceOn = gaia1001Form.AttendanceOn,
            AttendanceType = gaia1001Form.AttendanceType.ToString()
        };
    }
}