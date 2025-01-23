using System.Net;
using ASG.Application.Common.Interfaces;
using ASG.Domain.Common;

namespace ASG.Application.SubcutaneousTests.Common.Infrastructure.Common;

public class MockAnonymousRequestSender : IAnonymousRequestSender
{
    private readonly HttpClient _httpClient;

    public MockAnonymousRequestSender(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task SendAndUpdateAnonymousRequest(AnonymousRequest anonymousRequest)
    {
        anonymousRequest.StatusCode = HttpStatusCode.InternalServerError;
        anonymousRequest.Result = $"Status Code: {"Mock 500"}, Response Body: {"Mock Response Body"}";
        return Task.CompletedTask;
    }
}