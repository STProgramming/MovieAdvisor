using MADTOs.DTOs.EntityFrameworkDTOs;

namespace MADTOs.DTOs.ModelsDTOs
{
    public class MoviesSearchResultsDTO
    {
        public int MoviesCount { get; set; }

        public List<MoviesDTO> Movies { get; set; } = new List<MoviesDTO>();

        public List<MoviesDTO> ResultsForYear { get; set; } = new List<MoviesDTO>();

        public List<MoviesDTO> ResultsForLifeSpan { get; set; } = new List<MoviesDTO>();

        public List<MoviesDTO> ResultsForTitle { get; set; } = new List<MoviesDTO>();

        public List<MoviesDTO> ResultsForMaker { get; set; } = new List<MoviesDTO>();

        public List<MoviesDTO> ResultsForTag { get; set; } = new List<MoviesDTO>();

        public List<MoviesDTO> ResultsForDescription { get; set; } = new List<MoviesDTO>();
    }
}
