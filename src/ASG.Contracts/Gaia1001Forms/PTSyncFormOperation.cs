namespace ASG.Contracts.Gaia1001Forms;

public record PTSyncFormOperation
{
    public string FormContent { get; init; }

    public byte FormAction { get; init; }

    public DateTime CreatedOn { get; init; }

    public DateTime? ModifiedOn { get; init; }

    public byte Flag { get; init; }

    public byte? RetryCount { get; init; }
}
