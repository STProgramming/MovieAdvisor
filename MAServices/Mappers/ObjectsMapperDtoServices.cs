using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Mappers.AI;
using MAContracts.Contracts.Mappers.Identity.User;
using MAContracts.Contracts.Mappers.Movie;
using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.EntityFrameworkDTOs.AI;
using MADTOs.DTOs.EntityFrameworkDTOs.Identity;
using MADTOs.DTOs.EntityFrameworkDTOs.Movie;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.AI;
using MAModels.EntityFrameworkModels.Identity;
using MAModels.EntityFrameworkModels.Movie;

namespace MAServices.Mappers
{
    public class ObjectsMapperDtoServices : IObjectsMapperDtoServices
    {
        private readonly IMovieDtoObjectsMapper _movieMapper;

        private readonly IImageDtoObjectsMapper _imageMapper;

        private readonly ITagDtoObjectsMapper _tagMapper;

        private readonly IReviewDtoObjectsMapper _reviewMapper;

        private readonly IUserDtoObjectsMapper _userMapper;

        private readonly ISessionDtoObjectsMapper _sessionMapper;

        private readonly IRequestDtoObjectsMapper _requestMapper;

        private readonly IRecommendationDtoObjectsMapper _recommendMapper;

        public ObjectsMapperDtoServices(IMovieDtoObjectsMapper movieMapper, 
            IImageDtoObjectsMapper imageMapper, 
            ITagDtoObjectsMapper tagMapper, 
            IReviewDtoObjectsMapper reviewMapper, 
            IUserDtoObjectsMapper userMapper,
            ISessionDtoObjectsMapper sessionMapper,
            IRequestDtoObjectsMapper requestMapper,
            IRecommendationDtoObjectsMapper recommendMapper)
        {
            _movieMapper = movieMapper;
            _imageMapper = imageMapper;
            _tagMapper = tagMapper;
            _reviewMapper = reviewMapper;
            _userMapper = userMapper;
            _sessionMapper = sessionMapper;
            _requestMapper = requestMapper;
            _recommendMapper = recommendMapper;
        }

        #region MOVIES

        public MoviesDTO MovieMapperDtoService(Movies movie, List<Images> images, List<Tags> tags)
        {
            return _movieMapper.MovieMappingDto(movie, images, tags);
        }

        public List<MoviesDTO> MovieMapperDtoListService(List<Movies> movies, List<Images> images, List<Tags> tags)
        {
            return _movieMapper.MovieMappingDtoList(movies, images, tags);
        }

        #endregion

        #region IMAGES

        public ImagesDTO ImageMapperDtoService(Images image, byte[] data)
        {
            return _imageMapper.ImageMappingDto(image, data);
        }

        public List<ImagesDTO> ImageMapperDtoListService(List<Images> imageList, List<byte[]> imagesData)
        {
            return _imageMapper.ImageMappingDtoList(imageList, imagesData);
        }

        #endregion

        #region TAGS

        public TagsDTO TagMapperDtoService(Tags tag)
        {
            return _tagMapper.TagMappingDto(tag);
        }

        public List<TagsDTO> TagMapperDtoListService(List<Tags> tags)
        {
            return _tagMapper.TagMappingDtoList(tags);
        }

        #endregion

        #region REVIEWS

        public ReviewsDTO ReviewMapperDtoService(Reviews review)
        {
            return _reviewMapper.ReviewMappingDto(review);
        }

        #endregion

        #region USERS

        public UsersDTO UserMapperDtoService(Users user)
        {
            return _userMapper.UserMappingDto(user);
        }

        #endregion

        #region SESSIONS

        public SessionsDTO SessionMapperDtoService(Sessions session, List<Requests> listRequests, List<Recommendations> listRecoms)
        {
            return _sessionMapper.SessionMappingDto(session, listRequests, listRecoms);
        }

        #endregion

        #region REQUESTS

        public RequestsDTO RequestMapperDtoService(Requests request, List<Recommendations> recommendations)
        {
            return _requestMapper.RequestMappingDto(request, recommendations);
        }

        #endregion

        #region RECOMMENDATIONS

        public RecommendationsDTO RecommendationMapperDtoService(Recommendations recom)
        {
            return _recommendMapper.RecommendationMappingDto(recom);
        }

        public List<RecommendationsDTO> RecommendationMapperDtoListService(List<Recommendations> listRecoms)
        {
            return _recommendMapper.RecommendationMappingDtoList(listRecoms);
        }

        #endregion
    }
}
