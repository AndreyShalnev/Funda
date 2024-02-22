using Flurl;
using FundaApi.Client.Client.Interfaces;
using FundaApi.Client.Data;
using FundaApi.Client.Data.Dtos;

namespace FundaApi.Client.Client
{
    internal class EstateObjectsClient : ClientBase<EstateObjectsPagedDto>, IEstateObjectsClient
    {
        public EstateObjectsClient(HttpClient httpClient)
            : base(httpClient) { }

        public async Task<ActionResult<EstateObjectsPagedDto>> GetEstateObjects(SearchEstateObjectsParameters parameters, CancellationToken cancellationToken)
        {
            var request = CreateGetEstateObjectsRequest(parameters);
            return await ExecuteAsync(request, cancellationToken);
        }

        private HttpRequestMessage CreateGetEstateObjectsRequest(SearchEstateObjectsParameters parameters)
        {
            var url = string.Empty
                .SetQueryParam("type", parameters.purchaseType)
                .SetQueryParam("zo", $"/{parameters.City}{GetGardenParameter(parameters)}/")
                .SetQueryParam("page", parameters.Page)
                .SetQueryParam("pagesize", parameters.PageSize);

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url, UriKind.Relative),
                Method = HttpMethod.Get
            };

            return request;
        }

        private string GetGardenParameter(SearchEstateObjectsParameters parameters)
        {
            return parameters.WithTuin ? "tuin/" : "";
        }

    }
}
