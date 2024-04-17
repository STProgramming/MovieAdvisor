using MADTOs.DTOs.EntityFrameworkDTOs.Identity;
using MADTOs.DTOs.ModelsDTOs.Identity.User;

namespace MAContracts.Contracts.Services.Identity.User
{
    public interface IUserServices
    {
        Task<UsersDTO?> GetUserFromId(string userId);

        Task CreateNewUser(NewUserDTO newUserModel);


        //Task ModifyUserData(UserDTO userData, string? email, string? userName);
    }
}
