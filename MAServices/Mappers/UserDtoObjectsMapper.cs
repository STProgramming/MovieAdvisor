using MAContracts.Contracts.Mappers;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAServices.Mappers
{
    public class UserDtoObjectsMapper : IUserDtoObjectsMapper
    {
        public UserDtoObjectsMapper() { }

        public UserDTO UserMapperDto(User user)
        {
            UserDTO userDto = new UserDTO
            {
                Name = user.Name,
                LastName = user.LastName,
                UserName = user.UserName,
                EmailAddress = user.EmailAddress,
                BirthDate = user.BirthDate
            };
            return userDto;
        }
    }
}
