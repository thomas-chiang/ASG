using ASG.Application.Gaia1001Forms.Queries.GetGaia1001Form;
using ASG.Contracts.Gaia1001Form;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ASG.Api.Controllers;


[ApiController]
[Route("[controller]")]
public class Gaia1001FormController : ControllerBase
{
    private readonly ISender _mediator;

    public Gaia1001FormController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{formKindPlusFormNo}")]
    public async Task<IActionResult> GetGaia1001Form(string formKindPlusFormNo)
    {

        var command = new GetGaia1001FormQuery(formKindPlusFormNo);

        var getGaia1001FormResult = await _mediator.Send(command);
        
        // return Ok(formKindPlusFormNo);
        
        return getGaia1001FormResult.MatchFirst(
            gaia1001Form => Ok(new Gaia1001FormResponse(gaia1001Form.FormStatus.ToString())),
            error => Problem()
        );
    }
}



