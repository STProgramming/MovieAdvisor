using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAContracts.Contracts.Mappers
{
    public interface IObjectsMapperDtoServices
    {
        MovieDTO MovieMappingDtoService(Movie movie, List<Image> images, List<Tag> tags);

        ImageDTO ImageMapperDtoService(Image image, byte[] data);

        List<ImageDTO> ImageListMapperDtoService(List<Image> imageList, List<byte[]> imagesData);

        TagDTO TagMapperDtoService(Tag tag);

        List<TagDTO> TagMapperDtoListService(List<Tag> tags);

        ReviewDTO ReviewMapperDtoService(Review review);

        UserDTO UserMapperDtoService(User user);
    }
}
