using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services.AI;
using MADTOs.DTOs.EntityFrameworkDTOs.AI;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.AI;
using MAModels.EntityFrameworkModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MAServices.Services.AI
{
    public class SessionServices : ISessionServices
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<Users> _userManager;

        private readonly IObjectsMapperDtoServices _dtoService;

        public SessionServices(
            ApplicationDbContext context,
            UserManager<Users> userManager,
            IObjectsMapperDtoServices objectsMapperDtoServices
            )
        {
            _context = context;
            _userManager = userManager;
            _dtoService = objectsMapperDtoServices;
        }

        public async Task<IList<SessionsDTO>> GetSessionsByUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new UnauthorizedAccessException();
            List<Sessions> sessions = await _context.Sessions.Where(s => string.Equals(s.User.Id, userId)).ToListAsync();
            List<SessionsDTO> resultDto = new List<SessionsDTO>();
            foreach (var session in sessions)
            {
                List<Requests> reqOfSession = new List<Requests>();
                reqOfSession = await _context.Requests.Where(r => r.SessionId == session.SessionId).ToListAsync();
                if (reqOfSession.Count > 0) 
                {
                    foreach(var request in reqOfSession)
                    {
                        var recomForRequest = await _context.Recommendations.Where(r => r.RequestId == request.RequestId).ToListAsync();
                        if(recomForRequest != null && recomForRequest.Count > 0)
                        {
                            resultDto.Add(_dtoService.SessionMapperDtoService(session, reqOfSession, recomForRequest));
                        }
                    }
                }
            }    
            return resultDto;
        }
    }
}
