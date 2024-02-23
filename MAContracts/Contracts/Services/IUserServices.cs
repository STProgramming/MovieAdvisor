using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAContracts.Contracts.Services
{
    public interface IUserServices
    {
        Task<ICollection<User>> GetAllUsers();

        Task<UserDTO?> GetUserFromEmail(string emailUser);

        Task<UserDTO?> GetUserFromId(int userId);

        Task CreateNewUser(UserDTO newUserModel);

        Task ModifyUserData(UserDTO userData, string? email, string? userName);
    }
}
