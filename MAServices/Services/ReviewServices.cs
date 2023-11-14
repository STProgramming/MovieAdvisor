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

        public async Task PostNewReview(User user, Movie movie, string? descriptionVote, short vote)
        {
            Review newReview = new Review 
            { 
                UserId = user.UserId, 
                MovieId = movie.MovieId, 
                Movie = movie,
                User = user, 
                Vote = vote, 
                DescriptionVote = descriptionVote,
                DateTimeVote = DateTime.Now
            };
            await _database.Reviews.AddAsync(newReview);
            await _database.SaveChangesAsync();
            MovieUser userSeenMovie = new MovieUser
            {
                User = user,
                UserId = user.UserId,
                MovieId = movie.MovieId,
                Movie = movie,
            };
            user.ReviewsList.Add(newReview);
            user.MoviesList.Add(movie);
            movie.UsersList.Add(user);            
            _database.Users.Update(user);
            _database.Movies.Update(movie);
            await _database.MoviesUsers.AddAsync(userSeenMovie);
            await _database.SaveChangesAsync();
        }

        public async Task<List<ReviewDTO>?> SearchEngineReviews(User? user, Movie? movie)
        {
            List<ReviewDTO> ReviewsList = new List<ReviewDTO>();
            List<Review> Reviews = new List<Review>();
            if(user == null && movie == null)
            {
                Reviews = await GetReviews();
                foreach (var review in Reviews)
                {
                    var userObj = await _userServices.GetUserFromId(review.UserId);
                    var movieObj = await _movieServices.GetMovieData(review.MovieId);
                    ReviewDTO reviewDto = new ReviewDTO();
                    review.User = userObj != null ? userObj : new User();
                    review.Movie = movieObj != null ? movieObj : new Movie();
                    ReviewsList.Add(reviewDto.ConvertToReviewDTO(review));
                }
            }
            else if (user == null && movie != null)
            {
                Reviews = await GetReviewsOfMovie(movie.MovieId);
                foreach(var review in Reviews)
                {
                    var userObj = await _userServices.GetUserFromId(review.UserId);
                    ReviewDTO reviewDto = new ReviewDTO();
                    review.User = userObj != null ? userObj : new User();
                    review.Movie = movie;
                    ReviewsList.Add(reviewDto.ConvertToReviewDTO(review));
                }
            }
            else if (user != null && movie == null)
            {
                Reviews = await GetReviewsOfUser(user.UserId);
                foreach (var review in Reviews)
                {
                    var movieObj = await _movieServices.GetMovieData(review.MovieId);
                    ReviewDTO reviewDto = new ReviewDTO();
                    review.User = user;
                    review.Movie = movieObj != null ? movieObj : new Movie();
                    ReviewsList.Add(reviewDto.ConvertToReviewDTO(review));
                }
            }
            else if(user != null && movie != null)
            {
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
