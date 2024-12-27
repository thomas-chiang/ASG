using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ASG.Domain.ApolloSyncActions;

public class AnonymousRequest
{
    public string Url { get; set; }
    public HttpMethod Method { get; set; }
    public HttpRequestHeaders Headers { get; private set; }
    public HttpContent Content { get; set; }

    public AnonymousRequest(string url, HttpMethod method)
    {
        Url = url;
        Method = method;
        Headers = new HttpRequestMessage().Headers;
    }

    public void AddHeader(string name, string value)
    {
        Headers.Add(name, value);
    }

    public void SetContent(string content, string mediaType = "application/json")
    {
        Content = new StringContent(content, Encoding.UTF8, mediaType);
    }

    public async Task<HttpResponseMessage> ExecuteAsync(HttpClient httpClient)
    {
        using var request = new HttpRequestMessage(Method, Url)
        {
            Content = Content
        };

        foreach (var header in Headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        return await httpClient.SendAsync(request);
    }
}