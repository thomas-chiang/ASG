using ASG.Domain.Common;

namespace ASG.Domain.Gaia1001Forms;

public record RecalledReCheckInFormRequestBody(
    int SourceFormNo,
    string SourceFormKind,
    string CompanyId,
    string UserEmployeeId,
    bool IsCancel
): RequestBody;