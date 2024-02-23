namespace MADTOs.DTOs
{
    public class MovieDTO
    {
        public int MovieId { get; set; } = 0;

        public string MovieTitle { get; set; } = null!;

        public short MovieYearProduction { get; set; }

        public string MovieDescription { get; set; } = null!;

        public string MovieMaker { get; set; } = null!;

        public bool IsForAdult { get; set; }

        public List<TagDTO> Tags { get; set; } = new List<TagDTO>();

        public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();
    }
}
