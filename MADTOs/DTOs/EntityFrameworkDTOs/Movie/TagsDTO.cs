using MAModels.EntityFrameworkModels;

namespace MADTOs.DTOs.EntityFrameworkDTOs.Movie
{
    public class TagsDTO
    {
        public int TagId { get; set; }

        public string TagName { get; set; } = null!;
    }
}
