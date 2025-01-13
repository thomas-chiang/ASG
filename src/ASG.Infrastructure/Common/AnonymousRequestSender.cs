using System.Text.Json;
using ASG.Application.Common.Interfaces;
using ASG.Domain.Common;

namespace ASG.Infrastructure.Common;

public class AnonymousRequestSender : IAnonymousRequestSender
{
    private readonly HttpClient _httpClient;

    public AnonymousRequestSender(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendAndUpdateAnonymousRequest(AnonymousRequest anonymousRequest)
    {
        var requestMessage = new HttpRequestMessage(anonymousRequest.Method, anonymousRequest.Url);

        var jsonBody = JsonSerializer.Serialize(anonymousRequest.RequestBody);
        requestMessage.Content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(requestMessage);

        var responseBody = await response.Content.ReadAsStringAsync();

        anonymousRequest.Result = $"Status Code: {response.StatusCode}, Response Body: {responseBody}";
    }
}