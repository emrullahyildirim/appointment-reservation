using Core.Entities;

namespace AuthService.Entities.Concrete
{
    public class EmailVerificationToken : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual User User { get; set; } = null!;
    }
}

