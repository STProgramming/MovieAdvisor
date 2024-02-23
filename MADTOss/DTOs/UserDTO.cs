namespace MADTOs.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string EmailAddress { get; set; } = null!;

        public DateTime BirthDate { get; set; }
    }
}
