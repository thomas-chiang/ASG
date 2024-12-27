namespace ASG.Domain.Gaia1001Forms;

public class PTSyncFormOperation
{
    public string FormContent { get; set; }

    public byte FormAction { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte Flag { get; set; }

    public byte? RetryCount { get; set; }
}