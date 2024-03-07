using MADTOs.DTOs.EntityFrameworkDTOs.Identity;
using MADTOs.DTOs.ModelsDTOs;

namespace MAContracts.Contracts.Services.Identity.User
{
    public interface IUserServices
    {
        Task<UsersDTO?> GetUserFromEmail(string emailUser);

        Task<UsersDTO?> GetUserFromId(string userId);

        Task CreateNewUser(NewUserDTO newUserModel);


        //Task ModifyUserData(UserDTO userData, string? email, string? userName);
    }
}
