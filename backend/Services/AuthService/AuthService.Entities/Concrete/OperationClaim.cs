using Core.Entities;

namespace AuthService.Entities.Concrete
{
    public class OperationClaim : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; } = new List<UserOperationClaim>();
    }
}

