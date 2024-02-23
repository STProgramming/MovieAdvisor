using MAContracts.Contracts.Mappers;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MADTOs.Mappers
{
    public class MovieDtoObjectsMapper : IMovieDtoObjectsMapper
    {
        public MovieDtoObjectsMapper() { }

        public MovieDTO MovieMappingDto(Movie movie, List<Image> images, List<Tag> tags)
        {
            List<ImageDTO> imagesDto = new List<ImageDTO>();
            images.ForEach(image =>
            {
                var imageDTO = new ImageDTO
                {
                    ImageName = image.ImageName,
                    ImageExtension = image.ImageExtension
                };
                imagesDto.Add(imageDTO);
            });
            List<TagDTO> tagsDto = new List<TagDTO>();
            tags.ForEach(tag =>
            {
                var tagDTO = new TagDTO
                {
                    TagId = tag.TagId,
                    TagName = tag.TagName
                };
                tagsDto.Add(tagDTO);
            });
            MovieDTO movieDTO = new MovieDTO
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
