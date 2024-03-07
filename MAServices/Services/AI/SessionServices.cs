using MAModels.EntityFrameworkModels;

namespace MAServices.Services.AI
{
    public class SessionServices
    {
        private readonly ApplicationDbContext _context;

        public SessionServices(ApplicationDbContext context)
        {
            _context = context;
        }

    }
}
