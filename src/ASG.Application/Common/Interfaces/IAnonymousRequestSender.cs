using ASG.Domain.Common;

namespace ASG.Application.Common.Interfaces;

public interface IAnonymousRequestSender
{
    Task SendAndUpdateAnonymousRequest(AnonymousRequest anonymousRequest);
}