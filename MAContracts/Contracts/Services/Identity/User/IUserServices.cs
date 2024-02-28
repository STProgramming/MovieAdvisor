using MADTOs.DTOs;
using MAModels.EntityFrameworkModels.Identity;

namespace MAContracts.Contracts.Services.Identity.User
{
    public interface IUserServices
    {
        Task<UsersDTO?> GetUserFromEmail(string emailUser);

        Task<UsersDTO?> GetUserFromId(string userId);

        Task CreateNewUser(UsersDTO newUserModel);


        //Task ModifyUserData(UserDTO userData, string? email, string? userName);
    }
}
