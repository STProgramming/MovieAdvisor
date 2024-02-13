using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MAServices.Services
{
    public class ReviewServices : IReviewServices
    {
        private readonly ApplicationDbContext _database;

        private readonly IUserServices _userServices;

        private readonly IMovieServices _movieServices;

        public ReviewServices(ApplicationDbContext database,
            IUserServices userServices,
            IMovieServices movieServices)
        {
            _database = database;
            _userServices = userServices;
            _movieServices = movieServices;
        }

        public async Task<List<ReviewDTO>?> SearchEngineReviews(string? userEmail, int? movieId)
        {
            List<ReviewDTO> ReviewsList = new List<ReviewDTO>();
            List<Review> Reviews = new List<Review>();
            if (userEmail == null && movieId == null || movieId == 0)
            {
                Reviews = await GetReviews();
                foreach (var review in Reviews)
                {
                    var userObj = await _userServices.GetUserFromId(review.UserId);
                    var movieObj = await _movieServices.GetMovieDataById(review.MovieId);
                    ReviewDTO reviewDto = new ReviewDTO();
                    review.User = userObj != null ? userObj : new User();
                    review.Movie = movieObj != null ? movieObj : new Movie();
                    ReviewsList.Add(reviewDto.ConvertToReviewDTO(review));
                }
            }
            else if (userEmail == null && movieId != null || movieId > 0)
            {
                var movie = await _movieServices.GetMovieDataById(movieId.Value);
                if (movie == null) throw new NullReferenceException();
                Reviews = await GetReviewsOfMovie(movie.MovieId);
                foreach (var review in Reviews)
                {
                    var userObj = await _userServices.GetUserFromId(review.UserId);
                    ReviewDTO reviewDto = new ReviewDTO();
                    review.User = userObj != null ? userObj : new User();
                    review.Movie = movie;
                    ReviewsList.Add(reviewDto.ConvertToReviewDTO(review));
                }
            }
            else if (userEmail != null && movieId == null || movieId == 0)
            {
                var user = await _userServices.GetUserFromEmail(userEmail);
                if (user == null) throw new NullReferenceException();
                Reviews = await GetReviewsOfUser(user.UserId);
                foreach (var review in Reviews)
                {
                    var movieObj = await _movieServices.GetMovieDataById(review.MovieId);
                    ReviewDTO reviewDto = new ReviewDTO();
                    review.User = user;
                    review.Movie = movieObj != null ? movieObj : new Movie();
                    ReviewsList.Add(reviewDto.ConvertToReviewDTO(review));
                }
            }
            else if (userEmail != null && movieId != null || movieId > 0)
            {
                var user = await _userServices.GetUserFromEmail(userEmail);
                var movie = await _movieServices.GetMovieDataById(movieId.Value);
                if(user == null || movie == null) throw new NullReferenceException();
                var result = await GetYourRiviewOfMovie(user.UserId, movie.MovieId);
                if (result != null && result.Count > 0)
                {
                    Reviews = result.ToList();
                    foreach (var review in Reviews)
                    {
                        ReviewDTO reviewDto = new ReviewDTO();
                        review.Movie = movie;
                        review.User = user;
                        ReviewsList.Add(reviewDto.ConvertToReviewDTO(review));
                    }
                }
            }
            return ReviewsList;
        }

        public async Task PostNewReview(string userEmail, int movieId, string? descriptionVote, float vote, string? when)
        {
            var user = await _userServices.GetUserFromEmail(userEmail);
            var movie = await _movieServices.GetMovieDataById(movieId);
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
    }
}
