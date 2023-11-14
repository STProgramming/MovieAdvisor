using MAModels.EntityFrameworkModels;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MAModels.DTO
{
    public class MovieDTO
    {
        [Required, NotNull]
        public string MovieTitle { get; set; } = null!;

        [Required, NotNull]
        public short MovieYearProduction { get; set; }

        [Required, NotNull]
        public string MovieDescription { get; set; } = null!;

        [Required, NotNull]
        public string MovieMaker { get; set; } = null!;

        [Required, NotNull]
        public bool IsForAdult { get; set; }

        public List<int> TagsId { get; set; } = new List<int>();

        public MovieDTO ConvertToMovieDTO(Movie movie)
        {
            this.MovieTitle = movie.MovieTitle;
            this.MovieYearProduction = movie.MovieYearProduction;
            this.MovieDescription = movie.MovieDescription;
            this.MovieMaker = movie.MovieMaker;
            this.IsForAdult = movie.IsForAdult;
            return this;
        }
    }
}
