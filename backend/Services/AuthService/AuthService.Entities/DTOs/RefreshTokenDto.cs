using Core.Entities;

namespace AuthService.Entities.DTOs
{
    public class RefreshTokenDto : IDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}

