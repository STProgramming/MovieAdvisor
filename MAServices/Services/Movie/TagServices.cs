using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services.Movie;
using MADTOs.DTOs.EntityFrameworkDTOs.Movie;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.Movie;
using MAModels.Enumerables;
using Microsoft.EntityFrameworkCore;

namespace MAServices.Services.Movie
{
    public class TagServices : ITagServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _context;

        private readonly IObjectsMapperDtoServices _mapperService;

        public TagServices(IDbContextFactory<ApplicationDbContext> context,
            IObjectsMapperDtoServices mapperDtoService)
        {
            _context = context;
            _mapperService = mapperDtoService;
        }
        public async Task<TagsDTO> GetTag(int tagId)
        {
            using (var ctx = await _context.CreateDbContextAsync())
            {
                return _mapperService.TagMapperDtoService(await ctx.Tags.FindAsync(tagId));
            }
        }

        public async Task<List<TagsDTO>> GetAllTags()
        {
            using (var ctx = await _context.CreateDbContextAsync())
            {
                return _mapperService.TagMapperDtoListService(await ctx.Tags.OrderBy(x => x).ToListAsync());
            }
        }

        public async Task CreateAllTags()
        {
            using (var ctx = await _context.CreateDbContextAsync())
            {
                foreach (string name in Enum.GetNames(typeof(EMovieTags)))
                {
                    if (!ctx.Tags.Any(t => string.Equals(t.TagName, name)))
                    {
                        Tags tag = new Tags
                        {
                            TagName = name,
                        };

                        await ctx.Tags.AddAsync(tag);
                        await ctx.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
