using MAModels.EntityFrameworkModels;

namespace MAModels.DTO
{
    public class MovieDTO : Movie
    {
        public List<int> TagsId { get; set; } = new List<int>();
        public MovieDTO(string MovieTitle, short MovieYearProduction, string MovieDescription, string MovieMaker, bool IsForAdult, List<int> TagsId) : base() 
        {
            this.MovieTitle = MovieTitle;
            this.MovieYearProduction = MovieYearProduction;
            this.MovieDescription = MovieDescription;
            this.MovieMaker = MovieMaker;
            this.IsForAdult = IsForAdult;            
        }

        public void InsertTagsList(List<Tag> tags)
        {
            this.TagsList = tags;
        }

        public MovieDTO() { }
    }
}
