using Domain.Data;
using Domain.Data.SearchParameters;

namespace Domain.Services.Interfaces
{
    public interface IEstateAgentService
    {
        Task<Dictionary<EstateAgent, int>> GetAgentsWithEstateObjectsCountOrderedByCount(EstateObjectParameters estateObjectParameters, CancellationToken cancellationToken);
    }
}
