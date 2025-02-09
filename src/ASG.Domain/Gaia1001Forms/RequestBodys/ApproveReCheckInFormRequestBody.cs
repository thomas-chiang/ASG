using ASG.Domain.Common;

namespace ASG.Domain.Gaia1001Forms.RequestBodys;

public record ApproveReCheckInFormRequestBody(
    int SourceFormNo,
    string SourceFormKind,
    string Status,
    string CompanyId,
    string EmployeeId,
    string UserEmployeeId,
    bool IsAgree
) : RequestBody;