using System.ComponentModel.DataAnnotations;

namespace MAModels.EntityFrameworkModels
{
    public class MovieImage
    {
        [Key]
        public int MovieImageId { get; set; }

        [Required]
        public string MovieImageName { get; set; }

        [Required]
        public string MovieImagePath { get; set; }

        [Required]
        public string MovieImageExtension { get; set; }

        public int MovieId {  get; set; }

        public Movie Movie { get; set; }

        public MovieImage(
            string ImageName,
            string pathServer,
            int movieId,
            Movie movie
        ) 
        {
            MovieImageName = ImageName;
            MovieImageExtension = Path.GetExtension(MovieImageName);
            MovieImagePath = Path.Combine(pathServer, MovieImageName);
            MovieId = movieId;
            Movie = movie;
        }

        public MovieImage(MovieImage im)
        {
            MovieImageName = im.MovieImageName;
            MovieImageExtension = im.MovieImageName;
            MovieImagePath = im.MovieImagePath;
            MovieId = im.MovieId;
            Movie = im.Movie;
        }

    }
}
