using MADTOs.DTOs.EntityFrameworkDTOs.Identity;

namespace MADTOs.DTOs.EntityFrameworkDTOs.AI
{
    public class SessionsDTO
    {
        public int SessionId { get; set; }

        public List<RequestsDTO> Requests { get; set; } = new List<RequestsDTO>();

        public UsersDTO Users { get; set; }

        public DateTime DateTimeCreation { get; set; }
    }
}
