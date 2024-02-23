using MAContracts.Contracts.Mappers;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

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

        public MovieDTO MovieMappingDtoService(Movie movie, List<Image> images, List<Tag> tags)
        {
            return _movieMapper.MovieMappingDto(movie, images, tags);
        }

        #endregion

        #region IMAGES

        public ImageDTO ImageMapperDtoService(Image image, byte[] data)
        {
            return _imageMapper.ImageMapperDto(image, data);
        }

        public List<ImageDTO> ImageListMapperDtoService(List<Image> imageList, List<byte[]> imagesData)
        {
            return _imageMapper.ImageListMapperDto(imageList, imagesData);
        }

        #endregion

        #region TAGS

        public TagDTO TagMapperDtoService(Tag tag)
        {
            return _tagMapper.TagMapperDto(tag);
        }

        public List<TagDTO> TagMapperDtoListService(List<Tag> tags)
        {
            return _tagMapper.TagMapperDtoList(tags);
        }

        #endregion

        #region REVIEWS

        public ReviewDTO ReviewMapperDtoService(Review review)
        {
            return _reviewMapper.ReviewMapperDto(review);
        }

        #endregion

        #region USERS

        public UserDTO UserMapperDtoService(User user)
        {
            return _userMapper.UserMapperDto(user);
        }

        #endregion
    }
}
