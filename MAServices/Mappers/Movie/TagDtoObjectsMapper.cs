using MAContracts.Contracts.Mappers.Movie;
using MADTOs.DTOs.EntityFrameworkDTOs.Movie;
using MAModels.EntityFrameworkModels.Movie;

namespace MAServices.Mappers.Movie
{
    public class TagDtoObjectsMapper : ITagDtoObjectsMapper
    {
        public TagDtoObjectsMapper() { }

        public TagsDTO TagMapperDto(Tags tag)
        {
            TagsDTO tagDTO = new TagsDTO
            {
                TagId = tag.TagId,
                TagName = tag.TagName
            };
            return tagDTO;
        }

        public List<TagsDTO> TagMapperDtoList(List<Tags> tags)
        {
            List<TagsDTO> tagsDto = new List<TagsDTO>();
            foreach (var tag in tags)
            {
                tagsDto.Add(TagMapperDto(tag));
            }
            return tagsDto;
        }
    }
}
