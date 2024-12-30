using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASG.Domain.Gaia1001Forms;

namespace ASG.Infrastructure.Gaia1001Forms.AisaFlowDbSchemas;

[Table("PTSyncForm", Schema = "gbpm")]
public class PtSyncForm
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public Guid CompanyId { get; set; }

    public Guid UserEmployeeId { get; set; }

    [MaxLength(100)] public string FormKind { get; set; }

    public int FormNo { get; set; }

    [Column(TypeName = "nvarchar(max)")] public string FormContent { get; set; }

    public byte FormAction { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte Flag { get; set; }

    public byte? RetryCount { get; set; }

    public FormAction GetFormActionEnum()
    {
        return FormAction switch
        {
            1 => Domain.Gaia1001Forms.FormAction.Apply,
            2 => Domain.Gaia1001Forms.FormAction.Approve,
            3 => Domain.Gaia1001Forms.FormAction.Recalled,
            _ => throw new InvalidOperationException($"Invalid FormAction value: {FormAction}")
        };
    }

    public Flag GetFlagEnum()
    {
        return Flag switch
        {
            1 => Domain.Gaia1001Forms.Flag.Success,
            2 => Domain.Gaia1001Forms.Flag.Fail,
            _ => throw new InvalidOperationException($"Invalid Flag value: {FormAction}")
        };
    }
}