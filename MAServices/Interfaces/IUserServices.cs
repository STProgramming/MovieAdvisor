using MAModels.DTO;
using MAModels.EntityFrameworkModels;

namespace MAServices.Interfaces
{
    public interface IUserServices
    {
        Task<ICollection<User>> GetAllUsers();

        Task<User?> GetUserFromEmail(string emailUser);

        Task<User?> GetUserFromId(int userId);

        Task CreateNewUser(UserDTO newUserModel);

        Task ModifyUserData(User userData, string? email, string? userName);
    }
}
