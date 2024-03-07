using MADTOs.DTOs.EntityFrameworkDTOs.Movie;
using MAModels.EntityFrameworkModels.Movie;

namespace MAContracts.Contracts.Mappers.Movie
{
    public interface IImageDtoObjectsMapper
    {
        ImagesDTO ImageMappingDto(Images image, byte[] data);

        List<ImagesDTO> ImageMappingDtoList(List<Images> imageList, List<byte[]> imagesData);
    }
}
