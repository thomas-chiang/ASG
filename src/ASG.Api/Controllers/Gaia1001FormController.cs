using ASG.Application.Gaia1001Forms.Queries.GetGaia1001Form;
using ASG.Contracts.Gaia1001Forms;
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

    [HttpGet]
    public async Task<IActionResult> GetGaia1001Form([FromQuery] GetGaia1001FormRequest request)
    {

        var query = new GetGaia1001FormQuery (request.FormKind, request.FormNo);

        var getGaia1001FormResult = await _mediator.Send(query);
        
        return getGaia1001FormResult.MatchFirst(
            gaia1001Form => Ok(new Gaia1001FormResponse{
                FormKind = gaia1001Form.FormKind,               
                FormNo = gaia1001Form.FormNo,                 
                CompanyId = gaia1001Form.CompanyId,              
                UserEmployeeId = gaia1001Form.UserEmployeeId,         
                PtSyncFormOperations = gaia1001Form.PtSyncFormOperations?.Select(operation => new PtSyncFormOperation
                {
                    FormContent = operation.FormContent,
                    FormAction = operation.FormAction.ToString(),
                    CreatedOn = operation.CreatedOn,
                    ModifiedOn = operation.ModifiedOn,
                    Flag = operation.Flag.ToString(),
                    RetryCount = operation.RetryCount,
                }).ToList() ?? new List<PtSyncFormOperation>(),  
                FormStatus = gaia1001Form.FormStatus.ToString(),
                AttendanceOn = gaia1001Form.AttendanceOn,
                AttendanceType = gaia1001Form.AttendanceType.ToString()
            }),
            error => Problem()
        );
    }
}



