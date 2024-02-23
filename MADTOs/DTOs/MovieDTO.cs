using MAModels.EntityFrameworkModels;

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

        public List<Tag> Tags { get; set; } = new List<Tag>();

        public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();

        public MovieDTO (Movie movie)
        {
            this.MovieId = movie.MovieId;
            this.MovieTitle = movie.MovieTitle;
            this.MovieYearProduction = movie.MovieYearProduction;
            this.MovieDescription = movie.MovieDescription;
            this.MovieMaker = movie.MovieMaker;
            this.IsForAdult = movie.IsForAdult;
        }
        
        public MovieDTO (Movie movie, List<Tag> tags, List<ImageDTO> listImages)
        {
            this.MovieId = movie.MovieId;
            this.MovieTitle = movie.MovieTitle;
            this.MovieYearProduction = movie.MovieYearProduction;
            this.MovieDescription = movie.MovieDescription;
            this.MovieMaker = movie.MovieMaker;
            this.IsForAdult = movie.IsForAdult;
            this.Tags = tags;
            this.Images = listImages;
        }
    }
}
