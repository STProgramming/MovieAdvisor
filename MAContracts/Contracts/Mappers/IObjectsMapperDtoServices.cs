﻿using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.EntityFrameworkDTOs.Identity;
using MADTOs.DTOs.EntityFrameworkDTOs.Movie;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.Identity;
using MAModels.EntityFrameworkModels.Movie;

namespace MAContracts.Contracts.Mappers
{
    public interface IObjectsMapperDtoServices
    {
        MoviesDTO MovieMappingDtoService(Movies movie, List<Images> images, List<Tags> tags);

        ImagesDTO ImageMapperDtoService(Images image, byte[] data);

        List<ImagesDTO> ImageListMapperDtoService(List<Images> imageList, List<byte[]> imagesData);

        TagsDTO TagMapperDtoService(Tags tag);

        List<TagsDTO> TagMapperDtoListService(List<Tags> tags);

        ReviewsDTO ReviewMapperDtoService(Reviews review);

        UsersDTO UserMapperDtoService(Users user);
    }
}
