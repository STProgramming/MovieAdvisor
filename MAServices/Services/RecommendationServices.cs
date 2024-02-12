using MAAI.Interfaces;
using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAModels.Models;
using MAServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Trainers;

namespace MAAI.ScriptAI
{
    public class RecommendationServices : IRecommendationServices
    {
        private readonly ApplicationDbContext _context;

        private readonly IUserServices _userServices;

        public RecommendationServices(ApplicationDbContext context, IUserServices userServices)
        {
            _context = context;
            _userServices = userServices;
        }

        public async Task<List<MovieResultRecommendation>> MoviesSuggestedByUser(string userEmail)
        {
            User user = await _userServices.GetUserFromEmail(userEmail);
            if (user == null) throw new NullReferenceException();

            //E' importante che le reviews siano di tutti gli utenti perchè deve imparare da tutte le casistiche
            //In seguito chiederemo per un utente specifico

            List<Review> reviews = await _context.Reviews.ToListAsync();
            List<ModelInput> modelTrain = new List<ModelInput>();
            MLContext mlContext = new MLContext();
            var result = new List<MovieResultRecommendation>();
            List<ModelOutput> movieSuggesteds = new List<ModelOutput>();

            //Caricamento dei film non visti dall'utente

            List<Movie> movieNotYetSeen = await _context.Movies.Where(m => !m.UsersList.Contains(user)).ToListAsync();
            short yearOfUser = Convert.ToInt16(DateTime.Now.Year - user.BirthDate.Year);
            foreach (Movie movie in movieNotYetSeen)
            {
                //Verranno escusi i film che siano per un pubblico adulto nel caso in cui l'utente non abbia la maggiore età

                if (movie.IsForAdult == true && yearOfUser < 18)
                {
                    movieNotYetSeen.Remove(movie);
                }
            }

            //possiamo suggerire solo se l'utente ha già fatto delle review altrimenti possiamo consigliare altro TODO...

            if (reviews != null && reviews.Count > 0)
            {
                foreach (var review in reviews)
                {
                    ModelInput train = new ModelInput
                    {
                        UserId = user.UserId,
                        MovieId = review.MovieId,
                        Label = review.Vote
                    };
                    modelTrain.Add(train);
                }

                IDataView data = mlContext.Data.LoadFromEnumerable(modelTrain);

                var dataProcessingPipeline =
                   mlContext
                       .Transforms
                       .Conversion
                       .MapValueToKey(outputColumnName: "UserIdEncoded",
                                       inputColumnName: nameof(ModelInput.UserId))
                   .Append(mlContext
                           .Transforms
                           .Conversion
                           .MapValueToKey(outputColumnName: "MovieIdEncoded",
                                           inputColumnName: nameof(ModelInput.MovieId)));

                var finalOptions = new MatrixFactorizationTrainer.Options
                {
                    MatrixColumnIndexColumnName = "UserIdEncoded",
                    MatrixRowIndexColumnName = "MovieIdEncoded",
                    LabelColumnName = "Label",
                    NumberOfIterations = 10,
                    ApproximationRank = 200,
                    Quiet = true
                };

                var trainer = mlContext.Recommendation().Trainers.MatrixFactorization(finalOptions);

                var trainingPipeLine = dataProcessingPipeline.Append(trainer);

                var model = trainingPipeLine.Fit(data);

                var predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model);

                foreach (var movie in movieNotYetSeen)
                {
                    var inputCase = new ModelInput { UserId = user.UserId, MovieId = movie.MovieId, };
                    var movieRatingPrediction = predictionEngine.Predict(inputCase);
                    var userDTO = new UserDTO();
                    var movieDTO = new MovieDTO();
                    result.Add(new MovieResultRecommendation
                    {
                        MovieId = movie.MovieId,
                        UserId = user.UserId,
                        UserObj = userDTO.ConvertToUserDTO(user),
                        MovieObj = movieDTO.ConvertToMovieDTO(movie),
                        Score = movieRatingPrediction.Score
                    });
                }
            }
            return result.OrderBy(r => r.Score).ToList();
        }        
    }
}
