using ASG.Application.ApolloSyncGaia1001FormOperations.Commands.CreateApolloSyncAction;
using ASG.Contracts.ApolloSyncGaia1001FormOperation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ASG.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ApolloSyncActionController : Controller
{
    
    private readonly ISender _mediator;
    
    public ApolloSyncActionController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateApolloSyncAction(CreateApolloSyncGaia1001FormOperationRequest request)
    {
        var command = new CreateApolloSyncGaia1001FormOperationCommand(request.FormKind, request.FormNo);
        var createApolloSyncActionResult = await _mediator.Send(command);
        
        // var response = new CreateApolloSyncGaia1001FormOperationResponse(
        //     createApolloSyncActionResult.Value
        // );
        // return Ok(response);
        
        return createApolloSyncActionResult.MatchFirst(
            apolloSyncAction => Ok(new CreateApolloSyncGaia1001FormOperationResponse(apolloSyncAction.AnonymousRequests.Count)),
            error => Problem()
        );
    }
}



