using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
public static class JsonSerialization
{
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        PropertyNamingPolicy = null,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    
    public static void UseDefaultNamingStrategy() => DefaultOptions.PropertyNamingPolicy = null;
    public static void UseCamelCaseNamingStrategy() => DefaultOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    
    public static string Serialize(object theObject) => JsonSerializer.Serialize(theObject, GetJsonSerializerOptions());
    public static T Deserialize<T>(string theJson) => JsonSerializer.Deserialize<T>(theJson, GetJsonSerializerOptions())!;
    public static object? Deserialize(string theJson, Type type) => JsonSerializer.Deserialize(theJson, type, GetJsonSerializerOptions());
    public static void RegisterConverter(JsonConverter converter) => Converters.Add(converter);
    public static void RegisterConverters(IEnumerable<JsonConverter> converters)
    {
        foreach (var converter in converters)
        {
            RegisterConverter(converter);
        }
    }
    public static void ResetConverters() => Converters = [new JsonStringEnumConverter()];
    private static List<JsonConverter> Converters = [new JsonStringEnumConverter()];

    public static JsonSerializerOptions GetJsonSerializerOptions()
    {
        var options = new JsonSerializerOptions(DefaultOptions);
        foreach (var converter in Converters)
        {
            options.Converters.Add(converter);
        }
        return options;
    }
}