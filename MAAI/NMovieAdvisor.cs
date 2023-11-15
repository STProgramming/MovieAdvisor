using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace MAAI
{
    public class NMovieAdvisor : INMovieAdvisor
    {
        private readonly ApplicationDbContext _context;

        public NMovieAdvisor(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovieDTO>> NMoviesSuggestedByUser(User user)
        {
            MLContext _mlContext = new MLContext();
            var allReviewsFromUser = await _context.Reviews.Where(r => r.UserId == user.UserId).ToListAsync();
            List<ReviewDTO> reviewsDto = new List<ReviewDTO>();
            foreach(var rev in allReviewsFromUser)
            {
                rev.Movie = await _context.Movies.FindAsync(rev.MovieId);
                rev.User = await _context.Users.FindAsync(rev.UserId);
                ReviewDTO review = new ReviewDTO();
                reviewsDto.Add(review.ConvertToReviewDTO(rev));
            }
            var userReviews = _mlContext.Data.LoadFromEnumerable<ReviewDTO>(reviewsDto);

            var recommenderOptions = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "UserId",
                MatrixRowIndexColumnName = "MovieId",
                LabelColumnName = "Vote",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("UserId", "UserId")
                .Append(_mlContext.Transforms.Conversion.MapValueToKey("MovieId", "MovieId"))
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("Label", "Rating"))
                .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(recommenderOptions));

            var model = pipeline.Fit(userReviews);

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<ReviewDTO, MovieDTO>(model);
            var userId = user.UserId; // ID dell'utente di interesse
            List<MovieDTO> allMoviesDto = new List<MovieDTO>();
            var allMovies = await _context.Movies.ToListAsync();
            for(int i = 0; i < allMovies.Count; i++)
            {
                if (!allMovies[i].UsersList.Contains(user))
                {
                    MovieDTO movieDTO = new MovieDTO();
                    allMoviesDto.Add(movieDTO.ConvertToMovieDTO(allMovies[i]));
                }
            }            

            var recommendations = allMoviesDto.Select(movie => predictionEngine.Predict(reviewsDto.Where(r => r.Movie.MovieId == movie.MovieId).FirstOrDefault()));
            return recommendations.ToList();
        }
    }
}
