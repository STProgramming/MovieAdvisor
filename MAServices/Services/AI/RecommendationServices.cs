using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services.AI;
using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.EntityFrameworkDTOs.AI;
using MADTOs.DTOs.EntityFrameworkDTOs.Identity;
using MADTOs.DTOs.ModelsDTOs.AI;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.AI;
using MAModels.EntityFrameworkModels.Identity;
using MAModels.EntityFrameworkModels.Movie;
using MAModels.Exceptions.AI;
using MAModels.Models.AI.RecommendationServices.RecommendationBasedOnRequest;
using MAModels.Models.AI.RecommendationServices.RecommendationBasedOnReviews;
using MAModels.Models.AI.RecommendationServices.SentimentPrediction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Tensorflow;
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

        public async Task<List<RecommendationsDTO>> RecommendationsBasedOnReviews(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new NullReferenceException();

            await AISmartAvailability();

            await AISmartUserKnowledge(user);

            List<Recommendations> result = await UserCoerenceOnTrigger(user, null);

            if(result == null)
            {
                var sessionInfo = await SessionsManager(user, null);

                var request = await CreateRequest(null, sessionInfo);            

                result = await BasedOnReviews(user, request);

                if(await RecommendationServiceCoerence(result, sessionInfo, request))
                {
                    var requestUpdate = await UpdateRequet(request, result);

                    await SessionsManager(user, requestUpdate);
                }
            }

            return _mapper.RecommendationMapperDtoListService(result);
        }

        public async Task<List<RecommendationsDTO>> RecommendationsBasedOnRequest(string userId, NewRequestDTO requestUser)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new NullReferenceException();

            await AISmartAvailability();

            await AISmartUserKnowledge(user);

            List<Recommendations> result = await UserCoerenceOnTrigger(user, requestUser);

            if(result == null)
            {
                var sessionInfo = await SessionsManager(user, null);

                var request = await CreateRequest(requestUser, sessionInfo);

                result = await BasedOnReviews(user, request);

                SentimentPredict sentimentUser = await PredictSentimentUser(request);

                List<Recommendations> defResult = await BasedOnRequest(user, sentimentUser, result, request);

                if(await RecommendationServiceCoerence(defResult, sessionInfo, request))
                {
                    var requestUpdate = await UpdateRequet(request, defResult);

                    await SessionsManager(user, requestUpdate);
                }

                return _mapper.RecommendationMapperDtoListService(defResult);
            }

            return _mapper.RecommendationMapperDtoListService(result);
        }

        #endregion

        #region PRIVATE METHODS

        private async Task AISmartAvailability()
        {
            var reviews = await _context.Reviews.ToListAsync();

            if (reviews.Count < Int32.Parse(_config["Ai:Recommendation:MinimumLength"])) throw new InsufficientReviewsException();
        }

        private async Task AISmartUserKnowledge(Users user)
        {
            var reviewsForUser = await _context.Reviews.Where(r => string.Equals(r.UserId, user.Id)).ToListAsync();
            if (reviewsForUser == null || reviewsForUser.Count < Int32.Parse(_config["Ai:Recommendation:MinimumReviewsForUser"])) throw new InsufficientReviewsException();
        }

        private async Task<bool> AISmartRecommendationMovie(Movies movies)
        {
            var reviews = await _context.Reviews.Where(r => r.MovieId == movies.MovieId).ToListAsync();
            return reviews.Count > Int32.Parse(_config["Ai:Recommendation:MinimumReviewsForMovie"]);
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
                    train.MovieLifeSpan = movie.MovieLifeSpan.ToString();
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

                AISmartEvaluate(mlContext, data, model);

                var predictionEngine = mlContext.Model.CreatePredictionEngine<BasedOnReviewsInput, BasedOnReviewsOutput>(model);

                //RESULTS CONTRUCTION

                short counter = 0;
                foreach (var movie in movieNotYetSeen)
                {
                    //HO TOLTO QUESTO CONTROLLO PERCHE' DEVO TESTARE LA FUNZIONALITA' 
                    //TODO DA RIAGGIUNGERE QUANDO SARA' PUBBLICATO
                    //await AISmartRecommendationMovie(movie) && 
                    if (counter < Convert.ToInt32(_config["Ai:Recommendation:MaxLentghRecommendationMovies"]))
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
                        inputCase.MovieLifeSpan = movie.MovieLifeSpan.ToString();      
                        var movieRatingPrediction = predictionEngine.Predict(inputCase); 
                        //lo score minimum ora è 2 quando sarà in prod sarà più alto
                        //todo
                        if(movieRatingPrediction.Score >= Int32.Parse(_config["Ai:Recommendation:MinimumScoreAi"]))
                        {
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
                            counter++;
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return result;
        }

        private RegressionMetrics AISmartEvaluate(MLContext mlContext, IDataView testDataView, ITransformer model)
        {
            var prediction = model.Transform(testDataView);
            return mlContext.Regression.Evaluate(prediction);
        }

        private async Task<SentimentPredict> PredictSentimentUser(Requests requestUser)
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
                    if(!string.IsNullOrEmpty(request.HowClientFeels) && !string.IsNullOrEmpty(request.WhatClientWants) && request.Sentiment != null)
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

            AISmartEvaluate(mlContext, dataView, trainedModel);

            var predictions = trainedModel.Transform(testData);

            var metrics = mlContext.BinaryClassification.Evaluate(data: predictions, labelColumnName: "Label", scoreColumnName: "Score");

            var sentimentIssue = new SentimentIssue
            {
                HowClientFeels = requestUser.HowClientFeels
            };

            var predEngine = mlContext.Model.CreatePredictionEngine<SentimentIssue, SentimentPredict>(trainedModel);

            var resultSentiment = predEngine.Predict(sentimentIssue);

            requestUser.Sentiment = resultSentiment.Score > float.Parse(_config["Ai:SentimentPrediction:MinimumScoreAi"]) ? resultSentiment.Prediction : null;
            
            if(requestUser.Sentiment != null)
            {
                _context.Requests.Update(requestUser);
                await _context.SaveChangesAsync();
            }
            return resultSentiment;
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
                        if(request != null && session != null)
                        {
                            if (string.Equals(session.UserId, review.UserId))
                            {
                                WhatClientWantsVar = request.WhatClientWants;
                                HowClientFeelsVar = request.HowClientFeels;
                                sentiment = request.Sentiment;
                            }
                        }
                    });
                    if (sentiment != null)
                    {
                        BasedOnRequestInput model = new BasedOnRequestInput();
                        model.UserId = review.UserId;
                        model.MovieId = movie.MovieId;
                        model.MovieTitle = movie.MovieTitle;
                        model.WhatClientWants = WhatClientWantsVar;
                        model.HowClientFeels = HowClientFeelsVar;
                        model.Label1 = review.Vote;
                        model.Label2 = (bool)sentiment;
                        modelTrain.Add(model);
                    }
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

            AISmartEvaluate(mlContext, data, model);

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
                if (movieRatingPrediction.Score >= Int32.Parse(_config["Ai:Recommendation:MinimumScoreAi"]))
                {
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
            }
            await _context.SaveChangesAsync();
            return result;
        }
        
        private async Task<Sessions> SessionsManager(Users user, Requests? request)
        {
            var userSession = new Sessions();
            if (_context.Sessions.Any(s => s.DateTimeCreation.Date == DateTime.UtcNow.Date && string.Equals(s.UserId, user.Id)))
            {
                userSession = await _context.Sessions.Where(s => string.Equals(user.Id, s.UserId) && s.DateTimeCreation.Date == DateTime.UtcNow.Date).FirstOrDefaultAsync();
                if (request != null)
                    userSession.RequestList.Add(request);
                _context.Sessions.Update(userSession);
            }
            else
            {
                userSession.UserId = user.Id;
                userSession.User = user;
                userSession.DateTimeCreation = DateTime.Now;                
                _context.Sessions.Add(userSession);
                user.SessionsList.Add(userSession);
                _context.Users.Update(user);
            }
            await _context.SaveChangesAsync();
            return userSession;
        }

        private async Task<Requests> CreateRequest(NewRequestDTO? newRequestDto, Sessions sessionUser)
        {
            Requests newRequest = new Requests
            {
                WhatClientWants = newRequestDto == null || string.IsNullOrEmpty(newRequestDto.WhatClientWants) ? string.Empty : newRequestDto.WhatClientWants,
                HowClientFeels = newRequestDto == null || string.IsNullOrEmpty(newRequestDto.HowClientFeels) ? string.Empty : newRequestDto.HowClientFeels,
                Sentiment = null,
                SessionId = sessionUser.SessionId,
                Session = sessionUser,
                DateTimeRequest = DateTime.Now,
            };
            await _context.Requests.AddAsync(newRequest);
            await _context.SaveChangesAsync();
            return newRequest;
        }

        private async Task<Requests> UpdateRequet(Requests actualRequest, List<Recommendations> listRecommendations)
        {
            actualRequest.RecommendationsList = listRecommendations;
            _context.Requests.Update(actualRequest);
            await _context.SaveChangesAsync();
            return actualRequest;
        }

        private async Task<List<Recommendations>?> UserCoerenceOnTrigger(Users user, NewRequestDTO? request)
        {
            var sessions = await _context.Sessions.Where(s => string.Equals(user.Id, s.UserId)).ToListAsync();
            int timeOut = 0;
            List<Recommendations> resultRecoms = new List<Recommendations>();
            if (sessions == null || sessions.Count == 0) return null;
            foreach(var session in sessions)
            {
                var requestList = await _context.Requests.Where(r => r.SessionId == session.SessionId).ToListAsync();
                foreach (var requestSession in requestList)
                {
                    var recomList = await _context.Recommendations.Where(r => r.RequestId == requestSession.RequestId).ToListAsync();
                    if (request == null || (!string.Equals(requestSession.HowClientFeels, request.HowClientFeels) && !string.Equals(requestSession.WhatClientWants, request.WhatClientWants) && session.DateTimeCreation == DateTime.Now))
                    {
                        foreach (var rec in recomList)
                        {
                            if (rec.See != true)
                            {
                                if (resultRecoms.Count == 0 || resultRecoms.Any(r => r.MovieId != rec.MovieId)) resultRecoms.Add(rec);                                
                                var movie = await _context.Movies.Where(m => m.MovieId == rec.MovieId).FirstOrDefaultAsync();
                                if (movie == null) break;
                                if(timeOut < Int32.MaxValue) timeOut =+ timeOut + movie.MovieLifeSpan;
                            }                            
                        }
                    }
                }
            }
            int res = timeOut / 1440;
            if (res > 1) return resultRecoms;
            else return null;            
        }

        private async Task<bool> RecommendationServiceCoerence(List<Recommendations> recommendations, Sessions session, Requests requests)
        {
            var user = await this._userManager.FindByIdAsync(session.UserId);
            if (user == null) throw new NullReferenceException();
            var recomList = await _context.Recommendations.ToListAsync();
            var recomInnerJoinList = from recommendation in recommendations
                                     join recom in recomList on recommendation.MovieId equals recom.MovieId
                                     where recom.Email == user.Email
                                     select new
                                     {
                                         recom.RecommendationId,
                                         recom.MovieId,
                                         recom.MovieTitle,
                                         recom.Name,
                                         recom.LastName,
                                         recom.Email,
                                         recom.AiScore,
                                         recom.See,
                                         recom.RequestId,
                                         recom.Request
                                     };
            if (recomInnerJoinList != null && recomInnerJoinList.Count() > 0)
            {
                recommendations.ForEach(recom =>
                {
                    _context.Recommendations.Attach(recom);
                    _context.Recommendations.Remove(recom);
                });
                //Delete request
                _context.Requests.Attach(requests);
                _context.Requests.Remove(requests);

                //Delete Session 
                _context.Sessions.Attach(session);
                _context.Sessions.Remove(session);

                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
