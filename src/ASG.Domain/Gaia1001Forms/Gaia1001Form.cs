using ASG.Domain.Common;

namespace ASG.Domain.Gaia1001Forms;

public class Gaia1001Form
{
    // TODO: create class FormKind
    public required string FormKind { get; set; }

    public required int FormNo { get; set; }
    
    public Guid CompanyId { get; set; }

    public Guid UserEmployeeId { get; set; }
    
    public List<PtSyncFormOperation>? PtSyncFormOperations { get; set; }
    
    public Gaia1001FormStatus FormStatus { get; set; }
    
    public AttendanceType AttendanceType { get; set; }
    
    public DateTime AttendanceOn { get; set; }
}