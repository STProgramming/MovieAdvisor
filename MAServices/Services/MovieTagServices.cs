using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;

namespace MAServices.Services
{
    public class MovieTagServices : IMovieTagServices
    {
        private readonly ApplicationDbContext _context;
        public MovieTagServices(
            ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<MovieTagDTO> GetMovieTag(int movieTagId) 
        {
            var movieTag = _context.MoviesTags.Any(m => m.MovieTagsId == movieTagId) ? new MovieTagDTO(await _context.MoviesTags.FindAsync(movieTagId)) : new MovieTagDTO();
            return movieTag;
        }

        public async Task SetListMovieTag(List<MovieDescription> movieDescList)
        {            
            MovieTagDTO mt = new MovieTagDTO();
            var movieTag = await GetMovieTag(movieDescList.FirstOrDefault().MovieTagId);
            if (movieTag != null)
            {
                mt.MovieTagsId = movieDescList.FirstOrDefault().MovieTagId;
                mt.MovieTags = movieTag.MovieTags;
                mt.MovieDescriptions = movieDescList;
            }
            _context.MoviesTags.Update(mt);
            await _context.SaveChangesAsync();
        }
    }
}
