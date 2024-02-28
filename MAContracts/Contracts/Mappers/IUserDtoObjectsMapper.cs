using MADTOs.DTOs;
using MAModels.EntityFrameworkModels.Identity;

namespace MAContracts.Contracts.Mappers
{
    public interface IUserDtoObjectsMapper
    {
        UsersDTO UserMapperDto(Users user);
    }
}
