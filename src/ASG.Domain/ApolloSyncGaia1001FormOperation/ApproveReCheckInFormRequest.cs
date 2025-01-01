using ASG.Domain.Common;

namespace ASG.Domain.ApolloSyncGaia1001FormOperation;

public class ApproveReCheckInFormRequest : AnonymousRequest
{
    public ApproveReCheckInFormRequest()
        : base("https://pt-be.mayohr.com/api/anonymous/ReCheckInForm/Approve", HttpMethod.Put)
    {
    }
}