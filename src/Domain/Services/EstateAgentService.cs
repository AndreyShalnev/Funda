using Domain.Data;
using Domain.Data.SearchParameters;
using Domain.Repositories;
using Domain.Services.Interfaces;

namespace Domain.Services
{
    public class EstateAgentService : IEstateAgentService
    {
        private readonly IEstateObjectRepository EstateObjectRepository;

        public EstateAgentService(IEstateObjectRepository estateObjectRepository)
        {
            EstateObjectRepository = estateObjectRepository;
        }

        public async Task<Dictionary<EstateAgent, int>> GetAgentsWithEstateObjectsCountOrderedByCount(EstateObjectParameters estateObjectParameters, CancellationToken cancellationToken)
        {
            var estateObjects = await EstateObjectRepository.Get(estateObjectParameters, cancellationToken);
            var groupedObjects = estateObjects.GroupBy(i => i.EstateAgentId);

            var result = groupedObjects
                .Select(i => new { EstateAgentId = i.Key, EstateAgentName = i.First().EstateAgentName, Count = i.Count() })
                .OrderByDescending(i => i.Count)
                .ToDictionary(x => new EstateAgent(x.EstateAgentId, x.EstateAgentName), i => i.Count);

            return result;
        }
    }
}
