using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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


        public DbSet<Patient> Patient { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<AppointmentSlot> AppointmentSlot { get; set; }



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
        }
    }
}
