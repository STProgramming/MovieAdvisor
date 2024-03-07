using MADTOs.DTOs.EntityFrameworkDTOs.Movie;
using MAModels.EntityFrameworkModels.Movie;

namespace MAContracts.Contracts.Mappers.Movie
{
    public interface ITagDtoObjectsMapper
    {
        TagsDTO TagMapperDto(Tags tag);

        List<TagsDTO> TagMapperDtoList(List<Tags> tags);
    }
}
