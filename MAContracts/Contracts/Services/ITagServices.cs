using MADTOs.DTOs;

namespace MAContracts.Contracts.Services
{
    public interface ITagServices
    {
        Task<TagsDTO> GetTag(int tagId);

        Task<List<TagsDTO>> GetAllTags();

        Task CreateAllTags();
    }
}
