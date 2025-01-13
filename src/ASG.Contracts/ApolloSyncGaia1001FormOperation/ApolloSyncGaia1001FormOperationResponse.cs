namespace ASG.Contracts.ApolloSyncGaia1001FormOperation;

public record ApolloSyncGaia1001FormOperationResponse(
    string? SituationEnum,
    string? Situation,
    List<ApolloSyncGaia1001FormRequest> AnonymousRequests);