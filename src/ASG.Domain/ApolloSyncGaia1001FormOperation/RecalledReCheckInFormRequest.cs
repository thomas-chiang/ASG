using ASG.Domain.Common;

namespace ASG.Domain.ApolloSyncGaia1001FormOperation;

public class RecalledReCheckInFormRequest : AnonymousRequest
{
    public RecalledReCheckInFormRequest()
        : base("https://pt-be.mayohr.com/api/anonymous/ReCheckInForm/Recalled", HttpMethod.Put)
    {
    }
}