using MADTOs.DTOs.EntityFrameworkDTOs.Movie;
using MAModels.EntityFrameworkModels.Movie;

namespace MAContracts.Contracts.Mappers.Movie
{
    public interface IImageDtoObjectsMapper
    {
        ImagesDTO ImageMapperDto(Images image, byte[] data);

        List<ImagesDTO> ImageListMapperDto(List<Images> imageList, List<byte[]> imagesData);
    }
}
