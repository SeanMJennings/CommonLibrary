using NUnit.Framework;

namespace Testing;

[TestFixture]
public partial class SerializationShould
{
    [Test]
    public void serialize_enums_to_strings()
    {
        Given(a_test_object);
        When(serializing_the_object);
        Then(the_object_is_serialized_to_json_with_string_enums);
    }
    
    [Test]
    public void deserialize_enums_from_strings()
    {
        Given(a_test_object_as_json);
        When(deserializing_the_object);
        Then(the_object_is_deserialized_with_enum_values);
    }    
    
    [Test]
    public void deserialize_types_known_at_runtime()
    {
        Given(a_test_object_as_json);
        When(deserializing_the_object_with_type_at_runtime);
        Then(the_object_is_deserialized_with_enum_values);
    }

    [Test]
    public void ignore_null_values()
    {
        Given(a_test_object_with_a_null_value);
        When(serializing_the_object);
        Then(the_object_is_serialized_to_json_without_null_values);
    }
    
    [Test]
    public void pascal_case_properties_by_default()
    {
        Given(a_test_object);
        When(serializing_the_object);
        Then(the_object_is_serialized_to_json_with_pascal_case_properties);
    }
    
    [Test]
    public void allow_camel_case_properties()
    {
        Given(a_test_object);
        When(serializing_the_object_with_camel_case);
        Then(the_object_is_serialized_to_json_with_camel_case_properties);
    }
    
    [Test]
    public void allow_registering_converters()
    {
        Given(another_test_object);
        When(serializing_the_object_with_custom_converter);
        Then(the_object_is_serialized_to_json_with_custom_converter);
    }
}