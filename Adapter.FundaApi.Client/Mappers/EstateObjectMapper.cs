using Domain.Data;
using FundaApi.Client.Data.Dtos;

namespace Adapter.FundaApi.Client.Mappers
{
    internal static class EstateObjectMapper
    {
        public static EstateObject ToDomainEntity(this EstateObjectDto dto)
        {
            return new EstateObject(dto.EstateAgentId, dto.EstateAgentName);
        }
    }
}
