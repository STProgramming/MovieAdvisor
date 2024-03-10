using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services;
using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.ModelsDTOs;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MAServices.Services
{
    public class ReviewServices : IReviewServices
    {
        private readonly ApplicationDbContext _database;

        private readonly IObjectsMapperDtoServices _mapperService;

        private readonly UserManager<Users> _userManager;

        public ReviewServices(ApplicationDbContext database,
            IObjectsMapperDtoServices mapperService,
            UserManager<Users> userManager)
        {
            _database = database;
            _mapperService = mapperService;
            _userManager = userManager;
        }

        #region PUBLIC SERVICES

        public async Task<List<ReviewsDTO>?> SearchEngineReviews(string? userId, int? movieId)
        {
            List<ReviewsDTO> ReviewsList = new List<ReviewsDTO>();
            List<Reviews> Reviews = new List<Reviews>();
            if (userId == null && movieId == null || movieId == 0)
            {
                Reviews = await GetReviews();
                foreach (var review in Reviews)
                {
                    var userObj = await _userManager.FindByIdAsync(review.UserId);
                    var movieObj = await _database.Movies.Where(m => m.MovieId == review.MovieId).FirstOrDefaultAsync();
                    review.User = userObj == null ? throw new NullReferenceException() : userObj;
                    review.Movie = movieObj == null ? throw new NullReferenceException() : movieObj;
                    ReviewsList.Add(_mapperService.ReviewMapperDtoService(review));
                }
            }
            else if (userId == null && movieId != null || movieId > 0)
            {
                var movie = await _database.Movies.FindAsync(movieId);
                if (movie == null) throw new NullReferenceException();
                Reviews = await GetReviewsOfMovie(movie.MovieId);
                foreach (var review in Reviews)
                {
                    var userObj = await _userManager.FindByIdAsync(review.UserId);
                    review.User = userObj != null ? userObj : throw new NullReferenceException();
                    review.Movie = movie;
                    ReviewsList.Add(_mapperService.ReviewMapperDtoService(review));
                }
            }
            else if (userId != null && movieId == null || movieId == 0)
            {                
                var user = await _userManager.FindByIdAsync(userId);                
                if (user == null) throw new NullReferenceException();
                Reviews = await GetReviewsOfUser(user.Id);
                foreach (var review in Reviews)
                {
                    var movieObj = await _database.Movies.FindAsync(review.MovieId);
                    review.User = user;
                    review.Movie = movieObj != null ? movieObj : throw new NullReferenceException();
                    ReviewsList.Add(_mapperService.ReviewMapperDtoService(review));
                }
            }
            else if (userId != null && movieId != null || movieId > 0)
            {
                var user = await _userManager.FindByIdAsync(userId);
                var movie = await _database.Movies.FindAsync(movieId);
                if(user == null || movie == null) throw new NullReferenceException();
                var result = await GetYourRiviewOfMovie(user.Id, movie.MovieId);
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

        public async Task PostNewReview(string userId, NewReviewDTO newReviewDto)
        {
            var user = await _database.Users.Where(u => string.Equals(u.Id, userId)).FirstOrDefaultAsync();
            var movie = await _database.Movies.FindAsync(newReviewDto.MovieId);
            if (movie == null || user == null) throw new NullReferenceException();
            if (movie.MovieYearProduction > Convert.ToDateTime(newReviewDto.When).Year) throw new Exception();
            var movieReviewed = await GetReviewsOfMovie(newReviewDto.MovieId);
            if (movieReviewed != null && movieReviewed.Count > 0)
            {
                movieReviewed.FirstOrDefault().UserId = user.Id;
                movieReviewed.FirstOrDefault().MovieId = movie.MovieId;
                movieReviewed.FirstOrDefault().Movie = movie;
                movieReviewed.FirstOrDefault().User = user;
                movieReviewed.FirstOrDefault().Vote = newReviewDto.Vote;
                movieReviewed.FirstOrDefault().DescriptionVote = newReviewDto.DescriptionVote;
                movieReviewed.FirstOrDefault().DateTimeVote = newReviewDto.When == null ? DateTime.Now : Convert.ToDateTime(newReviewDto.When);
                _database.Reviews.Update(movieReviewed.FirstOrDefault());
                await _database.SaveChangesAsync();
                user.ReviewsList.Add(movieReviewed.FirstOrDefault());
                user.MoviesList.Add(movie);
            }
            else
            {
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

        #region PRIVATE METHODS

        private async Task<List<Reviews>> GetReviews()
        {
            return await _database.Reviews.ToListAsync();
        }

        private async Task<List<Reviews>> GetReviewsOfUser(string userId)
        {
            return await _database.Reviews.Where(r => string.Equals(r.UserId, userId)).ToListAsync();
        }

        private async Task<List<Reviews>> GetReviewsOfMovie(int movieId)
        {
            return await _database.Reviews.Where(r => r.MovieId == movieId).ToListAsync();
        }

        private async Task<List<Reviews>?> GetYourRiviewOfMovie(string userId, int movieId)
        {
            var moviesReviewedByUser = await _database.Reviews.Where(r => r.MovieId == movieId && string.Equals(r.UserId, userId)).ToListAsync();
            return moviesReviewedByUser;
        }

        #endregion
    }
}
