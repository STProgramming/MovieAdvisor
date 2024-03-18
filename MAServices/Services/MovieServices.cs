using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services;
using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.ModelsDTOs;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.Movie;
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
                    if (results.Count == 0) results = await _database.Movies.Where(m => m.MovieLifeSpan == int.Parse(Query)).ToListAsync();
                }
            }
            else
            {
                results = await _database.Movies.Where(m => string.Equals(m.MovieTitle.ToLower(), Query.Trim().ToLower()) || m.MovieTitle.ToLower().Contains(Query.Trim().ToLower()) || m.MovieTitle.ToLower().StartsWith(Query.Trim().ToLower()) || m.MovieTitle.ToLower().EndsWith(Query.Trim().ToLower())).ToListAsync();
                if (results.Count == 0)
                {
                    results = await _database.Movies.Where(m => string.Equals(m.MovieMaker.ToLower(), Query.Trim().ToLower()) || m.MovieMaker.ToLower().Contains(Query.Trim().ToLower()) || m.MovieMaker.ToLower().StartsWith(Query.Trim().ToLower()) || m.MovieMaker.ToLower().EndsWith(Query.Trim().ToLower())).ToListAsync();
                    if (results.Count == 0) results = await _database.Movies.Where(m => m.TagsList.Count > 0 && m.TagsList.Any(t => string.Equals(t.TagName.Trim().ToLower(), Query.Trim().ToLower()) || t.TagName.ToLower().Contains(Query.Trim().ToLower()) || t.TagName.ToLower().StartsWith(Query.Trim().ToLower()) || t.TagName.ToLower().EndsWith(Query.Trim().ToLower()))).ToListAsync();
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
                resultsDtos.Add(_mapperService.MovieMapperDtoService(result, images, tags));
            }
            return resultsDtos;
        }

        public async Task<int> CreateNewMovie(NewMovieDTO newMovie)
        {
            var moviesExist = await IsThisMovieAlreadyInDB(newMovie.MovieTitle, newMovie.MovieYearProduction, newMovie.MovieMaker);
            if (moviesExist != null && moviesExist.Count > 0) throw new IOException();
            Movies newMovieObj = new Movies
            {
                MovieTitle = newMovie.MovieTitle.Trim(),
                MovieYearProduction = newMovie.MovieYearProduction,
                MovieMaker = newMovie.MovieMaker.Trim(),
                MovieDescription = newMovie.MovieDescription,
                IsForAdult = newMovie.IsForAdult,
                MovieLifeSpan = newMovie.MovieLifeSpan
            };
            await _database.Movies.AddAsync(newMovieObj);
            await _database.SaveChangesAsync();
            List<Tags> tagsInserted = new List<Tags>();
            if (newMovie.TagsId.Count > 0)
            {
                foreach (int tag in newMovie.TagsId)
                {
                    var tagObj = await _database.Tags.Where(t => t.TagId == tag).FirstOrDefaultAsync();
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
