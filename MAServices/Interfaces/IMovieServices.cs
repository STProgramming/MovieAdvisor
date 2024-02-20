using MAModels.DTO;
using MAModels.EntityFrameworkModels;


namespace MAServices.Interfaces
{
    public interface IMovieServices
    {
        Task<List<MovieDTO>> SearchEngine(string Query);

        Task<Movie> GetMovieDataById(int movieId);

        Task CreateNewMovie(MovieDTO newMovie);
    }
}
