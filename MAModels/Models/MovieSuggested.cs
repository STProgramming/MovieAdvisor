using MAModels.DTO;

namespace MAModels.Models
{
    public class MovieSuggested
    {
        public int MovieId { get; set; }

        public int UserId { get; set; }

        public MovieDTO Movie { get; set; } = new MovieDTO();

        public UserDTO User { get; set; } = new UserDTO();

        public float Label {  get; set; }

        public float Score { get; set; }
    }
}
