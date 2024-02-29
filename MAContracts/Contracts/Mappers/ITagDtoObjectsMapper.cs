using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAContracts.Contracts.Mappers
{
    public interface ITagDtoObjectsMapper
    {
        TagsDTO TagMapperDto(Tags tag);

        List<TagsDTO> TagMapperDtoList(List<Tags> tags);
    }
}
