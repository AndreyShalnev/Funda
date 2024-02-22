using Domain.Data;
using Domain.Data.SearchParameters;
using FundaApi.Client.Services;
using Adapter.FundaApi.Client.Mappers;
using Domain.Exceptions;
using Domain.Repositories;

namespace Adapter.FundaApi.Client.Repositories
{
    internal class EstateObjectRepository : IEstateObjectRepository
    {
        private readonly IEstateObjectService EstateObjectService;
        
        public EstateObjectRepository(IEstateObjectService estateObjectService)
        {
            EstateObjectService = estateObjectService;
        }

        public async Task<IEnumerable<EstateObject>> Get(EstateObjectParameters parameters, CancellationToken cancellationToken)
        {
            var response = await EstateObjectService.GetEstateAgentsObjects(parameters.City, parameters.PurchaseType.ToString(), parameters.WithGarden, cancellationToken);

            if (response.IsSuccess)
            {
                return response.Result.Select(i => i.ToDomainEntity()).ToList();
            }

            throw new GetDataFailedException(response.Message ?? "EstateObjectService failed to get data", response.Exception);
        }
    }
}
