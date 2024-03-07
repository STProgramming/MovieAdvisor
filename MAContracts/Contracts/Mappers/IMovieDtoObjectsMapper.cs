using MADTOs.DTOs.EntityFrameworkDTOs;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.Movie;

namespace MAContracts.Contracts.Mappers
{
    public interface IMovieDtoObjectsMapper
    {
        public MoviesDTO MovieMappingDto(Movies movie, List<Images> images, List<Tags> tags);
    }
}
