using MADTOs.DTOs.EntityFrameworkDTOs.AI;
using MAModels.EntityFrameworkModels.AI;

namespace MAContracts.Contracts.Mappers.AI
{
    public interface IRequestDtoObjectsMapper
    {
        RequestsDTO RequestMappingDto(Requests request, List<Recommendations> recoms);
    }
}
