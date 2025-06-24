using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
public static class JsonSerialization
{
    private static readonly DefaultContractResolver ContractResolver = new()
    {
        NamingStrategy = new DefaultNamingStrategy()
    };
    
    public static void UseDefaultNamingStrategy() => ContractResolver.NamingStrategy = new CamelCaseNamingStrategy();
    public static void UseCamelCaseNamingStrategy() => ContractResolver.NamingStrategy = new CamelCaseNamingStrategy();
    
    public static string Serialize(object theObject) => JsonConvert.SerializeObject(theObject, GetJsonSerializerSettings());
    public static T Deserialize<T>(string theJson) => JsonConvert.DeserializeObject<T>(theJson, GetJsonSerializerSettings())!;
    public static object? Deserialize(string theJson, Type type) => JsonConvert.DeserializeObject(theJson, type, GetJsonSerializerSettings())!;
    public static void RegisterConverter(JsonConverter converter) => Converters.Add(converter);
    public static void RegisterConverters(IEnumerable<JsonConverter> converters)
    {
        foreach (var converter in converters)
        {
            RegisterConverter(converter);
        }
    }
    public static void ResetConverters() => Converters = [new Newtonsoft.Json.Converters.StringEnumConverter()];
    private static JsonConverterCollection Converters = [new Newtonsoft.Json.Converters.StringEnumConverter()];

    public static JsonSerializerSettings GetJsonSerializerSettings()
    {
        return new JsonSerializerSettings{
            Converters = Converters, 
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = ContractResolver
        };
    }

}