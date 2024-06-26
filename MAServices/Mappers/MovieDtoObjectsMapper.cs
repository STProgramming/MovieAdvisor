﻿using MAContracts.Contracts.Mappers;
using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.EntityFrameworkDTOs.Movie;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.Movie;

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
                MovieLifeSpan = movie.MovieLifeSpan,
                Tags = tagsDto,
                Images = imagesDto
            };
            return movieDTO;
        }

        public List<MoviesDTO> MovieMappingDtoList(List<Movies> movies, List<Images> images, List<Tags> tags)
        {
            List<MoviesDTO> resultsDtos = new List<MoviesDTO>();            
            foreach (var movie in movies)
            {
                var imagesList = images.Where(i => i.MovieId == movie.MovieId).ToList();
                var tagsList = tags.Where(t => t.MoviesList.Any(m => m.MovieId == movie.MovieId)).ToList();
                resultsDtos.Add(MovieMappingDto(movie, imagesList, tagsList));
            }
            return resultsDtos;
        }
    }
}
