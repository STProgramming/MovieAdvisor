using MAAI.Interfaces;
using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAModels.Models;
using MAServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MAServices.MovieServices
{
    public class MovieServices : IMovieServices
    {
        private readonly ApplicationDbContext _database;

        private readonly ITagServices _tagServices;

        public MovieServices(ApplicationDbContext database,
            ITagServices tagServices)
        {
            _database = database;
            _tagServices = tagServices;
        }

        public async Task<List<MovieDTO>> SearchEngine(string Query)
        {
            List<Movie> results = new List<Movie>();            
            if(int.TryParse(Query, out _))
            {
                results = await _database.Movies.Where(m => m.MovieYearProduction == Int16.Parse(Query)).ToListAsync();
                if (results.Count == 0) 
                {
                    results = await _database.Movies.Where(m => m.MovieId == Int32.Parse(Query)).ToListAsync();
                    if (results.Count == 0) results = await _database.Movies.Where(m => m.TagsList.Count > 0 && m.TagsList.Any(t => t.TagId == Int32.Parse(Query))).ToListAsync(); 
                }
            }
            else
            {
                results = await _database.Movies.Where(m => string.Equals(m.MovieTitle.Trim().ToLower(), Query.Trim().ToLower()) || m.MovieTitle.Contains(Query.Trim()) || m.MovieTitle.StartsWith(Query.Trim()) || m.MovieTitle.EndsWith(Query.Trim())).ToListAsync();
                if (results.Count == 0)
                {
                    results = await _database.Movies.Where(m => string.Equals(m.MovieMaker.Trim().ToLower(), Query.Trim().ToLower()) || m.MovieMaker.Contains(Query.Trim()) || m.MovieMaker.StartsWith(Query.Trim()) || m.MovieMaker.EndsWith(Query.Trim())).ToListAsync();
                    if(results.Count == 0) results = await _database.Movies.Where(m => m.MovieDescription.Contains(Query.Trim()) || m.MovieDescription.StartsWith(Query.Trim()) || m.MovieDescription.EndsWith(Query.Trim())).ToListAsync();
                    if (results.Count == 0) results = await _database.Movies.Where(m => m.TagsList.Count > 0 && m.TagsList.Any(t => string.Equals(t.TagName.Trim(), Query.Trim()) || t.TagName.Contains(Query.Trim()) || t.TagName.StartsWith(Query.Trim()) || t.TagName.EndsWith(Query.Trim()))).ToListAsync();
                }
            }
            if (string.IsNullOrEmpty(Query) || results.Count == 0)
            {
                results = await _database.Movies.ToListAsync();
            }
            List<MovieDTO> resultsDtos = new List<MovieDTO>();
            foreach (var result in results)
            {
                MovieDTO movieDTO = new MovieDTO();
                resultsDtos.Add(movieDTO.ConvertToMovieDTO(result));
            }           
            return resultsDtos;
        }

        public async Task<Movie> GetMovieDataById(int movieId)
        {            
            return await _database.Movies.FindAsync(movieId);            
        }

        public async Task CreateNewMovie(MovieDTO newMovie)
        {
            var moviesExist = await IsThisMovieAlreadyInDB(newMovie.MovieTitle, newMovie.MovieYearProduction, newMovie.MovieMaker);
            if (moviesExist != null && moviesExist.Count > 0) throw new IOException();
            Movie newMovieObj = new Movie
            {
                MovieTitle = newMovie.MovieTitle,
                MovieYearProduction = newMovie.MovieYearProduction,
                MovieMaker = newMovie.MovieMaker,
                MovieDescription = newMovie.MovieDescription,
                IsForAdult = newMovie.IsForAdult
            };
            await _database.Movies.AddAsync(newMovieObj);
            await _database.SaveChangesAsync();
            List<Tag> tagsInserted = new List<Tag>();
            if (newMovie.TagsId != null && newMovie.TagsId.Count > 0)
            {
                foreach(int tag in newMovie.TagsId)
                {
                    var tagObj = await _tagServices.GetTag(tag);
                    if (tagObj == null) throw new ArgumentNullException();
                    tagsInserted.Add(tagObj);
                }
            }
            newMovieObj.TagsList = tagsInserted;
            _database.Movies.Update(newMovieObj);
            await _database.SaveChangesAsync();
        }

        public async Task AddNewMovieImage(List<IFormFile> ImageList, int movieId, List<byte[]> imagesList) 
        {
            List<Image> imageList = new List<Image>();
            var movie = await GetMovieDataById(movieId);
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

        private async Task<List<Movie>> IsThisMovieAlreadyInDB(string movieTitle, short movieYearProduction, string movieMaker)
        {
            return await _database.Movies.Where(m => string.Equals(movieTitle.ToLower().Trim(), m.MovieTitle.ToLower().Trim()) && movieYearProduction == m.MovieYearProduction && string.Equals(movieMaker.Trim().ToLower(), m.MovieMaker.Trim().ToLower())).ToListAsync();
        }
    }
}
