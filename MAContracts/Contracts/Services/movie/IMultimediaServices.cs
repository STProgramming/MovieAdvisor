using MADTOs.DTOs;
using Microsoft.AspNetCore.Http;

namespace MAContracts.Contracts.Services.Movie
{
    public interface IMultimediaServices
    {
        Task AddNewMovieImage(List<IFormFile> ImageList, int movieId, List<byte[]> imagesList);

        Task<ImagesDTO> GetMovieImages(int movieId, int counter);
    }
}
