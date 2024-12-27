using ASG.Domain.Gaia1001Forms;
using ErrorOr;
using MediatR;

namespace ASG.Application.Gaia1001Forms.Queries.GetGaia1001Form;

public record GetGaia1001FormQuery(string FormKind, int FormNo) : IRequest<ErrorOr<Gaia1001Form>>;