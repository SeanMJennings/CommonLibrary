using System.Net;
using BDD;
using Http;
using Moq;
using Moq.Contrib.HttpClient;
using Shouldly;

namespace Testing;

public partial class HttpRetryShould : Specification
{
    private const string BaseUrl = "https://www.example.com";
    private Mock<HttpMessageHandler> MockHandler = null!;
    private int count;
    private DateTime start = DateTime.Now;
    private DateTime end = DateTime.Now;
    
    protected override void before_each()
    {
        base.before_each();
        MockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        count = 0;
        start = DateTime.Now;
        end = DateTime.Now;
    }
    
    private void a_request_will_fail()
    {
        MockHandler.SetupRequest(HttpMethod.Get, BaseUrl + "/wibble", _ =>
            {
                count++;
                return true;
            })
            .ReturnsResponse(HttpStatusCode.InternalServerError);
    }

    private void trying_to_send_the_request()
    {
        var httpClient = new HttpClient(MockHandler.Object) { BaseAddress = new Uri(BaseUrl) };
        HttpRetry.New(() => httpClient.GetAsync("wibble")).WithRetryCount(3).TryRequest().Await();
    }    
    
    private void trying_to_send_the_request_with_delays()
    {
        start = DateTime.Now;
        var httpClient = new HttpClient(MockHandler.Object) { BaseAddress = new Uri(BaseUrl) };
        HttpRetry.New(() => httpClient.GetAsync("wibble"))
            .WithDelay(TimeSpan.FromSeconds(1.5))
            .TryRequest()
            .Await();
        end = DateTime.Now;
    }

    private void the_request_should_be_attempted_four_times()
    {
        count.ShouldBe(4);
    }
    
    private void the_request_should_be_attempted_three_times_with_delays()
    {
        count.ShouldBe(3);
        (end - start).TotalSeconds.ShouldBeGreaterThan(3);
    }
}