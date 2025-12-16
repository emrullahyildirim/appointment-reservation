using Core.Entities;

namespace AuthService.Entities.DTOs
{
    public class ForgotPasswordDto : IDto
    {
        public string Email { get; set; } = string.Empty;
    }
}

