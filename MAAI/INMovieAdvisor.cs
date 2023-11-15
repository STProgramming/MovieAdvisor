using MAModels.DTO;
using MAModels.EntityFrameworkModels;

namespace MAAI
{
    public interface INMovieAdvisor
    {
        Task<List<MovieDTO>> NMoviesSuggestedByUser(User user);
    }
}
