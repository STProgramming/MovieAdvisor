using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAContracts.Contracts.Mappers
{
    public interface IMovieDtoObjectsMapper
    {
        public MoviesDTO MovieMappingDto(Movies movie, List<Images> images, List<Tags> tags);
    }
}
