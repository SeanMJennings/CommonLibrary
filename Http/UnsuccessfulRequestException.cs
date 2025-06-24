using System.Net;

namespace Http;

public class UnsuccessfulRequestException(string message, HttpStatusCode statusCode, string content) : Exception(message)
{
    public HttpStatusCode StatusCode => statusCode;
    public string Content => content;
}