using NUnit.Framework;

namespace Testing;

[TestFixture]
public partial class HttpRetryShould
{
    [Test]
    public void retry_a_http_request_three_times()
    {
        Given(a_request_will_fail);
        When(trying_to_send_the_request);
        Then(the_request_should_be_attempted_four_times);
    }
    
    [Test]
    public void retry_a_http_request_two_times_with_a_delay()
    {
        Given(a_request_will_fail);
        When(trying_to_send_the_request_with_delays);
        Then(the_request_should_be_attempted_three_times_with_delays);
    }
}