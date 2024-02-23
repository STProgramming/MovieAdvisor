using MAModels.EntityFrameworkModels;

namespace MADTOs.DTOs
{
    public class TagDTO
    {
        public int TagId { get; set; }
        
        public string TagName { get; set; } = null!;

        public TagDTO(Tag tag) 
        {
            TagId = tag.TagId;
            TagName = tag.TagName;
        }
    }
}
