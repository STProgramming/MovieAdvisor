using MAContracts.Contracts.Mappers.Movie;
using MADTOs.DTOs.EntityFrameworkDTOs.Movie;
using MAModels.EntityFrameworkModels.Movie;

namespace MAServices.Mappers.Movie
{
    public class ImageDtoObjectsMapper : IImageDtoObjectsMapper
    {
        public ImageDtoObjectsMapper() { }

        public ImagesDTO ImageMappingDto(Images image, byte[] data)
        {
            ImagesDTO imageDTO = new ImagesDTO
            {
                ImageName = image.ImageName,
                ImageExtension = image.ImageExtension,
                ImageData = data
            };
            return imageDTO;
        }

        public List<ImagesDTO> ImageMappingDtoList(List<Images> imageList, List<byte[]> imagesData)
        {
            int counter = 0;
            List<ImagesDTO> imageListDto = new List<ImagesDTO>();
            foreach (var image in imageList)
            {
                imageListDto.Add(ImageMappingDto(image, imagesData[counter]));
                counter++;
            }
            return imageListDto;
        }
    }
}
