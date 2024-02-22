using FundaApi.Client.Data;
using FundaApi.Client.Data.Dtos;

namespace FundaApi.Client.Client.Interfaces
{
    interface IEstateObjectsClient
    {
        Task<ActionResult<EstateObjectsPagedDto>> GetEstateObjects(SearchEstateObjectsParameters parameters, CancellationToken cancellationToken);
    }
}
