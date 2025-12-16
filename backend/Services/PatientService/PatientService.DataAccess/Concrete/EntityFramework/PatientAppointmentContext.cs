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
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            var conn = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            if (!string.IsNullOrEmpty(conn))
            {
                optionsBuilder.UseNpgsql(conn);
            }
            else
            {
                // fallback (development vs.)
                optionsBuilder.UseNpgsql("Host=localhost;Database=PatientAppointmentDb;Username=postgres;Password=1234");
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


        }
    }
}
