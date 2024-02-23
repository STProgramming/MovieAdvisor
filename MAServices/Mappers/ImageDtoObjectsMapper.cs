using MAContracts.Contracts.Mappers;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAServices.Mappers
{
    public class ImageDtoObjectsMapper : IImageDtoObjectsMapper
    {
        public ImageDtoObjectsMapper() { }

        public ImageDTO ImageMapperDto(Image image, byte[] data)
        {
            ImageDTO imageDTO = new ImageDTO
            {
                ImageName = image.ImageName,
                ImageExtension = image.ImageExtension,
                ImageData = data
            };
            return imageDTO;
        }

        public List<ImageDTO> ImageListMapperDto(List<Image> imageList, List<byte[]> imagesData)
        {
            int counter = 0;
            List<ImageDTO> imageListDto = new List<ImageDTO>();
            foreach(var image in imageList)
            {
                imageListDto.Add(ImageMapperDto(image, imagesData[counter]));
                counter++;
            }
            return imageListDto;
        }
    }
}
