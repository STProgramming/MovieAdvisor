using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services;
using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.ModelsDTOs;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.Identity;
using MAModels.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MAServices.Services
{
    public class ReviewServices : IReviewServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _database;

        private readonly IObjectsMapperDtoServices _mapperService;

        private readonly UserManager<Users> _userManager;

        public ReviewServices(IDbContextFactory<ApplicationDbContext> database,
            IObjectsMapperDtoServices mapperService,
            UserManager<Users> userManager)
        {
            _database = database;
            _mapperService = mapperService;
            _userManager = userManager;
        }

        #region PUBLIC SERVICES

        public async Task<List<ReviewsDTO>?> SearchEngineReviews(string? userId, string? movieTitle)
        {
            List<ReviewsDTO> ReviewsList = new List<ReviewsDTO>();
            List<Reviews> Reviews = new List<Reviews>();
            if (userId == null && string.IsNullOrEmpty(movieTitle))
            {
                using (var ctx = await _database.CreateDbContextAsync())
                {
                    Reviews = await GetReviews();
                    foreach (var review in Reviews)
                    {
                        var userObj = await _userManager.FindByIdAsync(review.UserId);
                        var movieObj = await ctx.Movies.Where(m => m.MovieId == review.MovieId).FirstOrDefaultAsync();
                        review.User = userObj == null ? throw new NullReferenceException() : userObj;
                        review.Movie = movieObj == null ? throw new NullReferenceException() : movieObj;
                        //Se non è loggato non può vedere le descrizione dei voti, potrebbe essere spoilerare parti del film
                        review.DescriptionVote = String.Empty;
                        ReviewsList.Add(_mapperService.ReviewMapperDtoService(review));
                    }
                }
            }
            else if (userId == null && !string.IsNullOrEmpty(movieTitle))
            {
                using (var ctx = await _database.CreateDbContextAsync())
                {
                    var movies = await ctx.Movies.Where(m => m.MovieTitle.ToLower().Contains(movieTitle.Trim().ToLower()) || string.Equals(m.MovieTitle.ToLower(), movieTitle.Trim().ToLower())).ToListAsync();
                    if (movies == null || movies.Count == 0) throw new NullReferenceException();
                    foreach (var movie in movies)
                    {
                        var reviewsFromMovie = await GetReviewsOfMovie(movie.MovieId);
                        reviewsFromMovie.ForEach(rev =>
                        {
                            Reviews.Add(rev);
                        });
                    }
                    foreach (var review in Reviews)
                    {
                        var userObj = await _userManager.FindByIdAsync(review.UserId);
                        review.User = userObj != null ? userObj : throw new NullReferenceException();
                        var reviewObj = await ctx.Reviews.Where(r => r.ReviewId == review.ReviewId).FirstOrDefaultAsync();
                        review.Movie = reviewObj.Movie;
                        review.DescriptionVote = string.Empty;
                        ReviewsList.Add(_mapperService.ReviewMapperDtoService(review));
                    }
                }
            }
            else if (userId != null && string.IsNullOrEmpty(movieTitle))
            {                
                var user = await _userManager.FindByIdAsync(userId);                
                if (user == null) throw new NullReferenceException();
                Reviews = await GetReviewsOfUser(user.Id);
                using (var ctx = await _database.CreateDbContextAsync())
                {
                    foreach (var review in Reviews)
                    {
                        var movieObj = await ctx.Movies.FindAsync(review.MovieId);
                        review.User = user;
                        review.Movie = movieObj != null ? movieObj : throw new NullReferenceException();
                        if (review.MovieId != movieObj.MovieId)
                            review.DescriptionVote = string.Empty;
                        ReviewsList.Add(_mapperService.ReviewMapperDtoService(review));
                    }
                }
            }
            else if (userId != null && !string.IsNullOrEmpty(movieTitle))
            {
                List<Reviews> result = null;
                using (var ctx = await _database.CreateDbContextAsync())
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    var movies = await ctx.Movies.Where(m => m.MovieTitle.ToLower().Contains(movieTitle.Trim().ToLower()) || string.Equals(m.MovieTitle.ToLower(), movieTitle.Trim().ToLower())).ToListAsync();
                    if (user == null || movies == null || movies.Count == 0) throw new NullReferenceException();
                    foreach(var movie in movies)
                    {
                        var reviewPerMovie = await GetYourRiviewOfMovie(user.Id, movie.MovieId);
                        if (reviewPerMovie != null) result.Add(reviewPerMovie);
                    }
                
                    if (result != null && result.Count > 0)
                    {
                        Reviews = result.ToList();
                        foreach (var review in Reviews)
                        {
                            var reviewObj = await ctx.Reviews.Where(r => r.ReviewId == review.ReviewId).FirstOrDefaultAsync();
                            review.Movie = reviewObj.Movie;
                            review.User = user;
                            ReviewsList.Add(_mapperService.ReviewMapperDtoService(review));
                        }
                    }
                }
            }
            return ReviewsList;
        }

        public async Task PostNewReview(string userId, NewReviewDTO newReviewDto)
        {
            using (var ctx = await _database.CreateDbContextAsync())
            {
                var user = await _userManager.FindByIdAsync(userId);
                var movie = await ctx.Movies.FindAsync(newReviewDto.MovieId);
                if (movie == null || user == null) throw new NullReferenceException();
                if (movie.MovieYearProduction > Convert.ToDateTime(newReviewDto.When).Year) throw new Exception();
                var movieReviewed = await GetYourRiviewOfMovie(user.Id, movie.MovieId);
                if (movieReviewed != null) throw new ConflictException();
                var recommendation = await ctx.Recommendations.Where(r => string.Equals(user.Email, r.Email) && r.MovieId == movie.MovieId).FirstOrDefaultAsync();
                if (recommendation != null)
                {
                    recommendation.See = true;
                    ctx.Recommendations.Update(recommendation);
                }

                Reviews newReview = new Reviews
                {
                    UserId = user.Id,
                    MovieId = movie.MovieId,
                    Movie = movie,
                    User = user,
                    Vote = newReviewDto.Vote,
                    DescriptionVote = newReviewDto.DescriptionVote,
                    DateTimeVote = newReviewDto.When == null ? DateTime.Now : Convert.ToDateTime(newReviewDto.When)
                };
                await ctx.Reviews.AddAsync(newReview);
                user.ReviewsList.Add(newReview);
                ctx.Movies.Update(movie);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task EditReview(string userId, int reviewId, NewReviewDTO reviewModified)
        {
            using (var ctx = await _database.CreateDbContextAsync())
            {
                var user = await _userManager.FindByIdAsync(userId);
                var movie = await ctx.Movies.FindAsync(reviewModified.MovieId);
                if (movie == null || user == null) throw new NullReferenceException();
                if (movie.MovieYearProduction > Convert.ToDateTime(reviewModified.When).Year) throw new Exception();
                var movieReviewed = await GetYourRiviewOfMovie(user.Id, movie.MovieId);
                if (movieReviewed == null) throw new NullReferenceException();

                movieReviewed.UserId = user.Id;
                movieReviewed.MovieId = movie.MovieId;
                movieReviewed.Movie = movie;
                movieReviewed.User = user;
                movieReviewed.Vote = reviewModified.Vote;
                movieReviewed.DescriptionVote = reviewModified.DescriptionVote;
                movieReviewed.DateTimeVote = reviewModified.When == null ? DateTime.Now : Convert.ToDateTime(reviewModified.When);
                ctx.Reviews.Update(movieReviewed);
                user.ReviewsList.Add(movieReviewed);
                await ctx.SaveChangesAsync();
            }
        }

            #endregion

            #region PRIVATE METHODS

        private async Task<List<Reviews>> GetReviews()
        {
            using (var ctx = await _database.CreateDbContextAsync())
            {
                return await ctx.Reviews.ToListAsync();
            }
        }

        private async Task<List<Reviews>> GetReviewsOfUser(string userId)
        {
            using (var ctx = await _database.CreateDbContextAsync())
            {
                return await ctx.Reviews.Where(r => string.Equals(r.UserId, userId)).ToListAsync();
            }
        }

        private async Task<List<Reviews>> GetReviewsOfMovie(int movieId)
        {
            using (var ctx = await _database.CreateDbContextAsync())
            {
                return await ctx.Reviews.Where(r => r.MovieId == movieId).ToListAsync();
            }
        }

        private async Task<Reviews> GetYourRiviewOfMovie(string userId, int movieId)
        {
            using (var ctx = await _database.CreateDbContextAsync())
            {
                return await ctx.Reviews.Where(r => r.MovieId == movieId && string.Equals(r.UserId, userId)).FirstOrDefaultAsync();
            }
        }

        #endregion
    }
}
