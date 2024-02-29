using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;
using Microsoft.EntityFrameworkCore;

namespace MAServices.Services
{
    public class MovieServices : IMovieServices
    {
        private readonly ApplicationDbContext _database;

        private readonly IObjectsMapperDtoServices _mapperService;

        public MovieServices(ApplicationDbContext database,
            IObjectsMapperDtoServices mapperService)
        {
            _database = database;
            _mapperService = mapperService;
        }

        #region PUBLIC SERVICES

        public async Task<List<MoviesDTO>> SearchEngine(string Query)
        {

            List<Movies> results = new List<Movies>();
            if (int.TryParse(Query, out _))
            {
                results = await _database.Movies.Where(m => m.MovieYearProduction == short.Parse(Query)).ToListAsync();
                if (results.Count == 0)
                {
                    results = await _database.Movies.Where(m => m.MovieId == int.Parse(Query)).ToListAsync();
                    if (results.Count == 0) results = await _database.Movies.Where(m => m.TagsList.Count > 0 && m.TagsList.Any(t => t.TagId == int.Parse(Query))).ToListAsync();
                }
            }
            else
            {
                results = await _database.Movies.Where(m => string.Equals(m.MovieTitle.Trim().ToLower(), Query.Trim().ToLower()) || m.MovieTitle.Contains(Query.Trim()) || m.MovieTitle.StartsWith(Query.Trim()) || m.MovieTitle.EndsWith(Query.Trim())).ToListAsync();
                if (results.Count == 0)
                {
                    results = await _database.Movies.Where(m => string.Equals(m.MovieMaker.Trim().ToLower(), Query.Trim().ToLower()) || m.MovieMaker.Contains(Query.Trim()) || m.MovieMaker.StartsWith(Query.Trim()) || m.MovieMaker.EndsWith(Query.Trim())).ToListAsync();
                    if (results.Count == 0) results = await _database.Movies.Where(m => m.MovieDescription.Contains(Query.Trim()) || m.MovieDescription.StartsWith(Query.Trim()) || m.MovieDescription.EndsWith(Query.Trim())).ToListAsync();
                    if (results.Count == 0) results = await _database.Movies.Where(m => m.TagsList.Count > 0 && m.TagsList.Any(t => string.Equals(t.TagName.Trim(), Query.Trim()) || t.TagName.Contains(Query.Trim()) || t.TagName.StartsWith(Query.Trim()) || t.TagName.EndsWith(Query.Trim()))).ToListAsync();
                }
            }
            if (string.IsNullOrEmpty(Query) || results.Count == 0)
            {
                results = await _database.Movies.ToListAsync();
            }
            List<MoviesDTO> resultsDtos = new List<MoviesDTO>();
            foreach (var result in results)
            {
                MoviesDTO movieDTO = new MoviesDTO();
                List<Images> images = await _database.Images.Where(i => i.MovieId == result.MovieId).ToListAsync();
                List<Tags> tags = await _database.Tags.Where(t => t.MoviesList.Any(m => m.MovieId == result.MovieId)).ToListAsync();
                resultsDtos.Add(_mapperService.MovieMappingDtoService(result, images, tags));
            }
            return resultsDtos;
        }

        public async Task<int> CreateNewMovie(MoviesDTO newMovie)
        {
            var moviesExist = await IsThisMovieAlreadyInDB(newMovie.MovieTitle, newMovie.MovieYearProduction, newMovie.MovieMaker);
            if (moviesExist != null && moviesExist.Count > 0) throw new IOException();
            Movies newMovieObj = new Movies
            {
                MovieTitle = newMovie.MovieTitle,
                MovieYearProduction = newMovie.MovieYearProduction,
                MovieMaker = newMovie.MovieMaker,
                MovieDescription = newMovie.MovieDescription,
                IsForAdult = newMovie.IsForAdult
            };
            await _database.Movies.AddAsync(newMovieObj);
            await _database.SaveChangesAsync();
            List<Tags> tagsInserted = new List<Tags>();
            if (newMovie.Tags.Count > 0)
            {
                foreach (var tag in newMovie.Tags)
                {
                    var tagObj = await _database.Tags.FindAsync(tag.TagId);
                    if (tagObj == null) throw new ArgumentNullException();
                    tagsInserted.Add(tagObj);
                }
            }
            newMovieObj.TagsList = tagsInserted;
            _database.Movies.Update(newMovieObj);
            await _database.SaveChangesAsync();
            return newMovieObj.MovieId;
        }

        #endregion

        #region PRIVATE SERVICES

        private async Task<List<Movies>> IsThisMovieAlreadyInDB(string movieTitle, short movieYearProduction, string movieMaker)
        {
            return await _database.Movies.Where(m => string.Equals(movieTitle.ToLower().Trim(), m.MovieTitle.ToLower().Trim()) && movieYearProduction == m.MovieYearProduction && string.Equals(movieMaker.Trim().ToLower(), m.MovieMaker.Trim().ToLower())).ToListAsync();
        }

        #endregion

    }
}
