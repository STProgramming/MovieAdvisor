using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services.Movie;
using MADTOs.DTOs.EntityFrameworkDTOs.Movie;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.Movie;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MAServices.Services.Movie
{
    public class MultimediaServices : IMultimediaServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _database;

        private readonly IObjectsMapperDtoServices _mapperService;

        public MultimediaServices(IDbContextFactory<ApplicationDbContext> database, 
            IObjectsMapperDtoServices mapperService)
        {
            _database = database;
            _mapperService = mapperService;
        }

        public async Task AddNewMovieImage(List<IFormFile> ImageList, int movieId, List<byte[]> imagesList)
        {
            List<Images> images = new List<Images>();
            using (var ctx = await _database.CreateDbContextAsync())
            {
                var movie = await ctx.Movies.FindAsync(movieId);
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
                    if (ctx.Images.Any(i => string.Equals(i.ImageName, image.FileName))) throw new Exception();
                    await ctx.Images.AddAsync(newImage);
                    await ctx.SaveChangesAsync();
                    images.Add(newImage);
                    counter++;
                }
                movie.ImagesList = images;
                ctx.Movies.Update(movie);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<ImagesDTO> GetMovieImages(int movieId, int counter)
        {
            using (var ctx = await _database.CreateDbContextAsync())
            {
                Movies movie = await ctx.Movies.FindAsync(movieId);
                if (movie == null) throw new NullReferenceException();
                List<Images> images = await ctx.Images.Where(i => i.MovieId == movieId).ToListAsync();
                if (images.Count == 0) throw new NullReferenceException();
                return _mapperService.ImageMapperDtoService(images[counter], images[counter].ImageData);
            }
        }
    }
}
