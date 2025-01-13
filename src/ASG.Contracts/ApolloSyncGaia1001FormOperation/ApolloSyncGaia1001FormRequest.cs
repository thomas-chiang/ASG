using ASG.Contracts.Gaia1001Forms;

namespace ASG.Contracts.ApolloSyncGaia1001FormOperation;

public record ApolloSyncGaia1001FormRequest(
    string Url,
    string Method,
    ApplyReCheckInFormJsonResponse? ApplyReCheckInFormRequestBody,
    ApproveReCheckInFormJsonResponse? ApproveReCheckInFormRequestBody,
    RecalledReCheckInFormJsonResponse? RecalledReCheckInFormRequestBody,
    string? Result
);