using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;

namespace MAServices.Services
{
    public class TagServices : ITagServices
    {
        private readonly ApplicationDbContext _context;

        public TagServices(ApplicationDbContext context)            
        {
            _context = context;
        }

        public async Task<Tag?> GetTag(int tagId) 
        {
            return await _context.Tags.FindAsync(tagId);
        }

        public async Task AssociateMovieToTag(List<MovieTag> movieTagsAssociation)
        {            
            foreach(var movieTag in movieTagsAssociation)
            {
                Tag? tag = await GetTag(movieTag.TagId);
                if (tag == null) throw new ArgumentNullException();
                tag.MoviesList.Add(movieTag.Movie);
                _context.Tags.Update(tag);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<MovieTag?> AssociateTagToMovie(int movieId, Movie movie ,int tagId)
        {
            var movieTag = new MovieTag();
            Tag? tag = await GetTag(tagId);
            if (tag != null)
            {
                movieTag.Tag = tag;
                movieTag.TagId = tagId;
                movieTag.MovieId = movieId;
                movieTag.Movie = movie;
                await _context.MoviesTags.AddAsync(movieTag);
                await _context.SaveChangesAsync();
            }
            return movieTag;
        }
    }
}
