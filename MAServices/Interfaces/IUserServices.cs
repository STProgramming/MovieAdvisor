using MAModels.DTO;
using MAModels.EntityFrameworkModels;

namespace MAServices.Interfaces
{
    public interface IUserServices
    {
        Task<ICollection<User>> GetAllUsers();

        Task<User?> GetUserData(string emailUser);

        Task CreateNewUser(UserDTO newUserModel);

        Task ModifyUserData(User userData, string? email, string? userName);
    }
}
