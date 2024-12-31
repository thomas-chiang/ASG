using System.Net.Http.Headers;
using System.Text;
using ASG.Domain.Common;

namespace ASG.Domain.ApolloSyncGaia1001FormOperation;

public class AnonymousRequest
{
    public string Url { get; set; }
    public HttpMethod Method { get; set; }
    public RequestBody RequestBody { get; set; }
    public string? RequestResult { get; set; }
}