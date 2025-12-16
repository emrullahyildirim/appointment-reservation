using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
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
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasMany(p => p.Appointments)
                      .WithOne(a => a.Patient)
                      .HasForeignKey(a => a.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<AppointmentSlot>(entity =>
            {
                entity.ToTable("AppointmentSlots");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.StartTime).IsRequired();
                entity.Property(s => s.EndTime).IsRequired();
                entity.Property(s => s.Status)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.HasIndex(s => new { s.DoctorId, s.StartTime, s.EndTime })
                      .IsUnique(); // aynı doktora aynı saatten 2 slot açılmasın

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
                      .HasConversion<string>() 
                      .HasMaxLength(20);

                entity.Property(a => a.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.HasOne<Patient>()
                      .WithMany()
                      .HasForeignKey(a => a.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<AppointmentSlot>()
                      .WithOne()
                      .HasForeignKey<Appointment>(a => a.SlotId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(a => a.SlotId).IsUnique(); 
            });
        }
    }
}
