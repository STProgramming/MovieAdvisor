using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;
using Microsoft.EntityFrameworkCore;

namespace MAServices.Services
{
    public class ReviewServices : IReviewServices
    {
        private readonly ApplicationDbContext _database;

        private readonly IObjectsMapperDtoServices _mapperService;

        public ReviewServices(ApplicationDbContext database,
            IObjectsMapperDtoServices mapperService)
        {
            _database = database;
            _mapperService = mapperService;
        }

        #region PUBLIC SERVICES

        public async Task<List<ReviewDTO>?> SearchEngineReviews(string? userEmail, int? movieId)
        {
            List<ReviewDTO> ReviewsList = new List<ReviewDTO>();
            List<Review> Reviews = new List<Review>();
            if (userEmail == null && movieId == null || movieId == 0)
            {
                Reviews = await GetReviews();
                foreach (var review in Reviews)
                {
                    var userObj = await _database.Users.Where(u => u.UserId == review.UserId).FirstOrDefaultAsync();
                    var movieObj = await _database.Movies.Where(m => m.MovieId == review.MovieId).FirstOrDefaultAsync();
                    review.User = userObj == null ? throw new NullReferenceException() : userObj;
                    review.Movie = movieObj == null ? throw new NullReferenceException() : movieObj;
                    ReviewsList.Add(_mapperService.ReviewMapperDtoService(review));
                }
            }
            else if (userEmail == null && movieId != null || movieId > 0)
            {
                var movie = await _database.Movies.FindAsync(movieId);
                if (movie == null) throw new NullReferenceException();
                Reviews = await GetReviewsOfMovie(movie.MovieId);
                foreach (var review in Reviews)
                {
                    var userObj = await _database.Users.FindAsync(review.UserId);
                    review.User = userObj != null ? userObj : throw new NullReferenceException();
                    review.Movie = movie;
                    ReviewsList.Add(_mapperService.ReviewMapperDtoService(review));
                }
            }
            else if (userEmail != null && movieId == null || movieId == 0)
            {                
                var user = await _database.Users.Where(u => string.Equals(u.EmailAddress, userEmail)).FirstOrDefaultAsync();                
                if (user == null) throw new NullReferenceException();
                Reviews = await GetReviewsOfUser(user.UserId);
                foreach (var review in Reviews)
                {
                    var movieObj = await _database.Movies.FindAsync(review.MovieId);
                    review.User = user;
                    review.Movie = movieObj != null ? movieObj : throw new NullReferenceException();
                    ReviewsList.Add(_mapperService.ReviewMapperDtoService(review));
                }
            }
            else if (userEmail != null && movieId != null || movieId > 0)
            {
                var user = await _database.Users.Where(u => string.Equals(u.EmailAddress, userEmail)).FirstOrDefaultAsync();
                var movie = await _database.Movies.FindAsync(movieId);
                if(user == null || movie == null) throw new NullReferenceException();
                var result = await GetYourRiviewOfMovie(user.UserId, movie.MovieId);
                if (result != null && result.Count > 0)
                {
                    Reviews = result.ToList();
                    foreach (var review in Reviews)
                    {
                        review.Movie = movie;
                        review.User = user;
                        ReviewsList.Add(_mapperService.ReviewMapperDtoService(review));
                    }
                }
            }
            return ReviewsList;
        }

        public async Task PostNewReview(string userEmail, int movieId, string? descriptionVote, float vote, string? when)
        {
            var user = await _database.Users.Where(u => string.Equals(u.EmailAddress, userEmail)).FirstOrDefaultAsync();
            var movie = await _database.Movies.FindAsync(movieId);
            if (movie == null || user == null) throw new NullReferenceException();
            if (movie.MovieYearProduction > Convert.ToDateTime(when).Year) throw new Exception();
            var movieReviewed = await GetReviewsOfMovie(movieId);
            if (movieReviewed != null && movieReviewed.Count > 0)
            {
                movieReviewed.FirstOrDefault().UserId = user.UserId;
                movieReviewed.FirstOrDefault().MovieId = movie.MovieId;
                movieReviewed.FirstOrDefault().Movie = movie;
                movieReviewed.FirstOrDefault().User = user;
                movieReviewed.FirstOrDefault().Vote = vote;
                movieReviewed.FirstOrDefault().DescriptionVote = descriptionVote;
                movieReviewed.FirstOrDefault().DateTimeVote = string.IsNullOrEmpty(when) ? DateTime.Now : Convert.ToDateTime(when);
                _database.Reviews.Update(movieReviewed.FirstOrDefault());
                await _database.SaveChangesAsync();
                user.ReviewsList.Add(movieReviewed.FirstOrDefault());
                user.MoviesList.Add(movie);
            }
            else
            {
                Review newReview = new Review
                {
                    UserId = user.UserId,
                    MovieId = movie.MovieId,
                    Movie = movie,
                    User = user,
                    Vote = vote,
                    DescriptionVote = descriptionVote,
                    DateTimeVote = string.IsNullOrEmpty(when) ? DateTime.Now : Convert.ToDateTime(when)
                };
                await _database.Reviews.AddAsync(newReview);
                await _database.SaveChangesAsync();
                user.ReviewsList.Add(newReview);
                user.MoviesList.Add(movie);
            }
            _database.Users.Update(user);
            _database.Movies.Update(movie);
            await _database.SaveChangesAsync();
        }

        #endregion

        #region PRIVATE SERVICES

        private async Task<List<Review>> GetReviews()
        {
            return await _database.Reviews.ToListAsync();
        }

        private async Task<List<Review>> GetReviewsOfUser(int userId)
        {
            return await _database.Reviews.Where(r => r.UserId == userId).ToListAsync();
        }

        private async Task<List<Review>> GetReviewsOfMovie(int movieId)
        {
            return await _database.Reviews.Where(r => r.MovieId == movieId).ToListAsync();
        }

        private async Task<List<Review>?> GetYourRiviewOfMovie(int userId, int movieId)
        {
            var moviesReviewedByUser = await _database.Reviews.Where(r => r.MovieId == movieId && r.UserId == userId).ToListAsync();
            return moviesReviewedByUser;
        }

        #endregion
    }
}
