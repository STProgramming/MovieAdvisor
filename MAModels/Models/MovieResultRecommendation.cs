using MAModels.DTO;

namespace MAModels.Models
{
    public class MovieResultRecommendation
    {
        public int MovieId { get; set; }

        public int UserId { get; set; }

        public MovieDTO MovieObj { get; set; } = new MovieDTO();

        public UserDTO UserObj { get; set; } = new UserDTO();

        public float Score { get; set; }
    }
}
