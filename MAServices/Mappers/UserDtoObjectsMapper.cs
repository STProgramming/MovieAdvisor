using MAContracts.Contracts.Mappers;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels.Identity;

namespace MAServices.Mappers
{
    public class UserDtoObjectsMapper : IUserDtoObjectsMapper
    {
        public UserDtoObjectsMapper() { }

        public UsersDTO UserMapperDto(Users user)
        {
            UsersDTO userDto = new UsersDTO
            {
                Name = user.Name,
                LastName = user.LastName,
                UserName = user.UserName,
                EmailAddress = user.Email,
                BirthDate = user.BirthDate
            };
            return userDto;
        }
    }
}
