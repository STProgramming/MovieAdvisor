using MAAI.Interfaces;
using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MAAI
{
    public class ModelTrainCore : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private static List<User> Users = new List<User>();

        public ModelTrainCore(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {  
                await LearnModelsFromUserPreferencies();

                await Task.Delay(new TimeSpan(3, 0, 0));
            }
        }

        private async Task LearnModelsFromUserPreferencies()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    Users = context.Users.ToList();
                    foreach (var user in Users)
                    {
                        var allReviewsFromUser = context.Reviews.Where(r => r.UserId == user.UserId).ToList();
                        List<ReviewDTO> reviewsDto = new List<ReviewDTO>();
                        List<MovieDTO> movieReviewed = new List<MovieDTO>();
                        foreach (var rev in allReviewsFromUser)
                        {
                            if (rev.MovieId > 0)
                            {
                                var resultMovie = await context.Movies.FindAsync(rev.MovieId);
                                rev.Movie = resultMovie != null ? resultMovie : new Movie();
                            }
                            if (rev.UserId > 0)
                            {
                                var resultUser = await context.Users.FindAsync(rev.UserId);
                                rev.User = resultUser != null ? resultUser : new User();
                            }
                            if (rev.User.UserId > 0 && rev.Movie.MovieId > 0)
                            {
                                ReviewDTO review = new ReviewDTO();
                                MovieDTO movieDto = new MovieDTO();
                                reviewsDto.Add(review.ConvertToReviewDTO(rev));
                                movieReviewed.Add(movieDto.ConvertToMovieDTO(rev.Movie));
                                string TagsNameList = string.Empty;
                                List<Tag> movieTags = new List<Tag>();
                                foreach (int tagId in movieDto.TagsId)
                                {
                                    var tag = context.Tags.Where(t => t.TagId == tagId).FirstOrDefault();
                                    if (tag != null)
                                    {
                                        if (rev.Movie.TagsList == null)
                                        {
                                            rev.Movie.TagsList = new List<Tag>();
                                        }
                                        rev.Movie.TagsList.Add(tag);
                                    }
                                }
                                if (rev.Movie.TagsList != null && rev.Movie.TagsList.Count > 0)
                                {
                                    short counter = 0;
                                    foreach (var tag in rev.Movie.TagsList)
                                    {
                                        if (counter == rev.Movie.TagsList.Count - 1)
                                        {
                                            TagsNameList += tag.TagName;
                                        }
                                        else
                                        {
                                            TagsNameList += tag.TagName + ", ";
                                        }
                                        counter++;
                                    }
                                }
                                Preference result = new Preference
                                {
                                    Email = rev.User.EmailAddress,
                                    UserId = rev.UserId,
                                    Name = rev.User.Name,
                                    LastName = rev.User.LastName,
                                    UserName = rev.User.UserName,
                                    DescriptionVote = string.IsNullOrEmpty(rev.DescriptionVote) ? string.Empty : rev.DescriptionVote,
                                    Vote = rev.Vote,
                                    MovieId = rev.Movie.MovieId,
                                    MovieTitle = rev.Movie.MovieTitle,
                                    MovieDescription = rev.Movie.MovieDescription,
                                    MovieYear = rev.Movie.MovieYearProduction,
                                    MovieMaker = rev.Movie.MovieMaker,
                                    MovieGenres = TagsNameList,
                                    DateTimeCreation = DateTime.Now,
                                };
                                if (!context.ModelsTrain.Any(t => t.UserId == result.UserId && string.Equals(t.MovieTitle, result.MovieTitle) && string.Equals(t.MovieMaker, result.MovieMaker) && t.MovieYear == result.MovieYear))
                                {
                                    await context.ModelsTrain.AddAsync(result);
                                    await context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
