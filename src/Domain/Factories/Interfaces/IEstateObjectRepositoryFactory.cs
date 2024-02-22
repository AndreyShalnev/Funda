using Domain.Repositories;

namespace Domain.Factories.Interfaces
{
    public interface IEstateObjectRepositoryFactory
    {
        IEstateObjectRepository CreateEstateObjectRepository();
    }
}
