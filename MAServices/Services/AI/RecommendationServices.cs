using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services.AI;
using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.EntityFrameworkDTOs.AI;
using MADTOs.DTOs.EntityFrameworkDTOs.Identity;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.AI;
using MAModels.EntityFrameworkModels.Identity;
using MAModels.EntityFrameworkModels.Movie;
using MAModels.Exceptions.AI;
using MAModels.Models.RecommendationServices.RecommendationBasedOnRequest;
using MAModels.Models.RecommendationServices.RecommendationBasedOnReviews;
using MAModels.Models.RecommendationServices.RecommendationBasedOnSentiments;
using MAModels.Models.RecommendationServices.SentimentPrediction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Trainers;
using static Microsoft.ML.DataOperationsCatalog;

namespace MAServices.Services.AI
{
    public class RecommendationServices : IRecommendationServices
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<Users> _userManager;

        private readonly IConfiguration _config;

        private readonly IObjectsMapperDtoServices _mapper;

        public RecommendationServices(
            ApplicationDbContext context,
            UserManager<Users> userManager,
            IConfiguration config,
            IObjectsMapperDtoServices objectsMapperDtoServices)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _mapper = objectsMapperDtoServices;
        }

        #region PUBLIC SERVICES

        public async Task<List<RecommendationsDTO>> RecommendationsBasedOnReviews(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null) throw new NullReferenceException();

            await AISmartAvailability();

            var request = await RequestsManager(user, null, null, null, null);

            var sessionInfo = await SessionsManager(user, request);

            List<Recommendations> result = await BasedOnReviews(user, request);

            await RequestsManager(user, null, result, sessionInfo, null);

            return _mapper.RecommendationMapperDtoListService(result);
        }

        public async Task<List<RecommendationsDTO>> RecommendationsBasedOnRequest(string userEmail, RequestsDTO requestUser)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null) throw new NullReferenceException();

            await AISmartAvailability();

            var request = await RequestsManager(user, requestUser, null, null, null);

            var sessionInfo = await SessionsManager(user, request);

            List<Recommendations> result = await BasedOnReviews(user, request);

            SentimentPredict sentimentUser = await PredictSentimentUser(user, request);

            List<Recommendations> defResult = await BasedOnRequest(user, sentimentUser, result, request);

            await RequestsManager(user, null, result, sessionInfo, sentimentUser.Prediction);

            return _mapper.RecommendationMapperDtoListService(defResult);
        }

        #endregion

        #region PRIVATE METHODS

        private async Task AISmartAvailability()
        {
            var reviews = await _context.Reviews.ToListAsync();

            if (reviews.Count < Int32.Parse(_config["Ai:Recommendation:MinimumLength"])) throw new InsufficientReviewsException();
        }

        private async Task<List<Recommendations>> BasedOnReviews(Users user, Requests request)
        {
            //MODEL TRAIN CONSTRUCTION
            List<Reviews> reviews = await _context.Reviews.ToListAsync();
            List<BasedOnReviewsInput> modelTrain = new List<BasedOnReviewsInput>();
            MLContext mlContext = new MLContext();
            var result = new List<Recommendations>();
            List<BasedOnReviewsOutput> movieSuggesteds = new List<BasedOnReviewsOutput>();

            //Caricamento dei film non visti dagl' utenti
            List<Movies> movieNotYetSeen = await _context.Movies.Where(m => m.UsersList.Count == 0 || !m.UsersList.Any(u => u.Id == user.Id)).ToListAsync();
            short yearOfUser = Convert.ToInt16(DateTime.Now.Year - user.BirthDate.Year);
            foreach (Movies movie in movieNotYetSeen)
            {
                //Verranno escusi i film che siano per un pubblico adulto nel caso in cui l'utente non abbia la maggiore età

                if (movie.IsForAdult == true && yearOfUser < 18)
                {
                    movieNotYetSeen.Remove(movie);
                }
            }

            if (reviews != null && reviews.Count > 0)
            {
                foreach (var review in reviews)
                {
                    BasedOnReviewsInput train = new BasedOnReviewsInput();
                    Movies movie = new Movies();
                    if (review.Movie == null) movie = await _context.Movies.Where(m => m.MovieId == review.MovieId).FirstOrDefaultAsync();
                    else movie = review.Movie;
                    List<Tags> tags = _context.Tags.Where(t => t.MoviesList.Any(m => m.MovieId == movie.MovieId)).ToList();
                    tags.ForEach(tag =>
                    {
                        train.MovieGenres += string.Join(", ", tag.TagName);
                    });
                    train.UserId = user.Id;
                    train.MovieId = review.MovieId;
                    train.MovieTitle = movie.MovieTitle;
                    train.MovieDescription = movie.MovieDescription;
                    train.MovieMaker = movie.MovieMaker;
                    train.UserName = user.UserName;
                    train.Label = review.Vote;
                    train.ReviewDate = review.DateTimeVote.ToString();
                    train.Gender = user.Gender;
                    train.Nationality = user.Nationality;
                    train.MovieLifeSpan = movie.MovieLifeSpan;
                    train.DescriptionVote = string.IsNullOrEmpty(review.DescriptionVote) ? "" : review.DescriptionVote;
                    modelTrain.Add(train);
                }

                //SET MODEL INPUT DATA

                IDataView data = mlContext.Data.LoadFromEnumerable(modelTrain);

                var dataProcessingPipeline =
                   mlContext
                       .Transforms
                       .Conversion
                       .MapValueToKey(outputColumnName: "UserIdEncoded",
                                       inputColumnName: nameof(BasedOnReviewsInput.UserId))
                   .Append(mlContext
                           .Transforms
                           .Conversion
                           .MapValueToKey(outputColumnName: "MovieIdEncoded",
                                           inputColumnName: nameof(BasedOnReviewsInput.MovieId)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "MovieTitleFeaturized",
                                            inputColumnName: nameof(BasedOnReviewsInput.MovieTitle)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "MovieDescriptionFeaturized",
                                            inputColumnName: nameof(BasedOnReviewsInput.MovieDescription)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "MovieMakerFeaturized",
                                            inputColumnName: nameof(BasedOnReviewsInput.MovieMaker)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "UserNameFeaturized",
                                            inputColumnName: nameof(BasedOnReviewsInput.UserName)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "MovieGenresFeaturized",
                                            inputColumnName: nameof(BasedOnReviewsInput.MovieGenres)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "ReviewDateFeaturized",
                                            inputColumnName: nameof(BasedOnReviewsInput.ReviewDate)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "GenderFeaturized",
                                            inputColumnName: nameof(BasedOnReviewsInput.Gender)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "NationalityFeaturized",
                                            inputColumnName: nameof(BasedOnReviewsInput.Nationality)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "MovieLifeSpanFeaturized",
                                            inputColumnName: nameof(BasedOnReviewsInput.MovieLifeSpan)))
                   .Append(mlContext
                           .Transforms
                           .Text
                           .FeaturizeText(outputColumnName: "DescriptionVoteFeaturized",
                                            inputColumnName: nameof(BasedOnReviewsInput.DescriptionVote)))

                   .Append(mlContext
                           .Transforms
                           .Concatenate("Features", "MovieTitle", "MovieDescription", "MovieMaker", "UserName", "MovieGenres", "ReviewDate", "Gender", "Nationality", "MovieLifeSpan", "DescriptionVote"));

                var finalOptions = new MatrixFactorizationTrainer.Options
                {
                    MatrixColumnIndexColumnName = "UserIdEncoded",
                    MatrixRowIndexColumnName = "MovieIdEncoded",
                    LabelColumnName = "Label",
                    NumberOfIterations = 10,
                    ApproximationRank = 200,
                    Quiet = true,
                };

                //AI SETUP

                var trainer = mlContext.Recommendation().Trainers.MatrixFactorization(finalOptions);

                var trainingPipeLine = dataProcessingPipeline.Append(trainer);

                var model = trainingPipeLine.Fit(data);

                var predictionEngine = mlContext.Model.CreatePredictionEngine<BasedOnReviewsInput, BasedOnReviewsOutput>(model);

                //RESULTS CONTRUCTION

                foreach (var movie in movieNotYetSeen)
                {
                    var inputCase = new BasedOnReviewsInput();

                    inputCase.UserId = user.Id;
                    inputCase.MovieId = movie.MovieId;
                    inputCase.MovieTitle = movie.MovieTitle;
                    inputCase.MovieDescription = movie.MovieDescription;
                    inputCase.MovieMaker = movie.MovieMaker;
                    inputCase.UserName = user.UserName;
                    List<Tags> tags = _context.Tags.Where(t => t.MoviesList.Any(m => m.MovieId == movie.MovieId)).ToList();
                    tags.ForEach(tag =>
                    {
                        inputCase.MovieGenres += string.Join(", ", tag.TagName);
                    });
                    inputCase.Gender = user.Gender;
                    inputCase.Nationality = user.Nationality;
                    inputCase.MovieLifeSpan = movie.MovieLifeSpan;
                    
                    var movieRatingPrediction = predictionEngine.Predict(inputCase);
                    var userDTO = new UsersDTO();
                    var movieDTO = new MoviesDTO();
                    var recommendation = new Recommendations();
                    recommendation.MovieId = movie.MovieId;
                    recommendation.MovieTitle = movie.MovieTitle;
                    recommendation.Name = user.Name;
                    recommendation.LastName = user.LastName;
                    recommendation.Email = user.Email;
                    recommendation.AiScore = double.IsNaN(movieRatingPrediction.Score) ? 0 : movieRatingPrediction.Score;
                    recommendation.See = false;
                    recommendation.RequestId = request.RequestId;
                    recommendation.Request = request;
                    result.Add(recommendation);
                    await _context.Recommendations.AddAsync(recommendation);
                }
                await _context.SaveChangesAsync();
            }
            return result;
        }

        private async Task<SentimentPredict> PredictSentimentUser(Users user, Requests? requestUser)
        {
            //E' importante che le reviews siano di tutti gli utenti perchè deve imparare da tutte le casistiche
            //In seguito chiederemo per un utente specifico                 

            MLContext mlContext = new MLContext(seed: 1);

            var requests = await _context.Requests.ToListAsync();

            IDataView dataView = null;

            if (requests.Count > int.Parse(_config["Ai:SentimentPrediction:MinimumLength"]))
            {
                List<SentimentIssue> sentimentModelTrain = new List<SentimentIssue>();

                requests.ForEach(request =>
                {
                    sentimentModelTrain.Add(new SentimentIssue
                    {
                        HowClientFeels = request.HowClientFeels,
                        Label = (bool)request.Sentiment
                    });
                });

                dataView = mlContext.Data.LoadFromEnumerable(sentimentModelTrain);
            }
            else
            {
                dataView = mlContext.Data.LoadFromTextFile<SentimentIssue>(Path.Combine(_config["Ai:SentimentPrediction:PathFileSentimentModelTrain"], _config["Ai:SentimentPrediction:FileNameSentimentModelTrain"]), hasHeader: true);
            }

            TrainTestData trainTestSplit = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

            IDataView trainingData = trainTestSplit.TrainSet;

            IDataView testData = trainTestSplit.TrainSet;

            var dataProcessPipeline = mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(SentimentIssue.HowClientFeels));

            var trainer = mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features");

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            ITransformer trainedModel = trainingPipeline.Fit(trainingData);

            var predictions = trainedModel.Transform(testData);

            var metrics = mlContext.BinaryClassification.Evaluate(data: predictions, labelColumnName: "Label", scoreColumnName: "Score");

            var sentimentIssue = new SentimentIssue
            {
                HowClientFeels = requestUser.HowClientFeels
            };

            var predEngine = mlContext.Model.CreatePredictionEngine<SentimentIssue, SentimentPredict>(trainedModel);

            return predEngine.Predict(sentimentIssue);
        }

        private async Task<List<Recommendations>> BasedOnRequest(Users user, SentimentPredict sentiment, List<Recommendations> recommedations, Requests requestUser)
        {
            //MODEL TRAIN CONSTRUCTION
            List<Recommendations> result = new List<Recommendations>();
            List<BasedOnRequestInput> modelTrain = new List<BasedOnRequestInput>();
            List<Movies> movies = await _context.Movies.ToListAsync();

            foreach (var movie in movies)
            {
                List<Reviews> reviews = await _context.Reviews.Where(r => r.MovieId == movie.MovieId).ToListAsync();
                reviews.ForEach(review =>
                {
                    bool? sentiment = null;
                    List<Recommendations> recommendations = _context.Recommendations.Where(r => r.MovieId == movie.MovieId).ToList();
                    string WhatClientWantsVar = string.Empty;
                    string HowClientFeelsVar = string.Empty;
                    recommedations.ForEach(recommendation =>
                    {
                        var request = _context.Requests.Where(r => r.RequestId == recommendation.RequestId).FirstOrDefault();
                        var session = _context.Sessions.Where(s => s.SessionId == request.SessionId).FirstOrDefault();
                        if (string.Equals(session.UserId, review.UserId))
                        {
                            WhatClientWantsVar = request.WhatClientWants;
                            HowClientFeelsVar = request.HowClientFeels;
                            sentiment = request.Sentiment;
                        }
                    });
                    BasedOnRequestInput model = new BasedOnRequestInput();
                    model.UserId = review.UserId;
                    model.MovieId = movie.MovieId;
                    model.MovieTitle = movie.MovieTitle;
                    model.WhatClientWants = WhatClientWantsVar;
                    model.HowClientFeels = HowClientFeelsVar;
                    model.Label1 = review.Vote;
                    model.Label2 = (bool)sentiment;
                    modelTrain.Add(model);
                });
            }

            MLContext mlContext = new MLContext();

                //SET MODEL INPUT DATA

            IDataView data = mlContext.Data.LoadFromEnumerable(modelTrain);

            var dataProcessingPipeline =
                mlContext
                    .Transforms
                    .Conversion
                    .MapValueToKey(outputColumnName: "UserIdEncoded",
                                    inputColumnName: nameof(BasedOnRequestInput.UserId))
                .Append(mlContext
                        .Transforms
                        .Conversion
                        .MapValueToKey(outputColumnName: "MovieIdEncoded",
                                        inputColumnName: nameof(BasedOnRequestInput.MovieId)))
                .Append(mlContext
                        .Transforms
                        .Text
                        .FeaturizeText(outputColumnName: "MovieTitleFeaturized",
                                        inputColumnName: nameof(BasedOnRequestInput.MovieTitle)))
                .Append(mlContext
                        .Transforms
                        .Text
                        .FeaturizeText(outputColumnName: "WhatClientWantsFeaturized",
                                        inputColumnName: nameof(BasedOnRequestInput.WhatClientWants)))
                .Append(mlContext
                        .Transforms
                        .Text
                        .FeaturizeText(outputColumnName: "HowClientFeelsFeaturized",
                                        inputColumnName: nameof(BasedOnRequestInput.HowClientFeels)))
                .Append(mlContext
                        .Transforms
                        .Concatenate("Features", "MovieTitle", "WhatClientWants", "HowClientFeels"));

            var combinedLabelPipeline = mlContext.Transforms.CustomMapping<BasedOnRequestInput, CombinedLabels>(
                (input, output) => output.Labels = (input.Label1, input.Label2),
                contractName: null
            );

            var finalOptions = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "UserIdEncoded",
                MatrixRowIndexColumnName = "MovieIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 10,
                ApproximationRank = 200,
                Quiet = true,
            };

                //AI SETUP

            var trainer = mlContext.Recommendation().Trainers.MatrixFactorization(finalOptions);

            var trainingPipeLine = dataProcessingPipeline.Append(trainer);

            var model = trainingPipeLine.Fit(data);

            var predictionEngine = mlContext.Model.CreatePredictionEngine<BasedOnRequestInput, BasedOnRequestOutput>(model);

                //RESULTS CONTRUCTION

            foreach (var movie in recommedations)
            {
                var inputCase = new BasedOnRequestInput 
                { 
                    UserId = user.Id, 
                    MovieId = movie.MovieId, 
                    MovieTitle = movie.MovieTitle,
                    WhatClientWants = requestUser.WhatClientWants,
                    HowClientFeels = requestUser.HowClientFeels,
                    Label2 = sentiment.Prediction
                };
                var movieRatingPrediction = predictionEngine.Predict(inputCase);
                var userDTO = new UsersDTO();
                var movieDTO = new MoviesDTO();
                var recommendation = new Recommendations();
                recommendation.MovieId = movie.MovieId;
                recommendation.MovieTitle = movie.MovieTitle;
                recommendation.Name = user.Name;
                recommendation.LastName = user.LastName;
                recommendation.Email = user.Email;
                recommendation.AiScore = double.IsNaN(movieRatingPrediction.Score) ? 0 : movieRatingPrediction.Score;
                recommendation.See = false;
                recommendation.RequestId = requestUser.RequestId;
                recommendation.Request = requestUser;
                result.Add(recommendation);
                await _context.Recommendations.AddAsync(recommendation);
            }
            await _context.SaveChangesAsync();
            return result;
        }
        
        private async Task<Sessions> SessionsManager(Users user, Requests? request)
        {
            var userSession = new Sessions();
            if (_context.Sessions.Any(s => s.DateTimeCreation.Date == DateTime.UtcNow.Date))
            {
                userSession = await _context.Sessions.Where(s => string.Equals(user.Id, s.UserId) && s.DateTimeCreation.Date == DateTime.UtcNow.Date).FirstOrDefaultAsync();
                if (request != null)
                    userSession.RequestList.Add(request);
                else
                    throw new NullReferenceException(); 
                _context.Sessions.Update(userSession);
            }
            else
            {
                userSession.UserId = user.Id;
                userSession.User = user;
                userSession.DateTimeCreation = DateTime.UtcNow;                
                _context.Sessions.Add(userSession);
            }
            await _context.SaveChangesAsync();
            return userSession;
        }

        private async Task<Requests> RequestsManager(Users user, RequestsDTO? request, IList<Recommendations>? recommendations, Sessions? session, bool? sentiment)
        {
            var userRequest = new Requests();
            if(session != null && session.RequestList.Count > 0 && request != null && _context.Requests.Any(r => r.DateTimeRequest.TimeOfDay == DateTime.Now.TimeOfDay && string.Equals(request.WhatClientWants.Trim().ToLower(), r.WhatClientWants.Trim().ToLower()) && string.Equals(request.HowClientFeels.Trim().ToLower(), r.HowClientFeels.Trim().ToLower())))
            {
                userRequest = await _context.Requests.Where(r => r.DateTimeRequest.TimeOfDay == DateTime.Now.TimeOfDay && string.Equals(request.WhatClientWants.Trim().ToLower(), r.WhatClientWants.Trim().ToLower()) && string.Equals(request.HowClientFeels.Trim().ToLower(), r.HowClientFeels.Trim().ToLower())).FirstOrDefaultAsync();
                userRequest.RecommendationsList = recommendations != null || recommendations.Count > 0 ? recommendations.ToList() : new List<Recommendations>();
                userRequest.Sentiment = sentiment != null ? sentiment : null;
                if(session != null)
                {
                    userRequest.Session = session;
                    userRequest.SessionId = session.SessionId;
                }
                _context.Requests.Update(userRequest);
            }
            else
            {
                userRequest.WhatClientWants = request == null || string.IsNullOrEmpty(request.WhatClientWants) ? string.Empty : request.WhatClientWants;
                userRequest.HowClientFeels = request == null || string.IsNullOrEmpty(request.HowClientFeels) ? string.Empty : request.HowClientFeels;
                userRequest.DateTimeRequest = DateTime.Now;
                await _context.Requests.AddAsync(userRequest);
            }
            await _context.SaveChangesAsync();
            return userRequest;
        }

        #endregion
    }
}
