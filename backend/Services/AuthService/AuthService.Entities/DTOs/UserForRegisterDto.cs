using Core.Entities;

namespace AuthService.Entities.DTOs
{
    public class UserForRegisterDto : IDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string IdentityNumber { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Gender { get; set; }
    }
}

