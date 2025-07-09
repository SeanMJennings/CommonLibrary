using BDD;
using Shouldly;

namespace Testing;

public partial class SerializationShould : Specification
{
    private TestObject testObject = null!;
    private AnotherTestObject anotherTestObject = null!;
    private string json = null!;
    private string testObjectJson = null!;
    
    private class TestObject
    {
        public TestEnum Enum { get; set; }
        public string? WibbleWobble { get; set; }
    }
    
    public class AnotherTestObject
    {
        public TestEnum Enum { get; set; }
        public string? WibbleWobble { get; set; }
    }
    
    public enum TestEnum
    {
        Value1,
        Value2
    }
    
    public class CustomTestObject2Converter : System.Text.Json.Serialization.JsonConverter<AnotherTestObject>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsSubclassOf(typeof(TestObject)) || typeToConvert == typeof(AnotherTestObject);
        }

        public override AnotherTestObject? Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            throw new ArgumentException();
        }

        public override void Write(System.Text.Json.Utf8JsonWriter writer, AnotherTestObject value, System.Text.Json.JsonSerializerOptions options)
        {
            writer.WriteStringValue("Value1");
        }
    }

    protected override void before_each()
    {
        base.before_each();
        JsonSerialization.UseDefaultNamingStrategy();
        JsonSerialization.ResetConverters();
        testObject = null!;
        anotherTestObject = null!;
        json = null!;
        testObjectJson = null!;
    }

    private void a_test_object()
    {
        testObject = new TestObject { Enum = TestEnum.Value2, WibbleWobble = "test"};
    }
    
    private void another_test_object()
    {
        anotherTestObject = new AnotherTestObject { Enum = TestEnum.Value2, WibbleWobble = "test"};
    }

    private void serializing_the_object()
    {
        json = JsonSerialization.Serialize(testObject);
    }

    private void serializing_the_object_with_camel_case()
    {
        JsonSerialization.UseCamelCaseNamingStrategy();
        serializing_the_object();
    }
    
    private void serializing_the_object_with_custom_converter()
    {
        JsonSerialization.RegisterConverter(new CustomTestObject2Converter());
        json = JsonSerialization.Serialize(anotherTestObject);
    }
    
    private void the_object_is_serialized_to_json_with_custom_converter()
    {
        json.ShouldContain("Value1");
    }

    private void the_object_is_serialized_to_json_with_string_enums()
    {
        json.ShouldContain("Value2");
    }

    private void a_test_object_as_json()
    {
        a_test_object();
        testObjectJson = JsonSerialization.Serialize(testObject);
    }
    
    private void a_test_object_with_a_null_value()
    {
        testObject = new TestObject { Enum = TestEnum.Value2, WibbleWobble = null };
    }
    
    private void deserializing_the_object()
    {
        testObject = JsonSerialization.Deserialize<TestObject>(testObjectJson);
    }

    private void deserializing_the_object_with_type_at_runtime()
    {
        testObject = (TestObject)JsonSerialization.Deserialize(testObjectJson, typeof(TestObject))!;
    }

    private void the_object_is_deserialized_with_enum_values()
    {
        testObject.Enum.ShouldBe(TestEnum.Value2);
    }
    
    private void the_object_is_serialized_to_json_without_null_values()
    {
        json.ShouldNotContain("WibbleWobble");
    }
    
    private void the_object_is_serialized_to_json_with_pascal_case_properties()
    {
        json.ShouldContain("Enum");
    }
    
    private void the_object_is_serialized_to_json_with_camel_case_properties()
    {
        json.ShouldContain("enum");
        json.ShouldContain("wibbleWobble");
    }
}