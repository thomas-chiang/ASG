using ASG.Domain.Common;

namespace ASG.Domain.ApolloSyncGaia1001FormOperations.Requests;

public class ApplyReCheckInFormRequest : AnonymousRequest
{
    public ApplyReCheckInFormRequest()
        : base("https://pt-be.mayohr.com/api/anonymous/ReCheckInForm", HttpMethod.Post)
    {
    }
}