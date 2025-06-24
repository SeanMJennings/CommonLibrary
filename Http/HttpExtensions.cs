using System.Text;
using Newtonsoft.Json;

namespace Http;

public static class HttpExtensions
{
    public static async Task<T> DeserialiseHttpResponse<T>(this HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(content)!;
    }
    
    public static async Task<T> DeserialiseHttpResponse<T>(this HttpResponseMessage response, string errorMessage)
    {
        await response.ThrowErrorIfUnsuccessful(errorMessage);
        return await response.DeserialiseHttpResponse<T>();
    }

    public static async Task ThrowErrorIfUnsuccessful(this HttpResponseMessage response, string errorMessage = "Request was unsuccessful")
    {
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new UnsuccessfulRequestException(errorMessage,response.StatusCode, content);
        }
    }

    public static async Task<T> DeserialiseHttpRequest<T>(this HttpRequestMessage request)
    {
        var content = await request.Content!.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(content)!;
    }
    
    public static StringContent ToJsonStringContent(this object theObject) => new(JsonSerialization.Serialize(theObject), Encoding.UTF8, "application/json");
}