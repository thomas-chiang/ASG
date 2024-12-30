using ASG.Domain.ApolloAttendances;
using ASG.Domain.Gaia1001Forms;

namespace ASG.Domain.Gaia1001FormWithApolloAttendance;

public class Gaia1001FormWithApolloAttendance
{
    public Gaia1001Form gaia1001Form { get; set; }
    
    public ApolloAttendance apolloAttendance { get; set; }
}