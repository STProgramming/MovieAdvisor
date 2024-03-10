using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MADTOs.DTOs.ModelsDTOs
{
    public class NewReviewDTO
    {
        [Required, NotNull]
        public int MovieId { get; set; }

        public string? DescriptionVote { get; set; }

        [Required, NotNull]
        public float Vote { get; set; }
        
        public DateTime? When { get; set; }
    }
}
