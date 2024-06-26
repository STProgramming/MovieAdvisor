﻿using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services.AI;
using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.EntityFrameworkDTOs.AI;
using MADTOs.DTOs.EntityFrameworkDTOs.Identity;
using MADTOs.DTOs.ModelsDTOs.AI;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.AI;
using MAModels.EntityFrameworkModels.Identity;
using MAModels.EntityFrameworkModels.Movie;
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
using static Microsoft.ML.DataOperationsCatalog;

namespace MAServices.Services.AI
{
    public class RecommendationServices : IRecommendationServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _context;

        private readonly UserManager<Users> _userManager;

        private readonly IConfiguration _config;

        private readonly IObjectsMapperDtoServices _mapper;

        public RecommendationServices(
            IDbContextFactory<ApplicationDbContext> context,
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

            Task<bool> result1 = AISmartAvailability();

            Task<bool> result2 = AISmartUserKnowledge(user);

            bool[] controls = await Task.WhenAll(result1, result2);

            if (controls[0] == true || controls[1] == true)
            {
                List<Recommendations> result = new List<Recommendations>();

                result = await BasedOnReviews(user);

                result = await RecommendationServiceCoerence(result, user, new NewRequestDTO());

                return _mapper.RecommendationMapperDtoListService(result);
            }
            throw new InsufficientExecutionStackException();
        }

        //public async Task<List<RecommendationsDTO>> RecommendationsBasedOnRequest(string userId, NewRequestDTO requestUser)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null) throw new NullReferenceException();

        //    await AISmartAvailability();

        //    await AISmartUserKnowledge(user);

        //    List<Recommendations> result = await UserCoerenceOnTrigger(user, requestUser);

        //    if(result == null)
        //    {
        //        var sessionInfo = await SessionsManager(user, null);

        //        var request = await CreateRequest(requestUser, sessionInfo);

        //        result = await BasedOnReviews(user);

        //        SentimentPredict sentimentUser = await PredictSentimentUser(request);

        //        List<Recommendations> defResult = await BasedOnRequest(user, sentimentUser, result, request);

        //        if(await RecommendationServiceCoerence(defResult, sessionInfo, request))
        //        {
        //            var requestUpdate = await UpdateRequet(request, defResult);

        //            await SessionsManager(user, requestUpdate);
        //        }

        //        return _mapper.RecommendationMapperDtoListService(defResult);
        //    }

        //    return _mapper.RecommendationMapperDtoListService(result);
        //}

        #endregion

        #region PRIVATE METHODS

        private async Task<bool> AISmartAvailability()
        {
            using (var ctx = await _context.CreateDbContextAsync())
            {
                var reviews = await ctx.Reviews.ToListAsync();
                if (reviews.Count < Int32.Parse(_config["Ai:Recommendation:MinimumLength"])) return false;
                else return true;
            }
        }

        private async Task<bool> AISmartUserKnowledge(Users user)
        {
            using (var ctx = await _context.CreateDbContextAsync())
            {
                var reviewsForUser = await ctx.Reviews.Where(r => string.Equals(r.UserId, user.Id)).ToListAsync();
                if (reviewsForUser == null || reviewsForUser.Count < Int32.Parse(_config["Ai:Recommendation:MinimumReviewsForUser"])) return false;
                else return true;
            }
        }

        private async Task<bool> AISmartRecommendationMovie(Movies movies)
        {
            using (var ctx = await _context.CreateDbContextAsync())
            {
                var reviews = await ctx.Reviews.Where(r => r.MovieId == movies.MovieId).ToListAsync();
                return reviews.Count > Int32.Parse(_config["Ai:Recommendation:MinimumReviewsForMovie"]);
            }
        }

        private async Task<List<Recommendations>> BasedOnReviews(Users user)
        {
            //MODEL TRAIN CONSTRUCTION
            List<Reviews> reviews = new List<Reviews>();
            using (var ctx = await _context.CreateDbContextAsync())
            {
                reviews = await ctx.Reviews.ToListAsync();
            }
            List<BasedOnReviewsInput> modelTrain = new List<BasedOnReviewsInput>();
            MLContext mlContext = new MLContext();
            var result = new List<Recommendations>();
            List<BasedOnReviewsOutput> movieSuggesteds = new List<BasedOnReviewsOutput>();

            //Caricamento dei film non visti dagl' utenti

            List<Movies> movieNotYetSeen = new List<Movies>();
            using (var ctx = await _context.CreateDbContextAsync())
            {
                var reviewsList = await ctx.Reviews.Where(r => !(string.Equals(r.UserId, user.Id))).ToListAsync();
                reviewsList.ForEach(async review =>
                {
                    var movie = await ctx.Movies.Where(m => m.MovieId == review.MovieId).FirstOrDefaultAsync();                    
                    movieNotYetSeen.Add(movie);
                });
            }

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
                using( var ctx = await _context.CreateDbContextAsync())
                {
                    foreach (var review in reviews)
                    {
                        BasedOnReviewsInput train = new BasedOnReviewsInput();
                        Movies movie = new Movies();
                        if (review.Movie == null) movie = await ctx.Movies.Where(m => m.MovieId == review.MovieId).FirstOrDefaultAsync();
                        else movie = review.Movie;
                        List<Tags> tags = ctx.Tags.Where(t => t.MoviesList.Any(m => m.MovieId == movie.MovieId)).ToList();
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

                        List<Tags> tags = new List<Tags>();

                        using (var ctx = await _context.CreateDbContextAsync())
                        {
                            tags = ctx.Tags.Where(t => t.MoviesList.Any(m => m.MovieId == movie.MovieId)).ToList();
                        }

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
                            recommendation.RequestId = 0;
                            recommendation.Request = new Requests();
                            result.Add(recommendation);
                            counter++;
                        }
                    }
                }
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

