using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services.Movie;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MAServices.Services.Movie
{
    public class MultimediaServices : IMultimediaServices
    {
        private readonly ApplicationDbContext _database;

        private readonly IObjectsMapperDtoServices _mapperService;

        public MultimediaServices(ApplicationDbContext database, 
            IObjectsMapperDtoServices mapperService)
        {
            _database = database;
            _mapperService = mapperService;
        }

        public async Task AddNewMovieImage(List<IFormFile> ImageList, int movieId, List<byte[]> imagesList)
        {
            List<Images> images = new List<Images>();
            var movie = await _database.Movies.FindAsync(movieId);
            if (movie == null) throw new ArgumentNullException();
            int counter = 0;
            foreach (var image in ImageList)
            {
                Images newImage = new Images
                {
                    ImageName = image.FileName,
                    ImageExtension = Path.GetExtension(image.FileName),
                    ImageData = imagesList[counter],
                    MovieId = movie.MovieId,
                    Movie = movie
                };
                if (_database.Images.Any(i => string.Equals(i.ImageName, image.Name))) throw new Exception();                 
                await _database.Images.AddAsync(newImage);
                await _database.SaveChangesAsync();
                images.Add(newImage);
                counter++;
            }
            movie.ImagesList = images;
            _database.Movies.Update(movie);
            await _database.SaveChangesAsync();
        }

        public async Task<ImagesDTO> GetMovieImages(int movieId, int counter)
        {
            Movies movie = await _database.Movies.FindAsync(movieId);
            if (movie == null) throw new NullReferenceException();
            List<Images> images = await _database.Images.Where(i => i.MovieId == movieId).ToListAsync();
            if (images.Count == 0) throw new NullReferenceException();
            return _mapperService.ImageMapperDtoService(images[counter], images[counter].ImageData);
        }
    }
}
