using Core.Entities;

namespace AuthService.Entities.DTOs
{
    public class ChangePasswordDto : IDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

