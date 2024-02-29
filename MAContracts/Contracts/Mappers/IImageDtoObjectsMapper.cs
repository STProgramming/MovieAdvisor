using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAContracts.Contracts.Mappers
{
    public interface IImageDtoObjectsMapper
    {
        ImagesDTO ImageMapperDto(Images image, byte[] data);

        List<ImagesDTO> ImageListMapperDto(List<Images> imageList, List<byte[]> imagesData);
    }
}
