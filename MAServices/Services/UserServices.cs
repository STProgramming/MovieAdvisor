using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;
using Microsoft.EntityFrameworkCore;

namespace MAServices.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _database;

        private readonly IObjectsMapperDtoServices _mapperService;

        public UserServices(ApplicationDbContext database, IObjectsMapperDtoServices mapperService)
        {
            _database = database;
            _mapperService = mapperService;
        }

        public async Task<ICollection<User>> GetAllUsers()
        {
            return await _database.Users.ToListAsync();
        }

        public async Task<UserDTO?> GetUserFromEmail(string emailUser)
        {
            try
            {
                return _mapperService.UserMapperDtoService(await _database.Users.Where(u => string.Equals(emailUser, u.EmailAddress)).FirstOrDefaultAsync());
            }
            catch (Exception)
            {
                throw new NullReferenceException();
            }
        }

        public async Task<UserDTO?> GetUserFromId(int userId)
        {
            return _mapperService.UserMapperDtoService(await _database.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync());
        }

        public async Task CreateNewUser(UserDTO newUserModel)
        {
            User newUser = new User
            {
                Name = newUserModel.Name,
                LastName = newUserModel.LastName,
                UserName = newUserModel.UserName,
                EmailAddress = newUserModel.EmailAddress,
                BirthDate = newUserModel.BirthDate,
            };
            await _database.Users.AddAsync(newUser);
            await _database.SaveChangesAsync();
        }

        public async Task ModifyUserData(UserDTO userDto, string? email, string? userName)
        {
            User userData = await _database.Users.Where(u => string.Equals(u.EmailAddress, userDto.EmailAddress)).FirstOrDefaultAsync();
            if (userData == null) throw new NullReferenceException();
            userData.EmailAddress = string.IsNullOrEmpty(email) ? userData.EmailAddress : email;
            userData.UserName = string.IsNullOrEmpty(userName) ? userData.UserName : userName;
            using (var transaction = _database.Database.BeginTransaction())
            {
                await transaction.CreateSavepointAsync("before modify user data");
                try
                {
                    _database.Users.Update(userData);
                    await _database.SaveChangesAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackToSavepointAsync("before modify user data");
                    throw new Exception("Something went wrong");
                }
            }
        }
    }
}
