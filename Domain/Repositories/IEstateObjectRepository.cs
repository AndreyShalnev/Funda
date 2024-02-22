using Domain.Data;
using Domain.Data.SearchParameters;

namespace Domain.Repositories
{
    public interface IEstateObjectRepository
    {
        Task<IEnumerable<EstateObject>> Get(EstateObjectParameters parameters, CancellationToken cancellationToken);
    }
}
