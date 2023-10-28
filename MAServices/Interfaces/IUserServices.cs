using MAModels.EntityFrameworkModels;

namespace MAServices.Interfaces
{
    public interface IUserServices
    {
        Task<ICollection<User>> GetAllUsers();

        Task<User?> GetUserData(string emailUser);

        Task CreateNewUser(User newUser);

        Task ModifyUserData(User userData, string? email, string? userName);
    }
}
