using MAContracts.Contracts.Mappers;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAServices.Mappers
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
            foreach(var tag in tags)
            {
                tagsDto.Add(TagMapperDto(tag));
            }
            return tagsDto;
        }
    }
}
