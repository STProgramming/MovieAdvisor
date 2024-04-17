using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.EntityFrameworkDTOs.AI;
using MADTOs.DTOs.EntityFrameworkDTOs.Identity;
using MADTOs.DTOs.EntityFrameworkDTOs.Movie;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.AI;
using MAModels.EntityFrameworkModels.Identity;
using MAModels.EntityFrameworkModels.Movie;

namespace MAContracts.Contracts.Mappers
{
    public interface IObjectsMapperDtoServices
    {
        MoviesDTO MovieMapperDtoService(Movies movie, List<Images> images, List<Tags> tags);

        List<MoviesDTO> MovieMapperDtoListService(List<Movies> movies, List<Images> images, List<Tags> tags);

        ImagesDTO ImageMapperDtoService(Images image, byte[] data);

        List<ImagesDTO> ImageMapperDtoListService(List<Images> imageList, List<byte[]> imagesData);

        TagsDTO TagMapperDtoService(Tags tag);

        List<TagsDTO> TagMapperDtoListService(List<Tags> tags);

        ReviewsDTO ReviewMapperDtoService(Reviews review);

        UsersDTO UserMapperDtoService(Users user);

        SessionsDTO SessionMapperDtoService(Sessions session, List<Requests> listRequests, List<Recommendations> listRecoms);

        RequestsDTO RequestMapperDtoService(Requests request, List<Recommendations> recommendations);

        RecommendationsDTO RecommendationMapperDtoService(Recommendations recom);

        List<RecommendationsDTO> RecommendationMapperDtoListService(List<Recommendations> listRecoms);
    }
}
