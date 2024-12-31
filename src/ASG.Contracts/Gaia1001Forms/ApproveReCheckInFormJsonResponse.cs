namespace ASG.Contracts.Gaia1001Forms;

public record ApproveReCheckInFormJsonResponse(
    int SourceFormNo,
    string SourceFormKind,
    string Status,
    string CompanyId,
    string EmployeeId,
    string UserEmployeeId,
    bool IsAgree
);