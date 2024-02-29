using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services.Identity.User;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MAServices.Services.Identity.Profile
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _database;

        private readonly IObjectsMapperDtoServices _mapperService;

        private readonly UserManager<Users> _userManager;

        public UserServices(
            ApplicationDbContext database,
            IObjectsMapperDtoServices mapperService,
            UserManager<Users> userManager)
        {
            _database = database;
            _mapperService = mapperService;
            _userManager = userManager;
        }


        public async Task<UsersDTO?> GetUserFromEmail(string emailUser)
        {
            try
            {
                return _mapperService.UserMapperDtoService(await _userManager.FindByEmailAsync(emailUser));
            }
            catch (Exception)
            {
                throw new NullReferenceException();
            }
        }

        public async Task<UsersDTO?> GetUserFromId(string userId)
        {
            return _mapperService.UserMapperDtoService(await _userManager.FindByIdAsync(userId));
        }

        [AllowAnonymous]
        public async Task CreateNewUser(UsersDTO newUserModel)
        {
            Users newUser = new Users
            {
                Name = newUserModel.Name,
                LastName = newUserModel.LastName,
                UserName = newUserModel.UserName,
                Email = newUserModel.EmailAddress,
                BirthDate = newUserModel.BirthDate,
                SecurityStamp = new Guid().ToString()
            };
            await _database.Users.AddAsync(newUser);
            await _database.SaveChangesAsync();
        }

        //TODO
        //[Authorize]
        //public async Task ModifyUserData(UserDTO userDto, string? email, string? userName)
        //{

        //    if (userData == null) throw new NullReferenceException();
        //    userData.EmailAddress = string.IsNullOrEmpty(email) ? userData.EmailAddress : email;
        //    userData.UserName = string.IsNullOrEmpty(userName) ? userData.UserName : userName;
        //    using (var transaction = _database.Database.BeginTransaction())
        //    {
        //        await transaction.CreateSavepointAsync("before modify user data");
        //        try
        //        {
        //            _database.Users.Update(userData);
        //            await _database.SaveChangesAsync();
        //        }
        //        catch (Exception)
        //        {
        //            await transaction.RollbackToSavepointAsync("before modify user data");
        //            throw new Exception("Something went wrong");
        //        }
        //    }
        //}
    }
}
