using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAContracts.Contracts.Mappers
{
    public interface IImageDtoObjectsMapper
    {
        ImageDTO ImageMapperDto(Image image, byte[] data);

        List<ImageDTO> ImageListMapperDto(List<Image> imageList, List<byte[]> imagesData);
    }
}
