using NUnit.Framework;

namespace Testing;

[TestFixture]
public partial class HttpExtensionsShould
{
    [Test]
    public void get_the_content_from_a_successful_response()
    {
        Given(a_successful_response);
        When(getting_the_content_for_response);
        Then(the_response_content_is_returned);
    }
    
    [Test]
    public void throw_an_exception_when_the_response_is_not_successful()
    {
        Given(an_unsuccessful_response);
        When(Validating(getting_the_content_that_may_error));
        Then(the_exception_is_thrown);
    }
    
    [Test]
    public void throw_an_exception_when_the_response_is_not_successful_and_response_object_is_not_needed()
    {
        Given(an_unsuccessful_response);
        When(Validating(getting_the_content_that_may_error_and_response_object_is_not_needed));
        Then(the_exception_is_thrown);
    }  
    
    [Test]
    public void not_throw_an_exception_when_the_response_is_not_successful()
    {
        Given(a_successful_response);
        When(Validating(getting_the_content_that_may_error));
        Then(the_response_content_is_returned);
        And(the_exception_is_not_thrown);
    }
    
    [Test]
    public void get_the_content_from_a_request()
    {
        Given(a_request);
        When(getting_the_content_for_request);
        Then(the_request_content_is_returned);
    }
    
    [Test]
    public void serialise_string_content()
    {
        Given(an_object_to_serialise);
        When(serialising_string_content);
        Then(the_serialised_string_content_is_returned);
    }
}