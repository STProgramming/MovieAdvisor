using MAModels.EntityFrameworkModels;

namespace MAServices.Interfaces
{
    public interface ITagServices
    {
        Task<Tag?> GetTag(int tagId);

        Task AssociateMovieToTag(List<MovieTag> movieTagsAssociation);

        Task<MovieTag?> AssociateTagToMovie(int movieId, Movie movie, int tagId);
    }
}
