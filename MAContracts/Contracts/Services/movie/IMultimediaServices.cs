using MADTOs.DTOs;
using Microsoft.AspNetCore.Http;

namespace MAContracts.Contracts.Services.movie
{
    public interface IMultimediaServices
    {
        Task AddNewMovieImage(List<IFormFile> ImageList, int movieId, List<byte[]> imagesList);

        Task<ImageDTO> GetMovieImages(int movieId, int counter);
    }
}
