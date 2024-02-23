﻿using MAContracts.Contracts.Services;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;
using MAModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Trainers;

namespace MAAI.ScriptAI
{
    public class RecommendationServices : IRecommendationServices
    {
        private readonly ApplicationDbContext _context;

        public RecommendationServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovieResultRecommendation>> MoviesSuggestedByUser(string userEmail)
        {
            var user = await _context.Users.Where(u => string.Equals(u.EmailAddress, userEmail)).FirstOrDefaultAsync();
            if (user == null) throw new NullReferenceException();

            //E' importante che le reviews siano di tutti gli utenti perchè deve imparare da tutte le casistiche
            //In seguito chiederemo per un utente specifico

            List<Review> reviews = await _context.Reviews.ToListAsync();
            List<ModelInput> modelTrain = new List<ModelInput>();
            MLContext mlContext = new MLContext();
            var result = new List<MovieResultRecommendation>();
            List<ModelOutput> movieSuggesteds = new List<ModelOutput>();

            //Caricamento dei film non visti dall'utente
            List<Movie> movieNotYetSeen = await _context.Movies.Where(m => m.UsersList.Count == 0 || !m.UsersList.Any(u => u.UserId == user.UserId)).ToListAsync();
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
                    ModelInput train = new ModelInput();
                    Movie movie = new Movie();
                    if (review.Movie == null) movie = await _context.Movies.Where(m => m.MovieId == review.MovieId).FirstOrDefaultAsync();
                    else movie = review.Movie;
                    List<Tag> tags = _context.Tags.Where(t => t.MoviesList.Any(m => m.MovieId == movie.MovieId)).ToList();
                    tags.ForEach(tag =>
                    {
                        train.MovieGenres += string.Join(", ", tag.TagName);
                    });

                    train.UserId = user.UserId;
                    train.MovieId = review.MovieId;
                    train.MovieTitle = movie.MovieTitle;
                    train.MovieDescription = movie.MovieDescription;
                    train.MovieMaker = movie.MovieMaker;
                    train.UserName = user.UserName;
                    train.Label = review.Vote;
                    train.ReviewDate = review.DateTimeVote.ToString();
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
                                           inputColumnName: nameof(ModelInput.MovieId)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "MovieTitleFeaturized",
                                            inputColumnName: nameof(ModelInput.MovieTitle)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "MovieDescriptionFeaturized",
                                            inputColumnName: nameof(ModelInput.MovieDescription)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "MovieMakerFeaturized",
                                            inputColumnName: nameof(ModelInput.MovieMaker)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "UserNameFeaturized",
                                            inputColumnName: nameof(ModelInput.UserName)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "MovieGenresFeaturized",
                                            inputColumnName: nameof(ModelInput.MovieGenres)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "ReviewDateFeaturized",
                                            inputColumnName: nameof(ModelInput.ReviewDate)))
                   .Append(mlContext
                           .Transforms
                           .Concatenate("Features", "MovieTitle", "MovieDescription", "MovieMaker", "UserName", "MovieGenres", "ReviewDate"));                   

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
                        Name = user.Name,
                        LastName = user.LastName,
                        Score = Double.IsNaN(movieRatingPrediction.Score) ? 0 : movieRatingPrediction.Score 
                    });
                }
            }
            return result.OrderByDescending(r => r.Score).ToList();
        }        
    }
}
