namespace ASG.Contracts.Gaia1001Forms;

public record RecalledReCheckInFormJsonResponse(
    int SourceFormNo,
    string SourceFormKind,
    string CompanyId,
    string UserEmployeeId,
    bool IsCancel
);