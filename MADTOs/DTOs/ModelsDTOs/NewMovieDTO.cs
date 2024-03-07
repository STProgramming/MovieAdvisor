using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MADTOs.DTOs.ModelsDTOs
{
    public class NewMovieDTO
    {
        [Required, NotNull]
        public string MovieTitle { get; set; }

        [Required, NotNull]
        public short MovieYearProduction { get; set; }

        [Required, NotNull]
        public string MovieDescription { get; set; }

        [Required, NotNull]
        public string MovieMaker { get; set; }

        [Required, NotNull]
        public short MovieLifeSpan { get; set; }

        [Required, NotNull]
        public bool IsForAdult { get; set; }

        [Required, NotNull]
        public List<short> TagsId { get; set; }
    }
}
