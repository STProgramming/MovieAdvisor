using MADTOs.DTOs.EntityFrameworkDTOs.Movie;

namespace MAContracts.Contracts.Services.Movie
{
    public interface ITagServices
    {
        Task<TagsDTO> GetTag(int tagId);

        Task<List<TagsDTO>> GetAllTags();

        Task CreateAllTags();
    }
}
