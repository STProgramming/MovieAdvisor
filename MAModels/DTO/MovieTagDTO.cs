using MAModels.EntityFrameworkModels;

namespace MAModels.DTO
{
    public class MovieTagDTO : MovieTag
    {
        public List<MovieDescription> MovieDescriptions { get; set; }
        public MovieTagDTO(
            int MovieTagId, 
            string MovieTag,
            List<MovieDescription> MovieDescription
            ) 
        {
            this.MovieTagsId = MovieTagId;
            this.MovieTags = MovieTag;
            this.MovieTagsDescriptionsList = MovieDescription == null || MovieDescription.Count > 0 ? new List<MovieDescription>() : MovieDescription; 
        }

        public MovieTagDTO(
            MovieTag movieTag
            ) 
        {
            this.MovieTagsId = movieTag.MovieTagsId;
            this.MovieTags = movieTag.MovieTags;
            this.MovieTagsDescriptionsList = movieTag.MovieTagsDescriptionsList == null || movieTag.MovieTagsDescriptionsList.Count > 0 ? new List<MovieDescription>() : movieTag.MovieTagsDescriptionsList;
        }

        public MovieTagDTO() { }
    }
}
