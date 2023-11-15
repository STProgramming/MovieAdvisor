using MAModels.EntityFrameworkModels;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MAModels.DTO
{
    public class MovieDTO
    {
        public int MovieId { get; set; } = 0;
        public string MovieTitle { get; set; } = null!;

        public short MovieYearProduction { get; set; }

        public string MovieDescription { get; set; } = null!;

        public string MovieMaker { get; set; } = null!;

        public bool IsForAdult { get; set; }

        public List<int> TagsId { get; set; } = new List<int>();

        public MovieDTO ConvertToMovieDTO(Movie movie)
        {
            this.MovieId = movie.MovieId;
            this.MovieTitle = movie.MovieTitle;
            this.MovieYearProduction = movie.MovieYearProduction;
            this.MovieDescription = movie.MovieDescription;
            this.MovieMaker = movie.MovieMaker;
            this.IsForAdult = movie.IsForAdult;
            return this;
        }
    }
}
