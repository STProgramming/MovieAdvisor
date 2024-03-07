using MADTOs.DTOs.EntityFrameworkDTOs;
using MAModels.EntityFrameworkModels;

namespace MAContracts.Contracts.Mappers
{
    public interface IReviewDtoObjectsMapper
    {
        ReviewsDTO ReviewMappingDto(Reviews review);
    }
}
