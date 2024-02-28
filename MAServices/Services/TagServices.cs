using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;
using MAModels.Enumerables;
using Microsoft.EntityFrameworkCore;

namespace MAServices.Services
{
    public class TagServices : ITagServices
    {
        private readonly ApplicationDbContext _context;

        private readonly IObjectsMapperDtoServices _mapperService;

        public TagServices(ApplicationDbContext context,
            IObjectsMapperDtoServices mapperDtoService)            
        {
            _context = context;
            _mapperService = mapperDtoService;
        }
        public async Task<TagsDTO> GetTag(int tagId) 
        {
            return _mapperService.TagMapperDtoService(await _context.Tags.FindAsync(tagId));
        }   
        
        public async Task<List<TagsDTO>> GetAllTags()
        {
            return _mapperService.TagMapperDtoListService(await _context.Tags.OrderBy(x => x).ToListAsync());
        }

        public async Task CreateAllTags()
        {
            foreach (string name in Enum.GetNames(typeof(EMovieTags)))
            {
                if(!_context.Tags.Any(t => string.Equals(t.TagName, name)))
                {
                    Tags tag = new Tags
                    {
                        TagName = name,
                    };

                    await _context.Tags.AddAsync(tag);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
