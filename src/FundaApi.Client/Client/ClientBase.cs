using FundaApi.Client.Data;
using Newtonsoft.Json;
using System.Net;

namespace FundaApi.Client.Client
{
    internal abstract class ClientBase<TResponse>
    {
        protected readonly HttpClient _httpClient;
        
        protected ClientBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ActionResult<TResponse>> ExecuteAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.SendAsync(request, cancellationToken);
                
                return await ConvertToActionResult(response);
            }
            catch (Exception ex)
            {
                return ActionResult<TResponse>.Failed(ex.Message, ex);
            }
        }

        private async Task<ActionResult<TResponse>> ConvertToActionResult(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return ActionResult<TResponse>.Success(default);
                }

                var resultObject = JsonConvert.DeserializeObject<TResponse>(content);
                return ActionResult<TResponse>.Success(resultObject);
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string reason = await response.Content.ReadAsStringAsync();
                return ActionResult<TResponse>.FailedWithHttpStatusCode(response.StatusCode, reason);
            }

            return ActionResult<TResponse>.FailedWithHttpStatusCode(response.StatusCode);
        }
    }
}
