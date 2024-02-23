using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAContracts.Contracts.Mappers
{
    public interface ITagDtoObjectsMapper
    {
        TagDTO TagMapperDto(Tag tag);

        List<TagDTO> TagMapperDtoList(List<Tag> tags);
    }
}
