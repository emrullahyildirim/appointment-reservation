using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PatientService.Entities.Concrete;
using PatientService.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService.DataAccess.Concrete.EntityFramework
{
    public class PatientAppointmentContext : DbContext
    {
        public PatientAppointmentContext()
        {
        }

        public PatientAppointmentContext(DbContextOptions<PatientAppointmentContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // OnConfiguring is only called if options are not configured in Program.cs
            // Since we configure it in Program.cs, this should not be called in normal operation
            // But we keep it for compatibility with EfEntityRepositoryBase which requires parameterless constructor
            if (!optionsBuilder.IsConfigured)
            {
                // Use connection string from environment variable or appsettings.json
                var conn = Environment.GetEnvironmentVariable("CONNECTION_STRING");
                if (!string.IsNullOrEmpty(conn))
                {
                    optionsBuilder.UseNpgsql(conn);
                }
                else
                {
                    // Fallback for Docker: use postgres_db hostname
                    optionsBuilder.UseNpgsql("Host=localhost;Database=PatientAppointmentDb;Username=postgres;Password=1234");
                }
            }
        }


        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentSlot> AppointmentSlots { get; set; }
        public DbSet<Waitlist> Waitlists { get; set; }
        public DbSet<Doctor> Doctors { get; set; }             
        public DbSet<DoctorTitle> DoctorTitles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.IdentityNumber).IsUnique();
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.Property(e => e.IdentityNumber).IsRequired().HasMaxLength(11);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");

                entity.HasMany(p => p.Appointments)
                      .WithOne(a => a.Patient)
                      .HasForeignKey(a => a.PatientId);

            });


            modelBuilder.Entity<AppointmentSlot>(entity =>
            {
                entity.ToTable("AppointmentSlots");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.StartTime)
                      .HasColumnType("time")
                      .IsRequired();

                entity.Property(s => s.EndTime)
                      .HasColumnType("time")
                      .IsRequired();

                entity.Property(a => a.Status)
                      .IsRequired()
                      .HasConversion<int>();


                entity.Property(a => a.SlotDate)
                      .HasColumnType("date")
                      .IsRequired();


                entity.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");

                entity.HasIndex(s => new {
                    s.DoctorId,
                    s.SlotDate,
                    s.StartTime,
                    s.EndTime
                }).IsUnique();

                entity.HasOne(s => s.Appointment)
                      .WithOne(a => a.AppointmentSlot)
                      .HasForeignKey<Appointment>(a => a.SlotId)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointments");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Status)
                      .IsRequired()
                      .HasConversion<int>();

                entity.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");

                entity.HasIndex(a => a.SlotId).IsUnique();
            });


            modelBuilder.Entity<Waitlist>(entity =>
            {
                entity.ToTable("WaitlistEntries");
                entity.HasKey(w => w.Id);

                entity.Property(w => w.PreferredDate)
                      .HasColumnType("date")
                      .IsRequired();

                entity.Property(w => w.PreferredStartTime)
                      .HasColumnType("time");

                entity.Property(w => w.PreferredEndTime)
                      .HasColumnType("time");

                entity.Property(w => w.Status)
                      .IsRequired()
                      .HasConversion<int>();

                entity.Property(w => w.CreatedAt)
                      .HasDefaultValueSql("NOW()");

                entity.HasIndex(w => new { w.PatientId, w.DoctorId, w.PreferredDate, w.Status });
                entity.HasIndex(w => new { w.DoctorId, w.PreferredDate, w.QueuePosition });

                entity.HasOne(w => w.Patient)
                      .WithMany()
                      .HasForeignKey(w => w.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(w => w.NotifiedSlot)
                      .WithMany()
                      .HasForeignKey(w => w.NotifiedSlotId)
                      .OnDelete(DeleteBehavior.SetNull);
            });




            modelBuilder.Entity<DoctorTitle>(entity =>
            {
                entity.ToTable("DoctorTitles");
                entity.HasKey(t => t.Id);

                entity.Property(t => t.TitleName)
                    .IsRequired()
                    .HasMaxLength(100);
            });


            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctors");
                entity.HasKey(d => d.Id);
                entity.Property(d => d.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(d => d.LastName)
                    .IsRequired()
                    .HasMaxLength(50);


                entity.HasOne(d => d.DoctorTitle)
                    .WithMany(t => t.Doctors)
                    .HasForeignKey(d => d.DoctorTitleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(d => d.AppointmentSlots)
                    .WithOne(s => s.Doctor)
                    .HasForeignKey(s => s.DoctorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.Appointments)
                    .WithOne(a => a.Doctor)
                    .HasForeignKey(a => a.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });







            modelBuilder.Entity<DoctorTitle>().HasData(
                new DoctorTitle { Id = 1, TitleName = "Ortopedi", },
                new DoctorTitle { Id = 2, TitleName = "Dahiliye" },
                new DoctorTitle { Id = 3, TitleName = "Kulak Burun Boğaz" },
                new DoctorTitle { Id = 4, TitleName = "Noroloji" },
                new DoctorTitle { Id = 5, TitleName = "Diş" }
            );

            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { Id = 1, DoctorTitleId = 1, FirstName = "Kadir", LastName = "Turan" },
                new Doctor { Id = 2, DoctorTitleId = 2, FirstName = "Haluk", LastName = "Özdemir" },
                new Doctor { Id = 3, DoctorTitleId = 3, FirstName = "Neslihan", LastName = "Doğan" },
                new Doctor { Id = 4, DoctorTitleId = 4, FirstName = "Emrullah", LastName = "Yıldırım" },
                new Doctor { Id = 5, DoctorTitleId = 5, FirstName = "Mert", LastName = "Gökçe" }
             );

            // Seed Patients
            modelBuilder.Entity<Patient>().HasData(
                new Patient 
                { 
                    Id = 1, 
                    UserId = 1, 
                    FirstName = "Ahmet", 
                    LastName = "Yılmaz", 
                    BirthDate = new DateOnly(1990, 5, 15), 
                    Gender = "Erkek", 
                    IdentityNumber = "12345678901",
                    CreatedAt = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                },
                new Patient 
                { 
                    Id = 2, 
                    UserId = 2, 
                    FirstName = "Ayşe", 
                    LastName = "Kaya", 
                    BirthDate = new DateOnly(1985, 8, 22), 
                    Gender = "Kadın", 
                    IdentityNumber = "12345678902",
                    CreatedAt = new DateTime(2024, 1, 2, 11, 0, 0, DateTimeKind.Utc)
                },
                new Patient 
                { 
                    Id = 3, 
                    UserId = 3, 
                    FirstName = "Mehmet", 
                    LastName = "Demir", 
                    BirthDate = new DateOnly(1978, 3, 10), 
                    Gender = "Erkek", 
                    IdentityNumber = "12345678903",
                    CreatedAt = new DateTime(2024, 1, 3, 9, 0, 0, DateTimeKind.Utc)
                },
                new Patient 
                { 
                    Id = 4, 
                    UserId = 4, 
                    FirstName = "Fatma", 
                    LastName = "Çelik", 
                    BirthDate = new DateOnly(1995, 12, 1), 
                    Gender = "Kadın", 
                    IdentityNumber = "12345678904",
                    CreatedAt = new DateTime(2024, 1, 4, 14, 0, 0, DateTimeKind.Utc)
                },
                new Patient 
                { 
                    Id = 5, 
                    UserId = 5, 
                    FirstName = "Ali", 
                    LastName = "Öztürk", 
                    BirthDate = new DateOnly(2000, 7, 25), 
                    Gender = "Erkek", 
                    IdentityNumber = "12345678905",
                    CreatedAt = new DateTime(2024, 1, 5, 16, 0, 0, DateTimeKind.Utc)
                }
            );

            // Seed AppointmentSlots (gelecek tarihler için)
            modelBuilder.Entity<AppointmentSlot>().HasData(
                // Doktor 1 için slotlar
                new AppointmentSlot { Id = 1, DoctorId = 1, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(9, 30), Status = AppointmentSlotStatus.Available, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) },
                new AppointmentSlot { Id = 2, DoctorId = 1, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(9, 30), EndTime = new TimeOnly(10, 0), Status = AppointmentSlotStatus.Booked, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) },
                new AppointmentSlot { Id = 3, DoctorId = 1, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(10, 30), Status = AppointmentSlotStatus.Available, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) },
                new AppointmentSlot { Id = 4, DoctorId = 1, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(10, 30), EndTime = new TimeOnly(11, 0), Status = AppointmentSlotStatus.Available, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) },
                
                // Doktor 2 için slotlar
                new AppointmentSlot { Id = 5, DoctorId = 2, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(9, 30), Status = AppointmentSlotStatus.Booked, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) },
                new AppointmentSlot { Id = 6, DoctorId = 2, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(9, 30), EndTime = new TimeOnly(10, 0), Status = AppointmentSlotStatus.Available, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) },
                new AppointmentSlot { Id = 7, DoctorId = 2, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(10, 30), Status = AppointmentSlotStatus.Available, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) },
                
                // Doktor 3 için slotlar
                new AppointmentSlot { Id = 8, DoctorId = 3, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(14, 30), Status = AppointmentSlotStatus.Booked, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) },
                new AppointmentSlot { Id = 9, DoctorId = 3, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(14, 30), EndTime = new TimeOnly(15, 0), Status = AppointmentSlotStatus.Available, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) },
                new AppointmentSlot { Id = 10, DoctorId = 3, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(15, 0), EndTime = new TimeOnly(15, 30), Status = AppointmentSlotStatus.Available, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) },
                
                // Doktor 4 için slotlar
                new AppointmentSlot { Id = 11, DoctorId = 4, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(11, 30), Status = AppointmentSlotStatus.Available, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) },
                new AppointmentSlot { Id = 12, DoctorId = 4, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(11, 30), EndTime = new TimeOnly(12, 0), Status = AppointmentSlotStatus.Available, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) },
                
                // Doktor 5 için slotlar
                new AppointmentSlot { Id = 13, DoctorId = 5, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(16, 0), EndTime = new TimeOnly(16, 30), Status = AppointmentSlotStatus.Booked, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) },
                new AppointmentSlot { Id = 14, DoctorId = 5, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(16, 30), EndTime = new TimeOnly(17, 0), Status = AppointmentSlotStatus.Available, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) },
                new AppointmentSlot { Id = 15, DoctorId = 5, SlotDate = new DateOnly(2025, 1, 15), StartTime = new TimeOnly(17, 0), EndTime = new TimeOnly(17, 30), Status = AppointmentSlotStatus.Available, CreatedAt = new DateTime(2024, 12, 1, 10, 0, 0, DateTimeKind.Utc) }
            );

            // Seed Appointments (Booked slotlar için)
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment 
                { 
                    Id = 1, 
                    PatientId = 1, 
                    DoctorId = 1, 
                    SlotId = 2, 
                    Status = AppointmentStatus.Confirmed, 
                    CreatedAt = new DateTime(2024, 12, 10, 10, 0, 0, DateTimeKind.Utc) 
                },
                new Appointment 
                { 
                    Id = 2, 
                    PatientId = 2, 
                    DoctorId = 2, 
                    SlotId = 5, 
                    Status = AppointmentStatus.Confirmed, 
                    CreatedAt = new DateTime(2024, 12, 11, 11, 0, 0, DateTimeKind.Utc) 
                },
                new Appointment 
                { 
                    Id = 3, 
                    PatientId = 3, 
                    DoctorId = 3, 
                    SlotId = 8, 
                    Status = AppointmentStatus.Pending, 
                    CreatedAt = new DateTime(2024, 12, 12, 14, 0, 0, DateTimeKind.Utc) 
                },
                new Appointment 
                { 
                    Id = 4, 
                    PatientId = 4, 
                    DoctorId = 5, 
                    SlotId = 13, 
                    Status = AppointmentStatus.Confirmed, 
                    CreatedAt = new DateTime(2024, 12, 13, 16, 0, 0, DateTimeKind.Utc) 
                }
            );


        }
    }
}
