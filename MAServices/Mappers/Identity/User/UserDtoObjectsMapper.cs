using MAContracts.Contracts.Mappers.Identity.User;
using MADTOs.DTOs.EntityFrameworkDTOs.Identity;
using MAModels.EntityFrameworkModels.Identity;

namespace MAServices.Mappers.Identity.User
{
    public class UserDtoObjectsMapper : IUserDtoObjectsMapper
    {
        public UserDtoObjectsMapper() { }

        public UsersDTO UserMappingDto(Users user)
        {
            UsersDTO userDto = new UsersDTO
            {
                Name = user.Name,
                LastName = user.LastName,
                UserName = user.UserName,
                EmailAddress = user.Email,
                BirthDate = user.BirthDate,
                Nationality = user.Nationality,
                Gender = user.Gender,
            };
            return userDto;
        }
    }
}
