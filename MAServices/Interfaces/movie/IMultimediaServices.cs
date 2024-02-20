using MAModels.DTO;
using Microsoft.AspNetCore.Http;

namespace MAServices.Interfaces.movie
{
    public interface IMultimediaServices
    {
        Task AddNewMovieImage(List<IFormFile> ImageList, int movieId, List<byte[]> imagesList);

        Task<ImageDTO> GetMovieImages(int movieId);
    }
}
