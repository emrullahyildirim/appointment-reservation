using Core.Entities;

namespace AuthService.Entities.DTOs
{
    public class ResetPasswordDto : IDto
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

