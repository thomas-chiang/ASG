using ASG.Domain.Gaia1001Forms;

namespace ASG.Application.Gaia1001Forms.Interfaces;

public interface IGaia1001FormRepository
{
    Task<Gaia1001Form?> GetByFormKindPlusFormNoAsync(string formKindPlusFormNo);
}