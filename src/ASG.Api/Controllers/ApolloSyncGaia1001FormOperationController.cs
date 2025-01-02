using System.ComponentModel;
using ASG.Application.ApolloSyncGaia1001FormOperations.Commands.CreateApolloSyncAction;
using ASG.Contracts.ApolloSyncGaia1001FormOperation;
using ASG.Contracts.Gaia1001Forms;
using ASG.Domain.ApolloSyncGaia1001FormOperation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASG.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ApolloSyncGaia1001FormOperationController : Controller
{
    private readonly ISender _mediator;

    public ApolloSyncGaia1001FormOperationController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateApolloSyncGaia1001FormOperationResponse), 200)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    public async Task<IActionResult> CreateApolloSyncGaia1001FormOperation(
        CreateApolloSyncGaia1001FormOperationRequest request)
    {
        var command = new CreateApolloSyncGaia1001FormOperationCommand(request.FormKind, request.FormNo);
        var result = await _mediator.Send(command);


        return result.MatchFirst(
            operation => Ok(new CreateApolloSyncGaia1001FormOperationResponse(
                Gaia1001FormController.ToGetGaia1001FormResponse(operation.Gaia1001Form),
                Gaia1001FormWithApolloAttendanceController.ToApolloAttendanceResponse(operation.ApolloAttendance),
                new ApolloSyncGaia1001FormOperationResponse(
                    GetSync1001FormSituationDescription(operation.Situation),
                    operation.AnonymousRequests.Select(anonymousRequest =>
                        new ApolloSyncGaia1001FormRequest
                        (
                            anonymousRequest.Url,
                            anonymousRequest.Method.ToString(),
                            anonymousRequest is ApplyReCheckInFormRequest
                                ? JsonConvert.DeserializeObject<ApplyReCheckInFormJsonResponse>(
                                    JsonConvert.SerializeObject(anonymousRequest.RequestBody))
                                : null,
                            anonymousRequest is ApproveReCheckInFormRequest
                                ? JsonConvert.DeserializeObject<ApproveReCheckInFormJsonResponse>(
                                    JsonConvert.SerializeObject(anonymousRequest.RequestBody))
                                : null,
                            anonymousRequest is RecalledReCheckInFormRequest
                                ? JsonConvert.DeserializeObject<RecalledReCheckInFormJsonResponse>(
                                    JsonConvert.SerializeObject(anonymousRequest.RequestBody))
                                : null,
                            anonymousRequest.Result
                        )
                    ).ToList()),
                operation.UpdatedApolloAttendance != null
                    ? Gaia1001FormWithApolloAttendanceController.ToApolloAttendanceResponse(operation
                        .UpdatedApolloAttendance)
                    : null
            )),
            error => Problem()
        );
    }
    
    private static string? GetSync1001FormSituationDescription(Sync1001FormSituation? situation)
    {
        if (!situation.HasValue) return null;

        var situationValue = situation.Value;
        var field = situationValue.GetType().GetField(situationValue.ToString());

        if (field == null) return situationValue.ToString();

        var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

        return attribute?.Description ?? situationValue.ToString();
    }
}