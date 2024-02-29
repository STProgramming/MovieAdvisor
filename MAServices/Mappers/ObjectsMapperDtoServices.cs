using MAContracts.Contracts.Mappers;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.Identity;

namespace MAServices.Mappers
{
    public class ObjectsMapperDtoServices : IObjectsMapperDtoServices
    {
        private readonly IMovieDtoObjectsMapper _movieMapper;

        private readonly IImageDtoObjectsMapper _imageMapper;

        private readonly ITagDtoObjectsMapper _tagMapper;

        private readonly IReviewDtoObjectsMapper _reviewMapper;

        private readonly IUserDtoObjectsMapper _userMapper;

        public ObjectsMapperDtoServices(IMovieDtoObjectsMapper movieMapper, 
            IImageDtoObjectsMapper imageMapper, 
            ITagDtoObjectsMapper tagMapper, 
            IReviewDtoObjectsMapper reviewMapper, 
            IUserDtoObjectsMapper userMapper)
        {
            _movieMapper = movieMapper;
            _imageMapper = imageMapper;
            _tagMapper = tagMapper;
            _reviewMapper = reviewMapper;
            _userMapper = userMapper;
        }

        #region MOVIES

        public MoviesDTO MovieMappingDtoService(Movies movie, List<Images> images, List<Tags> tags)
        {
            return _movieMapper.MovieMappingDto(movie, images, tags);
        }

        #endregion

        #region IMAGES

        public ImagesDTO ImageMapperDtoService(Images image, byte[] data)
        {
            return _imageMapper.ImageMapperDto(image, data);
        }

        public List<ImagesDTO> ImageListMapperDtoService(List<Images> imageList, List<byte[]> imagesData)
        {
            return _imageMapper.ImageListMapperDto(imageList, imagesData);
        }

        #endregion

        #region TAGS

        public TagsDTO TagMapperDtoService(Tags tag)
        {
            return _tagMapper.TagMapperDto(tag);
        }

        public List<TagsDTO> TagMapperDtoListService(List<Tags> tags)
        {
            return _tagMapper.TagMapperDtoList(tags);
        }

        #endregion

        #region REVIEWS

        public ReviewsDTO ReviewMapperDtoService(Reviews review)
        {
            return _reviewMapper.ReviewMapperDto(review);
        }

        #endregion

        #region USERS

        public UsersDTO UserMapperDtoService(Users user)
        {
            return _userMapper.UserMapperDto(user);
        }

        #endregion
    }
}
