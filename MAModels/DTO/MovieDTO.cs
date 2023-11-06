using MAModels.EntityFrameworkModels;

namespace MAModels.DTO
{
    public class MovieDTO : Movie
    {
        public List<int>? MovieTagsId { get; set; }

        public string? FileName { get; set; }


        public MovieDTO(string MovieTitle, short MovieYearProduction, string MovieDescription, string MovieMaker, bool IsForAdult) : base() 
        {
            this.MovieTitle = MovieTitle;
            this.MovieYearProduction = MovieYearProduction;
            this.MovieDescription = MovieDescription;
            this.MovieMaker = MovieMaker;
            this.IsForAdult = IsForAdult;
        }

        public MovieDTO(Movie movie, List<MovieImage> movieImages) : base()
        {
            this.MovieTitle = movie.MovieTitle;
            this.MovieYearProduction = movie.MovieYearProduction;
            this.MovieDescription = movie.MovieDescription;
            this.MovieMaker = movie.MovieMaker;
            this.IsForAdult = movie.IsForAdult;
            this.MovieImages = movieImages;
        }

        public MovieDTO() { }

        public void InsertMovieTagsId()
        {
            if (MovieTagsId != null && MovieTagsId.Count > 0)
            {
                this.MovieTagsList = new List<MovieDescription>();
                foreach (var item in MovieTagsId)
                {
                    var tag = new MovieDescription
                    {
                        MovieId = this.MovieId,
                        MovieTagId = item
                    };
                    this.MovieTagsList.Add(tag);
                }
            }
        }
    }
}
