using System.Net;
using System.Text.Json;
using BDD;
using Http;
using Shouldly;

namespace Testing;

public partial class HttpExtensionsShould : Specification
{
    private HttpRequestMessage request = null!;
    private TestObject theTestRequest = null!;
    private TestObject theReturnedTestRequest = null!;
    private HttpResponseMessage response = null!;
    private TestObject theTestResponse = null!;
    private TestObject theReturnedTestResponse = null!;
    private object objectToSerialise = null!;
    private object theApiError = null!;

    private record TestObject(string Wibble, string Wobble);
    private enum TestEnum { Wibble, Wobble, Wabble }
    private record TestObjectWithEnum(TestEnum Wibble, string Wobble);

    protected override void before_each()
    {
        base.before_each();
        request = null!;
        objectToSerialise = null!;
        theTestRequest = new TestObject("wubble", "wabble");
        theReturnedTestRequest = null!;
        theTestResponse = new TestObject("wibble", "wobble");
        theApiError = "I cannot work because...";
        theReturnedTestResponse = null!;
        response = null!;
    }

    private void a_successful_response()
    {
        response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(theTestResponse))
        };
    }

    private void a_request()
    {
        request = new HttpRequestMessage(HttpMethod.Post, "https://wibble")
        {
            Content = new StringContent(JsonSerializer.Serialize(theTestRequest))
        };
    }

    private void getting_the_content_for_response()
    {
        theReturnedTestResponse = response.DeserialiseHttpResponse<TestObject>().Await();
    }
    
    private void getting_the_content_for_request()
    {
        theReturnedTestRequest = request.DeserialiseHttpRequest<TestObject>().Await();
    }
    
    private void getting_the_content_that_may_error()
    {
        theReturnedTestResponse = response.DeserialiseHttpResponse<TestObject>("I'm an error message").Await();
    }    
    
    private void getting_the_content_that_may_error_and_response_object_is_not_needed()
    {
        response.ThrowErrorIfUnsuccessful("I'm an error message").Await();
    }

    private void the_response_content_is_returned()
    {
        theReturnedTestResponse.Wibble.ShouldBe(theTestResponse.Wibble);
        theReturnedTestResponse.Wobble.ShouldBe(theTestResponse.Wobble);
    }
    
    private void the_request_content_is_returned()
    {
        theReturnedTestRequest.Wibble.ShouldBe(theTestRequest.Wibble);
        theReturnedTestRequest.Wobble.ShouldBe(theTestRequest.Wobble);
    }

    private void an_unsuccessful_response()
    {
        response = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
        {
            Content = new StringContent(theApiError.ToString()!)
        };
    }

    private void the_exception_is_thrown()
    {
        error.Message.ShouldBe("I'm an error message");
        ((UnsuccessfulRequestException)error).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        ((UnsuccessfulRequestException)error).Content.ShouldBe(theApiError.ToString());
        error.ShouldBeAssignableTo<UnsuccessfulRequestException>();
    }    
    
    private static void the_exception_is_not_thrown()
    {
        error.ShouldBeNull();
    }

    private void an_object_to_serialise()
    {
        objectToSerialise = theTestRequest;
    }

    private void serialising_string_content()
    {
        objectToSerialise = new TestObjectWithEnum(TestEnum.Wabble, "wobble");
    }

    private void the_serialised_string_content_is_returned()
    {
        var serialised = objectToSerialise.ToJsonStringContent();
        var deserialised = JsonSerialization.Deserialize<TestObjectWithEnum>(serialised.ReadAsStringAsync().Await());
        deserialised!.Wibble.ShouldBe(TestEnum.Wabble);
        deserialised.Wobble.ShouldBe("wobble");
        serialised.Headers.ContentType!.MediaType.ShouldBe("application/json");
    }
}