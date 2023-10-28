using MAModels.DTO;
using MAModels.EntityFrameworkModels;

namespace MAServices.Interfaces
{
    public interface IMovieServices
    {
        Task<ICollection<Movie>> GetAllMoviesFilteredByUser(string? EmailUser);

        Task<ICollection<Movie>> IsThisMovieAlreadyInDB(string movieTitle, short movieYearProduction, string movieMaker);

        Task<ICollection<Movie>> GetAllMovies();

        Task CreateNewMovie(MovieDTO newMovie);

        Task<Movie?> GetMovieData(int movieId);
    }
}
