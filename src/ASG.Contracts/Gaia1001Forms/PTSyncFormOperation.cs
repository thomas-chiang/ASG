namespace ASG.Contracts.Gaia1001Forms;

public record PtSyncFormOperation
{
    public string FormContent { get; init; } = null!;
    public string FormAction { get; init; }

    public DateTime CreatedOn { get; init; }

    public DateTime? ModifiedOn { get; init; }

    public string Flag { get; init; } = null!;

    public byte? RetryCount { get; init; }
}