            List<Requests> requests = new List<Requests>();
            using (var ctx = await _context.CreateDbContextAsync())
            {
                requests = await ctx.Requests.ToListAsync();
            }

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
                using (var ctx = await _context.CreateDbContextAsync())
                {
                    ctx.Requests.Update(requestUser);
                    await ctx.SaveChangesAsync();
                }
            }
            return resultSentiment;
        }

        private async Task<List<Recommendations>> BasedOnRequest(Users user, SentimentPredict sentiment, List<Recommendations> recommedations, Requests requestUser)
        {
            //MODEL TRAIN CONSTRUCTION
            List<Recommendations> result = new List<Recommendations>();
            List<BasedOnRequestInput> modelTrain = new List<BasedOnRequestInput>();
            List<Movies> movies = new List<Movies>();

            using (var ctx = await _context.CreateDbContextAsync())
            {
                movies = await ctx.Movies.ToListAsync();

                foreach (var movie in movies)
                {
                    List<Reviews> reviews = await ctx.Reviews.Where(r => r.MovieId == movie.MovieId).ToListAsync();
                    reviews.ForEach(review =>
                    {
                        bool? sentiment = null;
                        List<Recommendations> recommendations = ctx.Recommendations.Where(r => r.MovieId == movie.MovieId).ToList();
                        string WhatClientWantsVar = string.Empty;
                        string HowClientFeelsVar = string.Empty;
                        recommedations.ForEach(recommendation =>
                        {
                            var request = ctx.Requests.Where(r => r.RequestId == recommendation.RequestId).FirstOrDefault();
                            var session = ctx.Sessions.Where(s => s.SessionId == request.SessionId).FirstOrDefault();
                            if (request != null && session != null)
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

            using (var ctx = await _context.CreateDbContextAsync())
            {
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
                        await ctx.Recommendations.AddAsync(recommendation);
                    }
                }
                await ctx.SaveChangesAsync();
            }

            return result;
        }
        
        private async Task<Sessions> SessionsManager(Users user, Requests? request)
        {
            var userSession = new Sessions();
            using (var ctx = await _context.CreateDbContextAsync())
            {
                if (ctx.Sessions.Any(s => s.DateTimeCreation.Date == DateTime.UtcNow.Date && string.Equals(s.UserId, user.Id)))
                {
                    userSession = await ctx.Sessions.Where(s => string.Equals(user.Id, s.UserId) && s.DateTimeCreation.Date == DateTime.UtcNow.Date).FirstOrDefaultAsync();
                    if (request != null)
                        userSession.RequestList.Add(request);
                    ctx.Sessions.Update(userSession);
                }
                else
                {
                    userSession.UserId = user.Id;
                    userSession.DateTimeCreation = DateTime.Now;
                    ctx.Sessions.Add(userSession);
                }
            await ctx.SaveChangesAsync();
            }
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

            using (var ctx = await _context.CreateDbContextAsync())
            {
                await ctx.Requests.AddAsync(newRequest);
                await ctx.SaveChangesAsync();
            }

            return newRequest;
        }

        private async Task<Requests> UpdateRequet(Requests actualRequest, List<Recommendations> listRecommendations)
        {
            using (var ctx = await _context.CreateDbContextAsync())
            {
                actualRequest.RecommendationsList = listRecommendations;
                ctx.Requests.Update(actualRequest);
                await ctx.SaveChangesAsync();
            }
            return actualRequest;
        }

        private async Task SaveRecommendations(List<Recommendations> recommendations, Requests request)
        {
            using (var ctx = await _context.CreateDbContextAsync())
            {
                recommendations.ForEach(async recommendation =>
                {
                    recommendation.RequestId = request.RequestId;
                    recommendation.Request = request;
                    await ctx.Recommendations.AddAsync(recommendation);
                });
                await ctx.SaveChangesAsync();
            }
        }

        private async Task<List<Recommendations>> RecommendationServiceCoerence(List<Recommendations> recommendations, Users user, NewRequestDTO requestUser)
        {
            using (var ctx = await _context.CreateDbContextAsync())
            {
                List<Recommendations> results = new List<Recommendations>();
                var recoms = await ctx.Recommendations
                                     .Where(r => string.Equals(r.Email, user.Email)).ToListAsync();
                if (recoms.Count == 0 || !(recoms.Any(r => recommendations.Any(x => x.MovieId == r.MovieId))))
                {
                    Sessions session = await SessionsManager(user, null);
                    Requests request = await CreateRequest(requestUser, session);
                    await SaveRecommendations(recommendations, request);
                    request = await UpdateRequet(request, recommendations);
                    await SessionsManager(user, request);
                    results = recommendations;
                }
                else
                {
                    var recomListNotSeen = await ctx.Recommendations
                                           .Where(r => string.Equals(r.Email, user.Email) &&
                                           r.See == false).ToListAsync();
                    if(recomListNotSeen.Count > 0)
                    {
                        for (int i = 0; i < Convert.ToInt32(_config["Ai:Recommendation:MaxLentghRecommendationMovies"]); i++)
                        {
                            if(recomListNotSeen.Count() >= i)
                            {
                                results.Add(recomListNotSeen[i]);
                            }
                        }                
                    }
                }
                return results;
            }
        }

        #endregion
    }
}
