using MADTOs.DTOs;

namespace MAContracts.Contracts.Services
{
    public interface ITagServices
    {
        Task<TagDTO> GetTag(int tagId);

        Task<List<TagDTO>> GetAllTags();

        Task CreateAllTags();
    }
}
