using Newtonsoft.Json;

namespace FundaApi.Client.Data.Dtos
{
    public class EstateObjectDto
    {
        [JsonProperty("MakelaarId")]
        public int EstateAgentId { get; set; }
        [JsonProperty("MakelaarNaam")]
        public string EstateAgentName { get; set; }
    }
}
