using Adapter.FundaApi.Client.Repositories;
using Domain.Factories.Interfaces;
using Domain.Repositories;
using FundaApi.Client;

namespace Adapter.FundaApi.Client
{
    public sealed class FundaApiAdapterFactory : IEstateObjectRepositoryFactory
    {
        private readonly FundaApiClientFactory FundaApiClientFactory;

        public FundaApiAdapterFactory(FundaApiClientFactory fundaApiClientFactory)
        {
            FundaApiClientFactory = fundaApiClientFactory;
        }

        public IEstateObjectRepository CreateEstateObjectRepository()
        {
            var estateObjectService = FundaApiClientFactory.CreateEstateObjectService();
            return new EstateObjectRepository(estateObjectService);
        }
    }
}
