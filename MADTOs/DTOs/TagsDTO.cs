using MAModels.EntityFrameworkModels;

namespace MADTOs.DTOs
{
    public class TagsDTO
    {
        public int TagId { get; set; }
        
        public string TagName { get; set; } = null!;
    }
}
