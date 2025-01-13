namespace ASG.Contracts.Gaia1001Forms;

public record PTSyncFormOperationResponse
{
    public string FormContent { get; init; } = null!;
    public string FormAction { get; init; }

    public DateTime CreatedOn { get; init; }

    public DateTime? ModifiedOn { get; init; }

    public string Flag { get; init; } = null!;

    public byte? RetryCount { get; init; }

    public ApplyReCheckInFormJsonResponse? ApplyReCheckInFormJson { get; init; }

    public ApproveReCheckInFormJsonResponse? ApproveReCheckInFormJson { get; init; }

    public RecalledReCheckInFormJsonResponse? RecalledReCheckInFormJson { get; init; }
}