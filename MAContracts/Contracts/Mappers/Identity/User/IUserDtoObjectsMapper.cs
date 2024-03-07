using MADTOs.DTOs.EntityFrameworkDTOs.Identity;
using MAModels.EntityFrameworkModels.Identity;

namespace MAContracts.Contracts.Mappers.Identity.User
{
    public interface IUserDtoObjectsMapper
    {
        UsersDTO UserMapperDto(Users user);
    }
}
