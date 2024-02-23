using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAContracts.Contracts.Mappers
{
    public interface IReviewDtoObjectsMapper
    {
        ReviewDTO ReviewMapperDto(Review review);
    }
}
