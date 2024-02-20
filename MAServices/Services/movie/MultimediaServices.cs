using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;
using MAServices.Interfaces.movie;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MAServices.Services.movie
{
    public class MultimediaServices : IMultimediaServices
    {
        private readonly ApplicationDbContext _database;

        private readonly IMovieServices _movieServices;

        public MultimediaServices(
            ApplicationDbContext database, IMovieServices movieServices, IFileServices fileServices)
        {
            _database = database;
            _movieServices = movieServices;
        }

        public async Task AddNewMovieImage(List<IFormFile> ImageList, int movieId, List<byte[]> imagesList)
        {
            List<Image> imageList = new List<Image>();
            var movie = await _movieServices.GetMovieDataById(movieId);
            if (movie == null) throw new ArgumentNullException();
            int counter = 0;
            foreach (var image in ImageList)
            {
                ImageDTO MovieImage = new ImageDTO(image.FileName, imagesList.ElementAt(counter), movieId, movie);
                await _database.Images.AddAsync(MovieImage);
                await _database.SaveChangesAsync();
                imageList.Add(MovieImage);
                counter++;
            }
            movie.ImagesList = imageList;
            _database.Movies.Update(movie);
            await _database.SaveChangesAsync();
        }

        public async Task<ImageDTO> GetMovieImages(int movieId)
        {
            Movie movie = await _movieServices.GetMovieDataById(movieId);
            if (movie == null) throw new NullReferenceException();
            List<Image> images = await _database.Images.Where(i => i.MovieId == movieId).ToListAsync();
            ImageDTO image = new ImageDTO(images[0]);
            return image;
        }
    }
}
