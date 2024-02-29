using MAContracts.Contracts.Mappers;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MADTOs.Mappers
{
    public class MovieDtoObjectsMapper : IMovieDtoObjectsMapper
    {
        public MovieDtoObjectsMapper() { }

        public MoviesDTO MovieMappingDto(Movies movie, List<Images> images, List<Tags> tags)
        {
            List<ImagesDTO> imagesDto = new List<ImagesDTO>();
            images.ForEach(image =>
            {
                var imageDTO = new ImagesDTO
                {
                    ImageName = image.ImageName,
                    ImageExtension = image.ImageExtension
                };
                imagesDto.Add(imageDTO);
            });
            List<TagsDTO> tagsDto = new List<TagsDTO>();
            tags.ForEach(tag =>
            {
                var tagDTO = new TagsDTO
                {
                    TagId = tag.TagId,
                    TagName = tag.TagName
                };
                tagsDto.Add(tagDTO);
            });
            MoviesDTO movieDTO = new MoviesDTO
            {
                MovieId = movie.MovieId,
                MovieTitle = movie.MovieTitle,
                MovieYearProduction = movie.MovieYearProduction,
                MovieDescription = movie.MovieDescription,
                MovieMaker = movie.MovieMaker,
                IsForAdult = movie.IsForAdult,
                Tags = tagsDto,
                Images = imagesDto
            };
            return movieDTO;
        }
    }
}
