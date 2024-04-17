using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services;
using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.ModelsDTOs;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.Movie;
using MAModels.Models;
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

        public async Task<MoviesSearchResultsDTO> SearchEngine(string Query, short page, short elementsViewed)
        {
            MovieSearchResults results = new MovieSearchResults();
            if (int.TryParse(Query, out _))
            {
                results.ResultsForYear = await _database.Movies.Where(m => m.MovieYearProduction == short.Parse(Query)).ToListAsync();
                results.ResultsForLifeSpan = await _database.Movies.Where(m => m.MovieLifeSpan == int.Parse(Query)).ToListAsync();
                results.ResultsCount = results.ResultsForYear.Count + results.ResultsForLifeSpan.Count;
            }
            else if (!string.IsNullOrEmpty(Query) && !string.IsNullOrWhiteSpace(Query))
            {
                results.ResultsForTitle = await _database.Movies.Where(m => string.Equals(m.MovieTitle.ToLower(), Query.Trim().ToLower()) || m.MovieTitle.ToLower().Contains(Query.Trim().ToLower()) || m.MovieTitle.ToLower().StartsWith(Query.Trim().ToLower()) || m.MovieTitle.ToLower().EndsWith(Query.Trim().ToLower())).ToListAsync();
                results.ResultsForMaker = await _database.Movies.Where(m => string.Equals(m.MovieMaker.ToLower(), Query.Trim().ToLower()) || m.MovieMaker.ToLower().Contains(Query.Trim().ToLower()) || m.MovieMaker.ToLower().StartsWith(Query.Trim().ToLower()) || m.MovieMaker.ToLower().EndsWith(Query.Trim().ToLower())).ToListAsync();
                results.ResultsForTag = await _database.Movies.Where(m => m.TagsList.Count > 0 && m.TagsList.Any(t => string.Equals(t.TagName.Trim().ToLower(), Query.Trim().ToLower()) || t.TagName.ToLower().Contains(Query.Trim().ToLower()) || t.TagName.ToLower().StartsWith(Query.Trim().ToLower()) || t.TagName.ToLower().EndsWith(Query.Trim().ToLower()))).ToListAsync();
                results.ResultsForDescription = await _database.Movies.Where(m => m.MovieDescription.ToLower().Contains(Query.Trim().ToLower()) || string.Equals(m.MovieDescription.Trim().ToLower(), Query.Trim().ToLower())).ToListAsync();
                results.ResultsCount = results.ResultsForTitle.Count + results.ResultsForMaker.Count + results.ResultsForTag.Count + results.ResultsForDescription.Count; 
            }
            else if (string.IsNullOrEmpty(Query) || string.IsNullOrWhiteSpace(Query) || results.ResultsCount == 0)
            {
                results.Movies = await _database.Movies.ToListAsync();
            }
            results.MoviesCount = await _database.Movies.CountAsync();
            return await MoviesSearchResultsDtoMapper(results, page, elementsViewed);
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

        private async Task<MoviesSearchResultsDTO> MoviesSearchResultsDtoMapper(MovieSearchResults results, short page, short elementsViewed)
        {
            List<Images> imagesList = await _database.Images.ToListAsync();

            List<Tags> tagsList = await _database.Tags.ToListAsync();

            int elementsMax = page * elementsViewed;

            int initialMin = elementsMax - elementsViewed;

            List<Movies> movs = new List<Movies>();

            List<Movies> forYear = new List<Movies>();

            List<Movies> forLifespan = new List<Movies>();

            List<Movies> forTitle = new List<Movies>();

            List<Movies> forMaker = new List<Movies>();

            List<Movies> forTag = new List<Movies>();

            List<Movies> forDescr = new List<Movies>();

            for (int i = initialMin; i < elementsMax; i++)
            {
                if(results.Movies.Count > 0 && i < results.Movies.Count)
                    movs.Add(results.Movies[i]);
                if(results.ResultsForYear.Count > 0 && i < results.ResultsForYear.Count)
                    forYear.Add(results.ResultsForYear[i]);
                if (results.ResultsForLifeSpan.Count > 0 && i < results.ResultsForLifeSpan.Count)
                    forLifespan.Add(results.ResultsForLifeSpan[i]);
                if (results.ResultsForTitle.Count > 0 && i < results.ResultsForTitle.Count)
                    forTitle.Add(results.ResultsForTitle[i]);
                if (results.ResultsForMaker.Count > 0 && i < results.ResultsForMaker.Count)
                    forMaker.Add(results.ResultsForMaker[i]);
                if (results.ResultsForTag.Count > 0 && i < results.ResultsForTag.Count)
                    forTag.Add(results.ResultsForTag[i]);
                if (results.ResultsForDescription.Count > 0 && i < results.ResultsForDescription.Count)
                    forDescr.Add(results.ResultsForDescription[i]);
            }

            MoviesSearchResultsDTO dto = new MoviesSearchResultsDTO();

            dto.MoviesCount = results.MoviesCount;

            dto.Movies = _mapperService.MovieMapperDtoListService(movs, imagesList, tagsList);

            dto.ResultsForYear = _mapperService.MovieMapperDtoListService(forYear, imagesList, tagsList);

            dto.ResultsForLifeSpan = _mapperService.MovieMapperDtoListService(forLifespan, imagesList, tagsList);

            dto.ResultsForTitle = _mapperService.MovieMapperDtoListService(forTitle, imagesList, tagsList);

            dto.ResultsForMaker = _mapperService.MovieMapperDtoListService(forMaker, imagesList, tagsList);

            dto.ResultsForTag = _mapperService.MovieMapperDtoListService(forTag, imagesList, tagsList);

            dto.ResultsForDescription = _mapperService.MovieMapperDtoListService(forDescr, imagesList, tagsList);

            return dto;
        }

        #endregion

    }
}
