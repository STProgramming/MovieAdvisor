using Azure;
using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAModels.Enumerables;
using MAServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MAServices.Services
{
    public class TagServices : ITagServices
    {
        private readonly ApplicationDbContext _context;

        public TagServices(ApplicationDbContext context)            
        {
            _context = context;
        }
        public async Task<Tag?> GetTag(int tagId) 
        {
            return await _context.Tags.FindAsync(tagId);
        }   
        
        public async Task<List<Tag>> GetAllTags()
        {
            return await _context.Tags.OrderBy(x => x).ToListAsync();
        }

        public async Task CreateAllTags()
        {
            foreach (string name in Enum.GetNames(typeof(EMovieTags)))
            {
                if(!_context.Tags.Any(t => string.Equals(t.TagName, name)))
                {
                    Tag tag = new Tag
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
