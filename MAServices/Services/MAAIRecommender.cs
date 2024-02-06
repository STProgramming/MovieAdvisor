using MAAI.Interfaces;
using MAModels.EntityFrameworkModels;
using MAModels.Models;
using MAServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Trainers;

namespace MAAI.ScriptAI
{
    public class MAAIRecommender : IMAAIRecommender
    {
        private readonly ApplicationDbContext _context;

        private readonly IFileServices _fileServices;

        public MAAIRecommender(ApplicationDbContext context,
            IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<List<MovieSuggested>> NMoviesSuggestedByUser(User user)
        {
            List<Review> reviewsByUser = await _context.Reviews.Where(r => r.UserId == user.UserId).ToListAsync();
            List<Preference>? preferencies = new List<Preference>();
            List<PreferenceModelTrain> modelTrain = new List<PreferenceModelTrain>();
            MLContext mlContext = new MLContext();
            preferencies = await _context.ModelsTrain.Where(u => u.UserId == user.UserId).ToListAsync();
            List<MovieSuggested> movieSuggesteds = new List<MovieSuggested>();
            List<Movie> movieNotYetSeen = await _context.Movies.Where(m => !m.UsersList.Contains(user)).ToListAsync();
            short yearOfUser = Convert.ToInt16(DateTime.Now.Year - user.BirthDate.Year);
            foreach (Movie movie in movieNotYetSeen)
            {
                if (movie.IsForAdult == true && yearOfUser < 18)
                {
                    movieNotYetSeen.Remove(movie);
                }
            }
            if (preferencies != null && preferencies.Count > 0)
            {
                foreach (var preference in preferencies)
                {
                    PreferenceModelTrain train = new PreferenceModelTrain
                    {
                        UserId = user.UserId,
                        MovieId = preference.MovieId,
                        Label = preference.Vote
                    };
                    modelTrain.Add(train);
                }

                var data = mlContext.Data.LoadFromEnumerable(modelTrain);

                var dataProcessingPipeline = mlContext
                    .Transforms
                    .Conversion
                    .MapValueToKey(outputColumnName: "UserIdEncoded",
                                    inputColumnName: nameof(PreferenceModelTrain.UserId))
                    .Append(mlContext
                    .Transforms
                    .Conversion
                    .MapValueToKey(outputColumnName: "MovieIdEncoded",
                                    inputColumnName: nameof(PreferenceModelTrain.MovieId)));

                var options = new MatrixFactorizationTrainer.Options
                {
                    MatrixColumnIndexColumnName = "UserIdEncoded",
                    MatrixRowIndexColumnName = "MovieIdEncoded",
                    LabelColumnName = "Label",
                    NumberOfIterations = modelTrain.Count,
                    ApproximationRank = 100,
                    Quiet = true,
                };

                var trainer = mlContext.Recommendation().Trainers.MatrixFactorization(options);

                var trainingPipeLine = dataProcessingPipeline.Append(trainer);

                var model = trainingPipeLine.Fit(data);

                var predictionEngine = mlContext.Model.CreatePredictionEngine<PreferenceModelTrain, MovieSuggested>(model);

                var crossValMetrics = mlContext.Recommendation()
                                        .CrossValidate(data: data,
                                        estimator: trainingPipeLine,
                                        labelColumnName: "Label");

                try
                {
                    var prediction = mlContext.Model.CreatePredictionEngine<PreferenceModelTrain, MovieSuggested>(model);
                    foreach (var movie in movieNotYetSeen)
                    {
                        var pred = prediction.Predict(new PreferenceModelTrain
                        {
                            UserId = user.UserId,
                            MovieId = movie.MovieId,
                        });
                        if (pred.Score > 3.5)
                        {
                            movieSuggesteds.Add(pred);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return movieSuggesteds;
        }        
    }
}
