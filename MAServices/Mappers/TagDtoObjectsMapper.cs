using MAContracts.Contracts.Mappers;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAServices.Mappers
{
    public class TagDtoObjectsMapper : ITagDtoObjectsMapper
    {
        public TagDtoObjectsMapper() { }

        public TagDTO TagMapperDto(Tag tag)
        {
            TagDTO tagDTO = new TagDTO
            {
                TagId = tag.TagId,
                TagName = tag.TagName
            };
            return tagDTO;
        }

        public List<TagDTO> TagMapperDtoList(List<Tag> tags)
        {
            List<TagDTO> tagsDto = new List<TagDTO>();
            foreach(var tag in tags)
            {
                tagsDto.Add(TagMapperDto(tag));
            }
            return tagsDto;
        }
    }
}
