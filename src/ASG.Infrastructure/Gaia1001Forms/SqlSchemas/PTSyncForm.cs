using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASG.Infrastructure.Gaia1001Forms.SqlSchemas;

[Table("PTSyncForm", Schema = "gbpm")]
public class PTSyncForm
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public Guid CompanyId { get; set; }

    public Guid UserEmployeeId { get; set; }

    [MaxLength(100)]
    public string FormKind { get; set; }

    public int FormNo { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string FormContent { get; set; }

    public byte FormAction { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte Flag { get; set; }

    public byte? RetryCount { get; set; }
}