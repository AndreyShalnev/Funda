using FundaApi.Client.Client;
using FundaApi.Client.Client.Interfaces;
using FundaApi.Client.Configuration;
using FundaApi.Client.Services;
using HttpClientFactoryLite;
using System.Threading.RateLimiting;

namespace FundaApi.Client
{
    public sealed class FundaApiClientFactory
    {
        private readonly string _httpClientName;
        private readonly HttpClientFactory _httpClientFactory;
        private readonly FundaApiClientSettings _settings;

        public FundaApiClientFactory(string applicationName, FundaApiClientSettings settings)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                throw new ArgumentNullException(nameof(applicationName));
            }

            ValidateSettings(settings);

            _settings = settings;
            _httpClientName = $"{applicationName}-http-client";
            _httpClientFactory = new HttpClientFactory();

            _httpClientFactory.Register(_httpClientName,
                builder =>
                {
                    builder.ConfigureHttpClient(baseHttpClient =>
                    {
                        baseHttpClient.BaseAddress = new Uri($"{_settings.BaseUri}/{_settings.AccessKey}/");
                    });
                    builder.ConfigureHttpMessageHandlerBuilder(httpMessageHandler =>
                    {
                        var fixedWindowRateLimiter = new FixedWindowRateLimiter(_settings.FixedWindowRateLimiterOptions);
                        var clientSideRateLimitedHandler = new ClientSideRateLimitedHandler(fixedWindowRateLimiter);
                        httpMessageHandler.PrimaryHandler = clientSideRateLimitedHandler;
                    });
                });
        }

        internal HttpClient CreateHttpClient()
        {
            return _httpClientFactory.CreateClient(_httpClientName);
        }

        internal IEstateObjectsClient CreateEstateObjectsClient()
        {
            var httpClient = CreateHttpClient();
            return new EstateObjectsClient(httpClient);
        }

        public IEstateObjectService CreateEstateObjectService()
        {
            var client = CreateEstateObjectsClient();
            return new EstateObjectService(client, _settings.PageSize);
        }

        private void ValidateSettings(FundaApiClientSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(typeof(FundaApiClientSettings).Name);
            }

            if (string.IsNullOrEmpty(settings.AccessKey))
            {
                throw new ArgumentException($"{nameof(settings.AccessKey)} is null or empty");
            }

            if (string.IsNullOrEmpty(settings.BaseUri))
            {
                throw new ArgumentException($"{nameof(settings.BaseUri)} is null or empty");
            }

            if (settings.PageSize < 1)
            {
                throw new ArgumentException($"{nameof(settings.PageSize)} should be greater than 0");
            }

            if (settings.FixedWindowRateLimiterOptions == null)
            {
                throw new ArgumentNullException($"{nameof(settings.FixedWindowRateLimiterOptions)} should be configured");
            }
        }
    }
}