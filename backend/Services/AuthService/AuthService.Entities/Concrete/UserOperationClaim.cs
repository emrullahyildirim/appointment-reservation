using Core.Entities;

namespace AuthService.Entities.Concrete
{
    public class UserOperationClaim : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual User User { get; set; } = null!;
        public virtual OperationClaim OperationClaim { get; set; } = null!;
    }
}

