using MADTOs.DTOs.EntityFrameworkDTOs.AI;
using MAModels.EntityFrameworkModels.AI;

namespace MAContracts.Contracts.Mappers.AI
{
    public interface IRecommendationDtoObjectsMapper
    {
        RecommendationsDTO RecommendationMappingDto(Recommendations recom);

        List<RecommendationsDTO> RecommendationMappingDtoList(List<Recommendations> listRecoms);
    }
}
