namespace ASG.Domain.Gaia1001Forms;

public class PtSyncFormOperation
{
    public string FormContent { get; set; }

    public FormAction FormAction { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Flag Flag { get; set; }

    public byte? RetryCount { get; set; }
}