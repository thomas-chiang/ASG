using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASG.Infrastructure.Common.AsiaTubeManageDbSchemas;

[Table("Company")]
public class Company
{
    [Key] public Guid CompanyId { get; set; }

    [Required]
    [MaxLength(400)]
    [Column(TypeName = "nvarchar")]
    public required string CompanyCode { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public required string CompanyName { get; set; }
}