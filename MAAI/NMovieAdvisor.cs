using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using Tensorflow.Contexts;

namespace MAAI
{
    public class NMovieAdvisor
    {
        private readonly ApplicationDbContext _context;

        private readonly MLContext _mlContext;

        private readonly IReviewServices _reviewServices;

        public NMovieAdvisor(ApplicationDbContext context,
            MLContext mlContext,
            IReviewServices reviewServices)
        {
            _context = context;
            _mlContext = mlContext;
            _reviewServices = reviewServices;
        }

        public async List<MovieDTO> NMoviesSuggested(User user)
        {
            var userReviews = _mlContext.Data.LoadFromEnumerable<ReviewDTO>(await _reviewServices.SearchEngineReviews(user, null));

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

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<ReviewDTO, MovieRatingPrediction>(model);
            var userId = 1; // ID dell'utente di interesse
            var moviesToRecommend = yourMoviesData
                .Where(movie => !yourUserReviewedMovies.Contains(movie.MovieId)) // Escludi film già recensiti
                .Select(movie => new Review { UserId = userId, MovieId = movie.MovieId });

            var recommendations = moviesToRecommend.Select(movie => predictionEngine.Predict(movie));

            foreach (var recommendation in recommendations)
            {
                Console.WriteLine($"MovieId: {recommendation.MovieId}, Predicted Rating: {recommendation.Score}");
            }
            Ricorda di adattare questi esempi alle tue esigenze specifiche, in base alla struttura dei tuoi dati.Questo è solo un punto di partenza e potresti dover ottimizzare e personalizzare ulteriormente il modello in base alle caratteristiche del tuo dataset e alle esigenze dell'applicazione.




Is this conversation helpful so far?





        }
    }
}
