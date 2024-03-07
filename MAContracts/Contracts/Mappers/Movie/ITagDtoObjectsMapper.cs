using MADTOs.DTOs.EntityFrameworkDTOs.Movie;
using MAModels.EntityFrameworkModels.Movie;

namespace MAContracts.Contracts.Mappers.Movie
{
    public interface ITagDtoObjectsMapper
    {
        TagsDTO TagMappingDto(Tags tag);

        List<TagsDTO> TagMappingDtoList(List<Tags> tags);
    }
}
