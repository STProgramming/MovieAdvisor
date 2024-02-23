using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAContracts.Contracts.Mappers
{
    public interface IMovieDtoObjectsMapper
    {
        public MovieDTO MovieMappingDto(Movie movie, List<Image> images, List<Tag> tags);
    }
}
