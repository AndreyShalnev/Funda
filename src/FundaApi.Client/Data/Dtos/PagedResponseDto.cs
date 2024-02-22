using Newtonsoft.Json;

namespace FundaApi.Client.Data.Dtos
{
    public class EstateObjectsPagedDto
    {
        public IEnumerable<EstateObjectDto> Objects { get; set; }
        public int AccountStatus { get; set; }
        public bool EmailNotConfirmed { get; set; }
        public bool ValidationFailed { get; set; }
        public int Website { get; set; }
        public PagingDto Paging { get; set; }

        [JsonProperty("TotaalAantalObjecten")]
        public int TotalNumberOfObjects { get; set; }
    }
}
