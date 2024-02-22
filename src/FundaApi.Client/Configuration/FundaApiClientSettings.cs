using System.Threading.RateLimiting;

namespace FundaApi.Client.Configuration
{
    public class FundaApiClientSettings
    {
        public string BaseUri { get; set; }
        public string AccessKey { get; set; }
        public int PageSize { get; set; }
        public FixedWindowRateLimiterOptions FixedWindowRateLimiterOptions { get; set; }
    }
}
