using MADTOs.DTOs.EntityFrameworkDTOs.AI;
using MAModels.EntityFrameworkModels.AI;

namespace MAContracts.Contracts.Mappers.AI
{
    public interface ISessionDtoObjectsMapper
    {
        SessionsDTO SessionMappingDto(Sessions session, List<Requests> listRequests, List<Recommendations> listRecommendations);
    }
}
