using AuthService.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataAccess.Concrete.EntityFramework
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext()
        {
        }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<ServiceClient> ServiceClients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.PasswordSalt).IsRequired();
                entity.Property(e => e.IdentityNumber).IsRequired().HasMaxLength(11);
            });

            // OperationClaim Configuration
            modelBuilder.Entity<OperationClaim>(entity =>
            {
                entity.ToTable("OperationClaims");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(200);
            });

            // UserOperationClaim Configuration
            modelBuilder.Entity<UserOperationClaim>(entity =>
            {
                entity.ToTable("UserOperationClaims");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.OperationClaimId }).IsUnique();

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserOperationClaims)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.OperationClaim)
                    .WithMany(oc => oc.UserOperationClaims)
                    .HasForeignKey(e => e.OperationClaimId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // EmailVerificationToken Configuration
            modelBuilder.Entity<EmailVerificationToken>(entity =>
            {
                entity.ToTable("EmailVerificationTokens");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Token).IsUnique();
                entity.Property(e => e.Token).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // PasswordResetToken Configuration
            modelBuilder.Entity<PasswordResetToken>(entity =>
            {
                entity.ToTable("PasswordResetTokens");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Token).IsUnique();
                entity.Property(e => e.Token).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ServiceClient Configuration (S2S Token için)
            modelBuilder.Entity<ServiceClient>(entity =>
            {
                entity.ToTable("ServiceClients");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ClientId).IsUnique();
                entity.HasIndex(e => e.ServiceName).IsUnique();
                entity.Property(e => e.ServiceName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ClientId).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ClientSecretHash).IsRequired();
                entity.Property(e => e.ClientSecretSalt).IsRequired();
                entity.Property(e => e.AllowedScopes).HasMaxLength(500);
            });

            // Seed default roles
            modelBuilder.Entity<OperationClaim>().HasData(
                new OperationClaim { Id = 1, Name = "Admin", Description = "Sistem yöneticisi" },
                new OperationClaim { Id = 2, Name = "User", Description = "Standart kullanıcı" },
                new OperationClaim { Id = 3, Name = "Moderator", Description = "Moderatör" }
            );
        }
    }
}

