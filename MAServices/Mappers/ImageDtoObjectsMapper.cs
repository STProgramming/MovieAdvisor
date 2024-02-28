using MAContracts.Contracts.Mappers;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAServices.Mappers
{
    public class ImageDtoObjectsMapper : IImageDtoObjectsMapper
    {
        public ImageDtoObjectsMapper() { }

        public ImagesDTO ImageMapperDto(Images image, byte[] data)
        {
            ImagesDTO imageDTO = new ImagesDTO
            {
                ImageName = image.ImageName,
                ImageExtension = image.ImageExtension,
                ImageData = data
            };
            return imageDTO;
        }

        public List<ImagesDTO> ImageListMapperDto(List<Images> imageList, List<byte[]> imagesData)
        {
            int counter = 0;
            List<ImagesDTO> imageListDto = new List<ImagesDTO>();
            foreach(var image in imageList)
            {
                imageListDto.Add(ImageMapperDto(image, imagesData[counter]));
                counter++;
            }
            return imageListDto;
        }
    }
}
