using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MAServices.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _database;

        private readonly IConfiguration _config;

        public UserServices(ApplicationDbContext database, IConfiguration config)
        {
            _config = config;
            _database = database;
        }

        public async Task<ICollection<User>> GetAllUsers()
        {
            return await _database.Users.ToListAsync();
        }

        public async Task<User?> GetUserFromEmail(string emailUser)
        {
            return await _database.Users.Where(u => string.Equals(emailUser, u.EmailAddress)).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserFromId(int userId)
        {
            return await _database.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
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

        public async Task ModifyUserData(User userData, string? email, string? userName)
        {
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
