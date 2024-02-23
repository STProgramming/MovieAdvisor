using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAContracts.Contracts.Mappers
{
    public interface IUserDtoObjectsMapper
    {
        UserDTO UserMapperDto(User user);
    }
}
