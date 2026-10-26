using Microsoft.EntityFrameworkCore;
using HealthCareSystem.Data;
using HealthCareSystem.Entities;

using var db = new HealthCareContext();
db.Database.Migrate();

if (!db.Patients.Any())
{
    var p1 = new Patient { Name = "Mona Salah",  DateOfBirth = new DateTime(1995, 5, 20) };
    var p2 = new Patient { Name = "Omar Khaled", DateOfBirth = new DateTime(1988, 11, 3) };

    var cardio = new Doctor { Name = "Dr. Yasser", Specialization = "Cardiology" };
    var derma  = new Doctor { Name = "Dr. Laila",  Specialization = "Dermatology" };

    db.AddRange(p1, p2, cardio, derma);
    db.AddRange(
        new Appointment { Patient = p1, Doctor = cardio, AppointmentDate = new DateTime(2026, 11, 2, 10, 0, 0) },
        new Appointment { Patient = p1, Doctor = derma,  AppointmentDate = new DateTime(2026, 11, 5, 12, 30, 0) },
        new Appointment { Patient = p2, Doctor = cardio, AppointmentDate = new DateTime(2026, 11, 2, 11, 0, 0) });
    db.SaveChanges();
}

foreach (var a in db.Appointments.Include(a => a.Patient).Include(a => a.Doctor).OrderBy(a => a.AppointmentDate))
    Console.WriteLine($"{a.AppointmentDate:g}: {a.Patient.Name} -> {a.Doctor.Name} ({a.Doctor.Specialization})");

// Skip navigation: doctors of a patient / patients of a doctor
var mona = db.Patients.Include(p => p.Doctors).First(p => p.Name == "Mona Salah");
Console.WriteLine($"\n{mona.Name}'s doctors: {string.Join(", ", mona.Doctors.Select(d => d.Name))}");

var dr = db.Doctors.Include(d => d.Patients).First(d => d.Specialization == "Cardiology");
Console.WriteLine($"{dr.Name}'s patients: {string.Join(", ", dr.Patients.Select(p => p.Name))}");
