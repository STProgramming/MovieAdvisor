namespace MADTOs.DTOs
{
    public class MoviesDTO
    {
        public int MovieId { get; set; } = 0;

        public string MovieTitle { get; set; } = null!;

        public short MovieYearProduction { get; set; }

        public string MovieDescription { get; set; } = null!;

        public string MovieMaker { get; set; } = null!;

        public bool IsForAdult { get; set; }

        public List<TagsDTO> Tags { get; set; } = new List<TagsDTO>();

        public List<ImagesDTO> Images { get; set; } = new List<ImagesDTO>();
    }
}
