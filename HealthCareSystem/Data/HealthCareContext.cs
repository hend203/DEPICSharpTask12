using HealthCareSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthCareSystem.Data;

public class HealthCareContext : DbContext
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.;Database=HealthCareDb;Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(e =>
        {
            e.Property(p => p.Name).IsRequired().HasMaxLength(100);
            e.Property(p => p.DateOfBirth).HasColumnType("date");
        });

        modelBuilder.Entity<Doctor>(e =>
        {
            e.Property(d => d.Name).IsRequired().HasMaxLength(100);
            e.Property(d => d.Specialization).IsRequired().HasMaxLength(100);
        });

        // Many-to-Many: Patient <-> Doctor through Appointment
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Doctors)
            .WithMany(d => d.Patients)
            .UsingEntity<Appointment>(
                right => right.HasOne(a => a.Doctor)
                              .WithMany(d => d.Appointments)
                              .HasForeignKey(a => a.DoctorId),
                left => left.HasOne(a => a.Patient)
                            .WithMany(p => p.Appointments)
                            .HasForeignKey(a => a.PatientId),
                join =>
                {
                    // Date is in the key: a patient can see the same doctor on different dates
                    join.HasKey(a => new { a.PatientId, a.DoctorId, a.AppointmentDate });
                    join.ToTable("Appointments");
                });
    }
}
