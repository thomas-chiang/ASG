namespace ASG.Domain.Common;

public class AnonymousRequest
{
    public string Url { get; set; }
    public HttpMethod Method { get; set; }
    public required RequestBody RequestBody { get; set; }
    public string? Result { get; set; }

    protected AnonymousRequest(string url, HttpMethod method)
    {
        Url = url;
        Method = method;
    }
}