using Newtonsoft.Json;

namespace FundaApi.Client.Data.Dtos
{
    public class PagingDto
    {
        [JsonProperty("AantalPaginas")]
        public int TotalPages { get; set; }
        [JsonProperty("HuidigePagina")]
        public int CurrentPage { get; set; }
    }
}
