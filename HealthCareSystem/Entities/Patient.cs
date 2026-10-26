using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareSystem.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        // Many-to-Many: Patient <-> Doctor (through Appointment)
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();   // skip navigation
    }
}
