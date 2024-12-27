using ASG.Application.Gaia1001Forms.Interfaces;
using ASG.Domain.Gaia1001Forms;
using ErrorOr;
using MediatR;

namespace ASG.Application.Gaia1001Forms.Queries.GetGaia1001Form;

public class GetGaia1001FormQueryHandler
    : IRequestHandler<GetGaia1001FormQuery, ErrorOr<Gaia1001Form>>
{
    
    private readonly IGaia1001FormRepository _gaia1001FormRepository;

    public GetGaia1001FormQueryHandler(IGaia1001FormRepository gaia1001FormRepository)
    {
        _gaia1001FormRepository = gaia1001FormRepository;
    }

    public async Task<ErrorOr<Gaia1001Form>> Handle(GetGaia1001FormQuery query, CancellationToken cancellationToken)
    {
        var gaia1001Form = await _gaia1001FormRepository.GetByFormKindPlusFormNoAsync(query.FormKindPlusFormNo);

        // return gaia1001Form is null
        //     ? Error.NotFound(description: "Gaia1001Forms not found")
        //     : gaia1001Form;

        return gaia1001Form is null
            ? Error.NotFound(description: "Gaia1001Forms not found")
            : new Gaia1001Form
            {
                FormStatus = gaia1001Form.FormStatus
            };
    }
}