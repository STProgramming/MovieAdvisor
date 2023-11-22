using MAAI.Interfaces;
using MAModels.EntityFrameworkModels;
using MAModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Trainers;
using Tensorflow.Contexts;

namespace MAAI.ScriptAI
{
    public class NMovieAdvisor : INMovieAdvisor
    {
        private readonly ApplicationDbContext _context;

        public NMovieAdvisor(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovieSuggested>> NMoviesSuggestedByUser(User user)
        {
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

                IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "UserIdEncoded", inputColumnName: "UserId")
                    .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "MovieIdEncoded", inputColumnName: "MovieId"));

                var options = new MatrixFactorizationTrainer.Options
                {
                    MatrixColumnIndexColumnName = "UserIdEncoded",
                    MatrixRowIndexColumnName = "MovieIdEncoded",
                    LabelColumnName = "Label",
                    NumberOfIterations = 20,
                    ApproximationRank = 100
                };

                var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));

                ITransformer model = trainerEstimator.Fit(data);

                var prediction = model.Transform(data);

                var metrics = mlContext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");

                var predictionEngine = mlContext.Model.CreatePredictionEngine<PreferenceModelTrain, MovieSuggested>(model);

                foreach (var movie in movieNotYetSeen)
                {
                    var pred = predictionEngine.Predict(new PreferenceModelTrain
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
            return movieSuggesteds;
        }
    }
}
