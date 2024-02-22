using Domain.Factories.Interfaces;
using Domain.Services;
using Domain.Services.Interfaces;

namespace Domain.Factories
{
    public class DomainFactory
    {
        private readonly IEstateObjectRepositoryFactory EstateObjectRepositoryFactory;

        public DomainFactory(IEstateObjectRepositoryFactory estateObjectRepositoryFactory) 
        {
            EstateObjectRepositoryFactory = estateObjectRepositoryFactory;
        }

        public IEstateAgentService CreateEstateAgentService()
        {
            var repository = EstateObjectRepositoryFactory.CreateEstateObjectRepository();
            return new EstateAgentService(repository);
        }
    }
}
