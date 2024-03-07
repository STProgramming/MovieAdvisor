using MAContracts.Contracts.Mappers.AI;
using MADTOs.DTOs.EntityFrameworkDTOs.AI;
using MAModels.EntityFrameworkModels.AI;

namespace MAServices.Mappers.AI
{
    public class RecommendationDtoObjectsMapper : IRecommendationDtoObjectsMapper
    {
        public RecommendationDtoObjectsMapper() { }

        public RecommendationsDTO RecommendationMappingDto(Recommendations recom)
        {
            RecommendationsDTO recomDTO = new RecommendationsDTO
            {
                RecommendationId = recom.RecommendationId,
                MovieId = recom.MovieId,
                MovieTitle = recom.MovieTitle,
                Name = recom.Name,
                LastName = recom.LastName,
                Email = recom.Email,
                AiScore = recom.AiScore,
                See = recom.See,
            };
            return recomDTO;
        }

        public List<RecommendationsDTO> RecommendationMappingDtoList(List<Recommendations> listRecoms)
        {
            List<RecommendationsDTO> resultDto = new List<RecommendationsDTO>();
            foreach(var recom in listRecoms)
            {
                resultDto.Add(RecommendationMappingDto(recom));
            }
            return resultDto;
        }
    }
}
