using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services.Identity.User;
using MADTOs.DTOs.EntityFrameworkDTOs.Identity;
using MADTOs.DTOs.ModelsDTOs;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.Identity;
using MAModels.Enumerables.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MAServices.Services.Identity.User
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _database;

        private readonly IObjectsMapperDtoServices _mapperService;

        private readonly UserManager<Users> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IConfiguration _configuration;

        public UserServices(
            ApplicationDbContext database,
            IObjectsMapperDtoServices mapperService,
            UserManager<Users> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _database = database;
            _mapperService = mapperService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
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

        public async Task CreateNewUser(NewUserDTO newUserModel)
        {
            var user = await _userManager.FindByEmailAsync(newUserModel.Email);
            if (user != null) throw new DuplicateNameException();

            //EMAIL CONFIRMATION HARDCODATO
            Users newUser = new Users
            {
                Name = newUserModel.Name,
                LastName = newUserModel.LastName,
                UserName = newUserModel.UserName,
                Email = newUserModel.Email,
                BirthDate = newUserModel.BirthDate,
                SecurityStamp = new Guid().ToString(),
                Gender = newUserModel.Gender,
                Nationality = newUserModel.Nationality,
                EmailConfirmed = true                
            };
            string role = string.Equals(newUserModel.Email, _configuration["EmailAdmin"]) ? nameof(ERoleUser.AppAdmin) : nameof(ERoleUser.User);
            if (await _roleManager.FindByNameAsync(role) == null) await _roleManager.CreateAsync(new IdentityRole(role));
            await _userManager.CreateAsync(newUser, newUserModel.Password);
            await _userManager.AddToRoleAsync(newUser, role);
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
