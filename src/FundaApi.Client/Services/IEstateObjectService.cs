using FundaApi.Client.Data.Dtos;
using FundaApi.Client.Data;

namespace FundaApi.Client.Services
{
    public interface IEstateObjectService
    {
        Task<ActionResult<IEnumerable<EstateObjectDto>>> GetEstateAgentsObjects(string City, string purchaseType, bool WithTuin, CancellationToken cancellationToken);
    }
}
