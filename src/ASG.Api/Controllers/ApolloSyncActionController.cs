using ASG.Application.ApolloSyncActions.Commands.CreateApolloSyncAction;
using ASG.Contracts.ApolloSyncActions;
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
    public async Task<IActionResult> CreateApolloSyncAction(CreateApolloSyncActionRequest request)
    {
        var command = new CreateApolloSyncActionCommand(request.FormKindPlusFormNo);
        var createApolloSyncActionResult = await _mediator.Send(command);
        
        // var response = new ApolloSyncActionResponse(
        //     createApolloSyncActionResult.Value
        // );
        // return Ok(response);
        
        return createApolloSyncActionResult.MatchFirst(
            apolloSyncAction => Ok(new ApolloSyncActionResponse(apolloSyncAction.anonymousRequests.Count)),
            error => Problem()
        );
    }
}



