using MADTOs.DTOs.EntityFrameworkDTOs.AI;

namespace MAContracts.Contracts.Services.AI
{
    public interface ISessionServices
    {
        Task<IList<SessionsDTO>> GetSessionsByUser(string userId);
    }
}
