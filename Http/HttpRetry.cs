using Polly;
using Polly.Retry;

namespace Http;

public static class HttpRetry
{
    public static HttpRetryBuilder New(Func<Task<HttpResponseMessage>> action)
    {
        return new HttpRetryBuilder(action);
    }
    
    public class HttpRetryBuilder
    {
        private readonly Func<Task<HttpResponseMessage>> action;
        private int retryCount = 2;
        private TimeSpan delay = TimeSpan.FromSeconds(1);
        
        internal HttpRetryBuilder(Func<Task<HttpResponseMessage>> action)
        {
            this.action = action;
        }
        
        public HttpRetryBuilder WithDelay(TimeSpan theDelay)
        {
            delay = theDelay;
            return this;
        }
        
        public HttpRetryBuilder WithRetryCount(uint theRetryCount)
        {
            retryCount = (int)theRetryCount;
            return this;
        }
        
        public Task<HttpResponseMessage> TryRequest()
        {
            var retryPolicy = Policy.HandleResult<HttpResponseMessage>(
                    response => !response.IsSuccessStatusCode)
                .WaitAndRetryAsync(retryCount, _ => delay);
            return retryPolicy.ExecuteAsync(action);
        }
    }
}